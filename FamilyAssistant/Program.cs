using FamilyAssistant.BotCommands;
using FamilyAssistant.Config;
using FamilyAssistant.DAL;
using FamilyAssistant.DAL.Repositories;
using FamilyAssistant.Extensions;
using FamilyAssistant.Interfaces.Commands;
using FamilyAssistant.Interfaces.DAL.Repositories;
using FamilyAssistant.Interfaces.Services;
using FamilyAssistant.Services;
using FamilyAssistant.Services.Polling;
using FamilyAssistant.Services.UpdateHandlers;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbSettings>(builder.Configuration.GetRequiredSection(nameof(DbSettings)));

var botConfigurationSection = builder.Configuration.GetRequiredSection(nameof(BotConfiguration));
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>();

builder.Services
    .AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((client, sp) =>
    {
        var config = sp.GetConfiguration<BotConfiguration>();
        var options = new TelegramBotClientOptions(config.BotToken);

        return new TelegramBotClient(options, client);
    });

if (botConfiguration!.UseWebHook)
{
    builder.Services.AddScoped<WebHookUpdateHandler>();

    // TODO: Sort out WebHook setup
}
else
{
    builder.Services.AddScoped<PollingUpdateHandler>();
    builder.Services.AddScoped<ReceiverService>();
    builder.Services.AddHostedService<PollingService>();
}

builder.Services.AddHostedService<SetBotCommandsService>();

builder.Services.AddScoped<IBotCommandFactory, BotCommandFactory>();
builder.Services.AddScoped<BuyProductBotCommand>();
builder.Services.AddScoped<GetProductsToBuyBotCommand>();
builder.Services.AddScoped<ToggleBuyProductQueryCommand>();

builder.Services.AddSingleton(PostgresDataSourceBuilder.Build(builder.Configuration["DbSettings:ConnectionString"]!));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductToBuyRepository, ProductToBuyRepository>();

builder.Services.AddScoped<IProductToBuyService, ProductToBuyService>();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
app.Run();
