using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = factoryMethod(value);
            if (result.IsSuccess)
                return;

            context.AddFailure(result.Error.Serialize());
        });
    }
}