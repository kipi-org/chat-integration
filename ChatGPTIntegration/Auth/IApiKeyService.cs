using System;
namespace ChatGPTIntegration.Auth
{
    public interface IApiKeyService
    {
        bool IsValidApiKey(string apiKey);
    }
}

