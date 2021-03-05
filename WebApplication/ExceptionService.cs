using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    public class ExceptionService
    {
        private readonly ILogger<ExceptionService> _logger;

        public ExceptionService(ILogger<ExceptionService> logger)
        {
            _logger = logger;
        }

        public async Task Error(Exception e)
        {
            _logger.LogError(e.Message + "   "+e.InnerException?.Message);
            await Task.Delay(10000);
            _logger.LogError(e.Message + "writed");
        }

        public async Task Error(string message)
        {
            _logger.LogError(message);
            await Task.Delay(10000);
            _logger.LogError(message + "writed");
        }
    }
}