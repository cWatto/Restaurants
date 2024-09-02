using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];
    public CreateRestaurantCommandValidator()
    {
        RuleFor(x => x.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Description is required");

        RuleFor(dto => dto.Category)
            .Must(validCategories.Contains)
            .WithMessage("Invalid category");

        RuleFor(dto => dto.ContactEmail)
          .EmailAddress()
          .WithMessage("Email Address is invalid");

        RuleFor(dto => dto.PostalCode)
          .Matches(@"^\d{2}-\d{3}$")
          .WithMessage("Postal Code is invalid (XX-XXX)");
    }
}
