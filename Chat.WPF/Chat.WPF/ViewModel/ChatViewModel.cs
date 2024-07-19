using Microsoft.AspNetCore.SignalR.Client;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Chat.WPF.ViewModel
{
    public class ChatViewModel : BindableBase
    {
        public string? AccessToken { get; private set; }
        public string? UserName { get; private set; }

        public ChatViewModel(string? accessToken, string? userName)
        {
            AccessToken = accessToken;
            UserName = userName;
            InitializeAsync();

        }
        private async Task SendMessageAsync(string userName, string? message)
        {
            await _connection.SendAsync("SendMessageAsync", userName, message);
        }
        private async Task InitializeAsync()
        {
            ConnectCommand = new DelegateCommand(ConnectCommandExecute, ConnectCommandCanExecute);

            SendCommand = new DelegateCommand(SendCommandExecute, SendCommandCanExecute);
            if (_connection is null)
            {
                await ConnectToChatAsync();
            }

        }


        const string ChatUrl = "https://localhost:20001/chat";
        private HubConnection _connection = null!;

        public string ChatServerUrl
        {
            get => _chatServerUrl;
            set => SetProperty(ref _chatServerUrl, value);
        }
        private string _chatServerUrl = "https://localhost:20001";

        public ObservableCollection<string> UserList
        {
            get => _userList;
            set => SetProperty(ref _userList, value);
        }
        private ObservableCollection<string> _userList = new();

        private bool _isConnected = true;
        /// <summary>
        /// Represent MessageList property
        /// </summary>
        public ObservableCollection<string> MessageList
        {
            get => _messageList;
            set => SetProperty(ref _messageList, value);
        }

        /// <summary>
        /// Backing field for property MessageList
        /// </summary>
        private ObservableCollection<string> _messageList = new();


        public string? MessageText
        {
            get => _messageText;
            set
            {
                SetProperty(ref _messageText, value);
                SendCommand!.RaiseCanExecuteChanged();
            }
        }
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                SetProperty(ref _isAuthenticated, value);
                ConnectCommand.RaiseCanExecuteChanged();
            }
        }
        private bool _isAuthenticated = true;

        private string? _messageText;

        public DelegateCommand ConnectCommand { get; private set; } = null!;

        private bool ConnectCommandCanExecute()
        {
            return IsAuthenticated && !_isConnected;
        }

        /// <summary>
        /// Execute method for ConnectCommand
        /// </summary>
        private async void ConnectCommandExecute()
        {
            await ConnectToChatAsync();
        }
        public DelegateCommand? SendCommand { get; private set; }


        private bool SendCommandCanExecute()
        {
            return _isConnected
                           && !string.IsNullOrWhiteSpace(AccessToken)
                           && !string.IsNullOrWhiteSpace(UserName)
                   && !string.IsNullOrWhiteSpace(MessageText);
        }

        private async void SendCommandExecute()
        {
            await SendMessageAsync(UserName, MessageText);
            MessageText = string.Empty;
        }
        private async Task ConnectToChatAsync()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(ChatUrl, options =>
                {
                    // custom Token Provider if you needed
                    //options.AccessTokenProvider =

                    options.Headers.Add("Authorization", $"Bearer {AccessToken}");
                })
                .WithAutomaticReconnect()
                .Build();


            _connection.On<IEnumerable<string>>("UpdateUsersAsync", users =>
            {
                UserList = new ObservableCollection<string>(users);
            });

            _connection.On<string, string>("SendMessageAsync", (user, message) =>
            {
                var item = $"{user} | {message}";
                App.Current.Dispatcher.Invoke((System.Action)delegate //without dispatcher -> Этот тип CollectionView не поддерживает изменения в SourceCollection из потока, отличного от потока Dispatcher
                {
                    MessageList.Add(item);
                });
            });

            try
            {
                await _connection.StartAsync();
                _isConnected = true;
            }
            catch (Exception exception)
            {
                MessageList.Add(exception.Message);
            }
        }

    }
}
