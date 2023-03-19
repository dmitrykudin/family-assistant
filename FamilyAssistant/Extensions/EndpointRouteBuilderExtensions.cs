namespace FamilyAssistant.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static ControllerActionEndpointConventionBuilder MapBotWebhookRoute<T>(
        this IEndpointRouteBuilder endpoints,
        string route)
    {
        var controllerName = typeof(T).Name.Replace("Controller", string.Empty);
        var actionName = typeof(T).GetMethods()[0].Name;

        return endpoints.MapControllerRoute(
            name: "bot_webhook",
            pattern: route,
            defaults: new { controller = controllerName, action = actionName });
    }
}
