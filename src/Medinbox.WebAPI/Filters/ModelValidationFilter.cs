using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Medinbox.WebAPI.Filters
{
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                context.HttpContext.Items["ModelStateErrors"] = errors;
                context.Result = new BadRequestResult(); // Permet au middleware de gérer la réponse
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
