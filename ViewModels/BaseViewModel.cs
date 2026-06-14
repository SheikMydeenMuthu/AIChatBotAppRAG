using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AIChatBotAppRAG.Data;
using AIChatBotAppRAG.Models;
using AIChatBotAppRAG.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace AIChatBotAppRAG.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        private readonly DatabaseHelper _dbHselper;
        private readonly ChatBotService _chatBotService;
        private Conversation _currentConversation = new();

        private readonly DatabaseHelper _dbHelper;

        public BaseViewModel(DatabaseHelper dbHelper, ChatBotService chatBotService)
        {
            _dbHelper = dbHelper;
            _chatBotService = chatBotService;
        }
    }
}
