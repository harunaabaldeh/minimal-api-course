using FluentValidation;
using Library.Api.Models;

namespace Library.Api.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(book => book.Isbn).Matches(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$")
            .WithMessage("Value was not valid ISBN-13");
        RuleFor(b => b.Title).NotEmpty();
        RuleFor(b => b.ShortDescription).NotEmpty();
        RuleFor(b => b.PageCount).GreaterThan(0);
        RuleFor(b => b.Author).NotEmpty();
    }
}