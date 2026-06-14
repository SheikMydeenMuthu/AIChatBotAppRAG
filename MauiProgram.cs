using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using AIChatBotAppRAG.Data;
using AIChatBotAppRAG.Services;
using AIChatBotAppRAG.ViewModels;
using AIChatBotAppRAG.Views;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace AIChatBotAppRAG
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "chatbot.db3");
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("AIChatBotAppRAG.appsettings.json");

            if (stream != null)
            {
                var fileConfig = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                builder.Configuration.AddConfiguration(fileConfig);
            }

            var baseUrl = builder.Configuration["AzureFunctionBaseUrl"];
            var appCode = builder.Configuration["AzureFunctionAppCode"];
            var chatUrl = baseUrl + appCode;
            builder.Services.AddSingleton<DatabaseHelper>(s => new DatabaseHelper(dbPath));
            builder.Services.AddSingleton<ChatBotService>(s => new ChatBotService(chatUrl));

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<ChatViewModel>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ChatPage>();

            return builder.Build();
        }
    }
}
