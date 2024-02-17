using System;
using ChatGPTIntegration.Auth;
using Microsoft.Extensions.Configuration;

namespace ChatGPTIntegration.Auth
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IConfiguration _configuration;

        public ApiKeyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsValidApiKey(string apiKey)
        {
            var expectedApiKey = _configuration.GetSection("middle_key").Value.ToString();
            return apiKey == expectedApiKey;
        }
    }
}