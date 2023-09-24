using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationTestController :  ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigurationTestController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        [HttpGet]
        public string Get()
        {
            var message = _configuration.GetSection("Messages:Welcome").Get<string>();        
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")??"";

            return $"{environment} - {message}";
        }
    }
}
