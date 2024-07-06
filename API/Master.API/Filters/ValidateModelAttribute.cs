using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Master.API.Filters;

/// <summary>
///     ValidateModelAttribute
/// </summary>
public class ValidateModelAttribute : ActionFilterAttribute
{
    /// <summary>
    ///     OnActionExecuting
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid) context.Result = new BadRequestObjectResult(context.ModelState);
    }
}