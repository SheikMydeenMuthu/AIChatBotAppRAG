using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AIChatBotAppRAG.Data;
using AIChatBotAppRAG.Models;
using AIChatBotAppRAG.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AIChatBotAppRAG.ViewModels
{
    public partial class ChatViewModel : ObservableObject
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly ChatBotService _chatBotService;
        private Conversation _currentConversation = new();

        [ObservableProperty]
        private string messageText = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Message> messages = new();

        [ObservableProperty]
        private ObservableCollection<Conversation> conversations = new();

        [ObservableProperty]
        private bool isConversationPopupVisible;

        public ChatViewModel(DatabaseHelper dbHelper, ChatBotService chatBotService)
        {
            _dbHelper = dbHelper;
            _chatBotService = chatBotService;
            LoadConversations();
            InitializeConversation();
        }

        private async void InitializeConversation()
        {
            var conversations = await _dbHelper.GetConversationsAsync();
            if (conversations.Count == 0)
            {
                _currentConversation = new Conversation
                {
                    Title = "New Conversation",
                    Timestamp = DateTime.Now
                };
                await _dbHelper.SaveConversationAsync(_currentConversation);
            }
            else
            {
                _currentConversation = conversations[0];
            }
            await LoadMessages();
        }

        [RelayCommand]
        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageText))
                return;

            var userMessage = new Message
            {
                ConversationId = _currentConversation.Id,
                Content = MessageText,
                IsUser = true,
                Timestamp = DateTime.Now
            };
             var chatRequest = new ChatRequest
             {
                 UserInput = MessageText,
                 Model = "abacusai/dracarys-llama-3.1-70b-instruct",
                 Provider = "Nvidia",
                 InputType = "query",
                 UseRag = true
             };

            Messages.Add(userMessage);
            await _dbHelper.SaveMessageAsync(userMessage);

            var response = await _chatBotService.SendMessageAsync(chatRequest);

            var botMessage = new Message
            {
                ConversationId = _currentConversation.Id,
                Content = response,
                IsUser = false,
                Timestamp = DateTime.Now
            };

            Messages.Add(botMessage);
            await _dbHelper.SaveMessageAsync(botMessage);

            if (Messages.Count <= 2)
            {
                _currentConversation.Title = MessageText.Length > 20 ? MessageText.Substring(0, 20) + "..." : MessageText;
                await _dbHelper.SaveConversationAsync(_currentConversation);
                LoadConversations();
            }

            MessageText = string.Empty;
            await _dbHelper.LimitConversationsAsync(10);
        }

        [RelayCommand]
        private void ShowConversations()
        {
            IsConversationPopupVisible = true;
        }

        [RelayCommand]
        private void HideConversations()
        {
            IsConversationPopupVisible = false;
        }

        [RelayCommand]
        private async Task SelectConversation(Conversation conversation)
        {
            _currentConversation = conversation;
            await LoadMessages();
            IsConversationPopupVisible = false;
        }

        [RelayCommand]
        private async Task StartNewConversation()
        {
            _currentConversation = new Conversation
            {
                Title = "New Conversation",
                Timestamp = DateTime.Now
            };
            await _dbHelper.SaveConversationAsync(_currentConversation);
            Messages.Clear();
            LoadConversations();
        }

        [RelayCommand]
        private async Task DeleteConversation(Conversation conversation)
        {
            await _dbHelper.DeleteConversationAsync(conversation);
            LoadConversations();
            if (_currentConversation?.Id == conversation.Id)
            {
                await StartNewConversation();
            }
        }

        private async void LoadConversations()
        {
            var convs = await _dbHelper.GetConversationsAsync();
            Conversations.Clear();
            foreach (var conv in convs)
            {
                Conversations.Add(conv);
            }
        }

        private async Task LoadMessages()
        {
            var messages = await _dbHelper.GetMessagesAsync(_currentConversation.Id);
            Messages.Clear();
            foreach (var msg in messages)
            {
                Messages.Add(msg);
            }
        }
    }
}
