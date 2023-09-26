using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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

        [HttpGet("GetTestSecret")]
        public string GetTestSecret()
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };

            string vaultName = _configuration["KeyVaultName"];

            var client = new SecretClient(new Uri($"https://{vaultName}.vault.azure.net/"), new DefaultAzureCredential(), options);

            KeyVaultSecret secret = client.GetSecret("mySecret");

            string secretValue = secret.Value;

            return secretValue;
        }
    }
}
