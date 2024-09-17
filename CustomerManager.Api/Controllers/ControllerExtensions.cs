using FluentValidation;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManager.Api.Controllers
{
    public static class ControllerExtensions
    {
        public static IActionResult ToActionResult<TResult>(this Result<TResult> result)
        {
            return result.Match<IActionResult>(obj =>
            {
                return new OkObjectResult(true);
            }, exception =>
            {
                if (exception is ValidationException validationException)
                {
                    return new BadRequestObjectResult(validationException);
                }
                return new StatusCodeResult(500);
            });
        }
    }
}
