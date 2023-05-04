using FamilyAssistant.Filters;
using FamilyAssistant.Services.UpdateHandlers;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace FamilyAssistant.Controllers;

[ApiController]
[Route("[controller]")]
public class BotController : ControllerBase
{
    [HttpPost]
    [ValidateTelegramBotRequest]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] WebHookUpdateHandler handler,
        CancellationToken token)
    {
        await handler.HandleUpdateAsync(update, token);

        return Ok();
    }
}
