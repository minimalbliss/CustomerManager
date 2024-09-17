using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CustomerManager.Api.Services.Validators
{
    public static class FluentValidationExtensions
    {
        public static void AddToModelState(this ValidationException exception, ModelStateDictionary modelState)
        {
            foreach (ValidationFailure? error in exception.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
