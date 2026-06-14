using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using AIChatBotAppRAG.Views;
using AIChatBotAppRAG.ViewModels;

namespace AIChatBotAppRAG
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

             var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>();

            Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
             var loginPage = IPlatformApplication.Current!
        .Services
        .GetRequiredService<LoginPage>();
            return new Window(loginPage);            
        }
    }
}
