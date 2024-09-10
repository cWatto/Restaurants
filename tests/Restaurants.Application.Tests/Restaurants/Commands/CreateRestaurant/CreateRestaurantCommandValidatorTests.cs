﻿using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        var command = new CreateRestaurantCommand()
        {
            Name = "Test",
            Description = "Description is required",
            Category = "Italian",
            ContactEmail = "test@test.com",
            PostalCode = "12-345",
        };

        var validator = new CreateRestaurantCommandValidator();

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Category = "Ita",
            ContactEmail = "test@test.com",
            PostalCode = "12345",
        };

        var validator = new CreateRestaurantCommandValidator();

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        result.ShouldHaveValidationErrorFor(c => c.Description);
    }

    [Theory]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Indian")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
    {
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand()
        {
            Category = category
        };

        var result = validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }


    [Theory]
    [InlineData("10220")]
    [InlineData("102-20")]
    [InlineData("10 220")]
    [InlineData("10-2 20")]
    public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrorsForPostalCodeProperty(string postalCode)
    {
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand()
        {
            PostalCode = postalCode
        };

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }
}