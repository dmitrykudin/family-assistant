using FamilyAssistant.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace FamilyAssistant.Filters;

/// <summary>
/// Check for "X-Telegram-Bot-Api-Secret-Token"
/// Read more: <see href="https://core.telegram.org/bots/api#setwebhook"/> "secret_token"
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ValidateTelegramBotRequestAttribute : TypeFilterAttribute
{
    public ValidateTelegramBotRequestAttribute()
        : base(typeof(ValidateTelegramBotFilter))
    {
    }

    private class ValidateTelegramBotFilter : IActionFilter
    {
        private const string SecretTokenHeader = "X-Telegram-Bot-Api-Secret-Token";
        private readonly string _secretToken;

        public ValidateTelegramBotFilter(IOptions<BotConfiguration> options)
        {
            var botConfiguration = options.Value;
            _secretToken = botConfiguration.SecretToken;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsValidRequest(context.HttpContext.Request))
            {
                context.Result = new ObjectResult($"{SecretTokenHeader} is invalid")
                {
                    StatusCode = 403,
                };
            }
        }

        private bool IsValidRequest(HttpRequest request)
        {
            var isSecretTokenProvided = request.Headers.TryGetValue(SecretTokenHeader, out var secretTokenHeader);

            return isSecretTokenProvided
                   && string.Equals(secretTokenHeader, _secretToken, StringComparison.Ordinal);
        }
    }
}
