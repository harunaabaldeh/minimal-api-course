namespace Library.Api.Tests.Integration;

public abstract class ValidationError
{
    public string PropertyName { get; set; } = default!;

    public string ErrorMessage { get; set; } = default!;
}