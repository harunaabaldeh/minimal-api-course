using FluentValidation;
using Library.Api.Auth;
using Library.Api.Data;
using Library.Api.Endpoints.Internal;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // WebRootPath = "./wwwroot",
    // EnvironmentName = Environment.GetEnvironmentVariable("env"),
    // ApplicationName = "Library.Api"
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AnyOrigin", x => x.AllowAnyOrigin());
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.IncludeFields = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsetting.Local.json", true, true);
builder.Services.AddAuthentication(ApiKeySchemeConstants.SchemeName)
    .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthHandler>
        (ApiKeySchemeConstants.SchemeName, _ => { });
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(
    builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<DatabaseInitializer>();
// builder.Services.AddSingleton<IBookService, BookService>();

builder.Services.AddEndpoints<Program>(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.UseEndpoints<Program>();


//Db init here
var dbInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await dbInitializer.InitializeAsync();

app.Run();