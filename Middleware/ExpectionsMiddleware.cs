using API.Exceptions;
using System.Text.Json;

namespace API.Middleware
{
    public class ExpectionsMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        private readonly ILogger<ExpectionsMiddleware> _logger;

        private readonly IHostEnvironment _environment;

        public ExpectionsMiddleware(RequestDelegate requestDelegate , 
            ILogger<ExpectionsMiddleware> logger ,
            IHostEnvironment environment
            )
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
            _environment = environment;
        }

        public async  Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _requestDelegate.Invoke(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"{message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var respon = _environment.IsDevelopment() ?
                    new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace!)
                    : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error ");


                var responseJson = JsonSerializer.Serialize(respon);


              await  context.Response.WriteAsync(responseJson);




            }
        }
    }
}
