using API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Extensions
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultcontext = await next();

            if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;

            var memberId = resultcontext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ;

            var dbContext = resultcontext.HttpContext.RequestServices.GetRequiredService<DatingContext>();

            await dbContext.Members.Where(m => m.Id == memberId ).ExecuteUpdateAsync(setter => setter.SetProperty(m => m.LastActive, DateTime.UtcNow));

            await dbContext.SaveChangesAsync();

        }
    }
}
