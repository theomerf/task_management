using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.ActionFilters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            var dtoParam = context.ActionArguments
                .FirstOrDefault(x => IsDtoType(x.Value?.GetType()));

            if (dtoParam.Value is null)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    message = "Geçersiz istek verisi",
                    errors = new { general = new[] { $"Controller: {controller}, Action: {action} - DTO nesnesi bulunamadı." } }
                });
                return;
            }

            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? new string[0]
                    );

                context.Result = new UnprocessableEntityObjectResult(new
                {
                    message = "Doğrulama hatası.",
                    errors = errors
                });
            }
        }
        
        private bool IsDtoType(Type? type)
        {
            if (type is null)
                return false;

            return type.Name.Contains("Dto", StringComparison.OrdinalIgnoreCase);
        }
    }
}
