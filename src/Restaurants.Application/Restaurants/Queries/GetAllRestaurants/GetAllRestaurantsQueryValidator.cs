using FluentValidation;
using Restaurants.Application.Restaurants.Dto;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowedPageSizes = [5, 10, 15, 30];
    private string[] allowedSortByColumnNames = [
        nameof(RestaurantDto.Name),
        nameof(RestaurantDto.Category),
        nameof(RestaurantDto.Description)];

    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(allowedPageSizes.Contains)
            .WithMessage($"Page size must be: {string.Join(", ", allowedPageSizes)}");

        RuleFor(r => r.SortBy)
            .Must(allowedSortByColumnNames.Contains)
            .When(q => q.SortBy != null)
            .WithMessage($"Allowable sorting columns are {string.Join(", ", allowedPageSizes)}");
    }
}
