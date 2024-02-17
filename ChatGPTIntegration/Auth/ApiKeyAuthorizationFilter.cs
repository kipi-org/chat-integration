using ChatGPTIntegration.Auth;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

public class ApiKeyAuthorizationFilter : IAuthorizationFilter
{
    private readonly IApiKeyService _apiKeyService;

    public ApiKeyAuthorizationFilter(IApiKeyService apiKeyService)
    {
        _apiKeyService = apiKeyService;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Получите ключ из запроса (например, из заголовка)
        var apiKeyFromRequest = context.HttpContext.Request.Headers["ApiKey"];

        // Проверьте соответствие ключа сервиса
        if (!_apiKeyService.IsValidApiKey(apiKeyFromRequest))
        {
            context.Result = new Microsoft.AspNetCore.Mvc.StatusCodeResult(401); // Неавторизованный доступ
        }
    }
}
