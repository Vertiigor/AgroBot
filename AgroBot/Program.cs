using AgroBot.Bot;
using AgroBot.Commands;
using AgroBot.Data;
using AgroBot.Keyboards;
using AgroBot.Models;
using AgroBot.Pipelines.CropCreation;
using AgroBot.Pipelines.Registration;
using AgroBot.Repositories.Abstractions;
using AgroBot.Repositories.Implementations;
using AgroBot.Services.Abstractions;
using AgroBot.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace AgroBot;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseContext") ?? throw new InvalidOperationException("Connection string 'BookStoreContext' not found.")));

        var botApiToken = builder.Configuration["BotConnection:ApiToken"]
            ?? throw new InvalidOperationException("API token 'BotConnection:ApiToken' not found.");

        // Configure Identity to use the custom ApplicationUser model
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();

        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botApiToken));
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IPipelineContextService, PipelineContextService>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IPipelineContextRepository, PipelineContextRepository>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddScoped<CallbackHandler>();
        builder.Services.AddScoped<PipelineHandler>();
        builder.Services.AddScoped<KeyboardMarkupBuilder>();
        builder.Services.AddScoped<BotMessageSender>();
        builder.Services.AddScoped<ICommand, StartCommand>();
        builder.Services.AddScoped<ICommand, NewCropCommand>();
        builder.Services.AddScoped<BotMessageHandler>();
        builder.Services.AddScoped<CommandDispatcher>();
        builder.Services.AddSingleton<BotClient>();
        builder.Services.AddScoped<ICropRepository, CropRepository>();
        builder.Services.AddScoped<ICropService, CropService>();


        // Register the RegistrationPipeline and its steps
        builder.Services.AddScoped<RegistrationPipeline>();
        builder.Services.AddScoped<BotInfoStep>();

        // Register the CropCreationPipeline and its steps
        builder.Services.AddScoped<CropCreationPipeline>();
        builder.Services.AddScoped<NameStep>();
        builder.Services.AddScoped<CultureStep>();
        builder.Services.AddScoped<SowingDateStep>();
        builder.Services.AddScoped<SubstrateStep>();
        builder.Services.AddScoped<CollectionDateStep>();


        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        //builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        var botHandler = app.Services.GetRequiredService<BotClient>();
        botHandler.StartReceiving();

        app.Run();
    }
}
