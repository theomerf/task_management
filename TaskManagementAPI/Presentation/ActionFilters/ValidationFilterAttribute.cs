using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Presentation.ActionFilters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString() ?? "Unknown";
            var action = context.RouteData.Values["action"]?.ToString() ?? "Unknown";

            if (!context.ModelState.IsValid)
            {
                var validationErrors = ExtractValidationErrors(context.ModelState);

                if (validationErrors.Any())
                {
                    context.Result = new UnprocessableEntityObjectResult(new
                    {
                        message = "Doğrulama hatası.",
                        errors = validationErrors,
                        timestamp = DateTime.UtcNow
                    });
                    return;
                }
            }

            var dtoParameters = context.ActionArguments
                .Where(x => IsDtoType(x.Value?.GetType()))
                .ToList();

            if (!dtoParameters.Any())
            {
                context.Result = new BadRequestObjectResult(new
                {
                    message = "DTO nesnesi bulunamadı veya eksik parametreler var.",
                    errors = new
                    {
                        general = new[] { $"Controller: {controller}, Action:  {action} - Beklenen DTO parametreleri gönderilmedi." }
                    }
                });
                return;
            }

            foreach (var dtoParam in dtoParameters)
            {
                ValidateDtoProperties(context, dtoParam.Value, dtoParam.Key);
            }
        }

        private Dictionary<string, object> ExtractValidationErrors(ModelStateDictionary modelState)
        {
            var errors = new Dictionary<string, object>();

            foreach (var entry in modelState)
            {
                if (entry.Value?.Errors.Count > 0)
                {
                    var fieldName = ConvertToCamelCase(entry.Key);
                    var errorMessages = entry.Value.Errors
                        .Select(e => e.ErrorMessage)
                        .Where(msg => !string.IsNullOrWhiteSpace(msg))
                        .ToList();

                    if (errorMessages.Any())
                    {
                        errors[fieldName] = errorMessages.Count == 1
                            ? (object)errorMessages[0]
                            : errorMessages;
                    }
                }
            }

            return errors;
        }

        private void ValidateDtoProperties(ActionExecutingContext context, object? dtoObject, string parameterName)
        {
            if (dtoObject == null)
                return;

            var errors = new Dictionary<string, object>();
            var dtoType = dtoObject.GetType();
            var properties = dtoType.GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(dtoObject);
                var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();

                var propertyErrors = new List<string>();

                foreach (var attribute in validationAttributes)
                {
                    if (!attribute.IsValid(propertyValue))
                    {
                        var errorMessage = attribute.ErrorMessage ?? $"{property.Name} doğrulaması başarısız.";
                        propertyErrors.Add(errorMessage);
                    }
                }

                if (propertyErrors.Any())
                {
                    var fieldName = ConvertToCamelCase(property.Name);
                    errors[fieldName] = propertyErrors.Count == 1
                        ? (object)propertyErrors[0]
                        : propertyErrors;
                }
            }

            if (errors.Any())
            {
                context.Result = new UnprocessableEntityObjectResult(new
                {
                    message = "Doğrulama hatası.",
                    errors = errors,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        private string ConvertToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length == 0)
                return str;

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        private bool IsDtoType(Type? type)
        {
            if (type is null)
                return false;

            var typeName = type.Name;
            return typeName.EndsWith("Dto", StringComparison.OrdinalIgnoreCase) ||
                   typeName.Contains("Dto", StringComparison.OrdinalIgnoreCase);
        }
    }
}