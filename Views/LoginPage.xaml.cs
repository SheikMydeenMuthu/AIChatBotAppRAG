using AIChatBotAppRAG.ViewModels;
using Microsoft.Maui.Controls;

namespace AIChatBotAppRAG.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
