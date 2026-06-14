using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AIChatBotAppRAG.Data;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AIChatBotAppRAG.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly DatabaseHelper _dbHelper;

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public LoginViewModel(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter username and password";
                return;
            }

            ErrorMessage = string.Empty;
            await Shell.Current.GoToAsync("//ChatPage");
        }
    }
}
