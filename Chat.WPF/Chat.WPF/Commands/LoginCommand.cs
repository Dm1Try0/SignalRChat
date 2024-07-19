using Chat.WPF.TokenHelper;
using Chat.WPF.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Chat.WPF.Commands
{
    public class LoginCommand : ICommand
    {
        private readonly LoginViewModel _viewModel;
        private string _authServerUrl = "https://localhost:10001/connect/token";
        public LoginCommand(LoginViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.Username) &&
                !string.IsNullOrEmpty(_viewModel.Password);
        }
        /// <summary>
        /// Login method
        /// </summary>
        /// <param name="parameter"></param>
        public async void Execute(object parameter)
        {
            if (_viewModel.Username.Length < 4 && _viewModel.Password.Length < 8)
            {
                MessageBox.Show("login must be more than 4 characters long and password more 8");
                return;
            }
            var token = await GetToken(_viewModel.Username, _viewModel.Password, _authServerUrl);
            if (token == null) { MessageBox.Show("Incorrect username or password"); return; }
            ChatWindow chatWindow = new ChatWindow()
            {
                DataContext = new ChatViewModel(token.AccessToken, _viewModel.Username)
            };
            chatWindow.Show();
            var window = Application.Current.Windows[0];
            if (window != null) { window.Close(); }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// get acceess token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="serverUrl"></param>
        /// <returns></returns>
        private async Task<SecurityToken?> GetToken(string userName, string password, string serverUrl)
        {
            var token = await TokenLoader.RequestToken(userName, password, serverUrl);

            if (!string.IsNullOrWhiteSpace(token?.AccessToken))
            {
                return token;
            }
            MessageBox.Show("Error username or password");
            return null;

        }
    }
}
