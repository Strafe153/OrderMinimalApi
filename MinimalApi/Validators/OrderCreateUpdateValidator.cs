using Core.Dtos;
using FluentValidation;
using System.Text.RegularExpressions;

namespace MinimalApi.Validators;

public class OrderCreateUpdateValidator : AbstractValidator<OrderCreateUpdateDto>
{
    public OrderCreateUpdateValidator()
    {
        RuleFor(o => o.CustomerName)
            .NotEmpty()
            .WithMessage("CustomerName is required")
            .MinimumLength(5)
            .WithMessage("CustomerName must be at least 5 characters long")
            .MaximumLength(30)
            .WithMessage("CustomerName must not be longer than 30 characters")
            .Must(BeOfFormat)
            .WithMessage("CustomerName must be of format 'Firstname Lastname'");

        RuleFor(o => o.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MinimumLength(5)
            .WithMessage("Address must be at least 5 characters long")
            .MaximumLength(50)
            .WithMessage("Address must not be longer than 50 characters");

        RuleFor(o => o.Product)
            .NotEmpty()
            .WithMessage("Product is required")
            .MinimumLength(2)
            .WithMessage("Product must be at least 2 characters long")
            .MaximumLength(50)
            .WithMessage("Product must not be longer than 50 characters");

        RuleFor(w => w.Price)
            .GreaterThan(0)
            .WithMessage("Damage must be greater than 0")
            .LessThan(100001)
            .WithMessage("Damage must not be greater than 100000");
    }

    private bool BeOfFormat(string name) =>
        name is not null 
            ? new Regex("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+").Match(name).Length > 0
            : false;
}
