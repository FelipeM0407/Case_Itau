using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CaseItau.API.Infrastructure.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Erro não tratado.");

            context.Result = new ObjectResult("Ocorreu um erro interno no servidor.")
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}
