using AIChatBotAppRAG.ViewModels;
using Microsoft.Maui.Controls;

namespace AIChatBotAppRAG.Views
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage(ChatViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
