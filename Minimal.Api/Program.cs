using Microsoft.AspNetCore.Mvc;
using Minimal.Api;

var builder = WebApplication.CreateBuilder(args);
//adding Swagger support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<PeopleService>();
builder.Services.AddSingleton<GuidGenerator>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("get", () => "This is a GET");
app.MapPost("post", () => "This is a POST");
app.MapPut("put", () => "This is a PUT");
app.MapPatch("patch", () => "This is PATCH");
app.MapMethods("option-or-head", new []{"HEAD", "OPTIONS"},() => "Hello from options or head");
app.MapDelete("delete", () => "This is a DELETE");


var handler = () => "This is coming from a var";
app.MapGet("handler", handler);

app.MapGet("from-class", Example.SomeMethod);
// Route parameters and rules
app.MapGet("get-params/{age}", (int age) =>
{
    return $"Age provided was {age}";
});

//parameter binding
app.MapGet("people/search", (string? searchTerm, PeopleService peopleService) =>
{
    if (searchTerm is null) return Results.Ok();

    var results = peopleService.Search(searchTerm);
    return Results.Ok(results);
});

app.MapGet("mix/{routeParam}", ([FromRoute]string routeParam, [FromQuery(Name = "query")]int queryParam, [FromServices]GuidGenerator guidGenerator) =>
{
    return $"{routeParam} {queryParam} {guidGenerator.NewGuid}";
});

app.MapPost("people", (Person person) =>
{
    return Results.Ok(person);
});

// special parameter types
app.MapGet("httpcontext-1", async (context) =>
{
    await context.Response.WriteAsync("Hello from HttpContext 1");
});

app.MapGet("http", async (HttpRequest request, HttpResponse response) =>
{
    var queries = request.QueryString.Value;
    await response.WriteAsync($"Hello from HttpResponse. Queries were: {queries}");
});

app.MapGet("logger", (ILogger<Program> logger) =>
{
    const string something = "Something";
    logger.LogInformation($"Hello from endpoint {something}");
});

app.Run();