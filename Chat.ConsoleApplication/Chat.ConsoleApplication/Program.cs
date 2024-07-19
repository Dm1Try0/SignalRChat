using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.ConsoleApplication
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const string serverUrl = "https://localhost:10001/connect/token";
            const string chatUrl = "https://localhost:20001/chat";

            Console.WriteLine("Start app");
            Console.WriteLine("Enter Username: ");
            string Username = Console.ReadLine();
            Console.WriteLine("Enter Password: ");
            string Password = Console.ReadLine();

            Console.WriteLine("Getting token...");
            var token = await TokenLoader.RequestToken(Username, Password, serverUrl);

            if (string.IsNullOrWhiteSpace(token?.AccessToken))
            {
                Console.WriteLine("Request token error. Exit.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Creating connection...");

            var connection = new HubConnectionBuilder()
                .WithUrl(chatUrl, options =>
                {
                    // custom Token Provider if you needed
                    // options.AccessTokenProvider =

                    options.Headers.Add("Authorization", $"Bearer {token.AccessToken}");
                })
                .WithAutomaticReconnect()
                .Build();

            Console.WriteLine("Subscribe to actions...");

            #region subscriptions
            connection.On<IEnumerable<string>>("UpdateUsersAsync", users =>
            {
                Console.WriteLine("--------------------------------");
                var enumerable = users as string[] ?? users.ToArray();
                Console.WriteLine($"Total users: {enumerable.Length}");
                foreach (var user in enumerable)
                {
                    Console.WriteLine($"{user}");
                }
            });

            connection.On<string, string>("SendMessageAsync", (user, message) =>
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"{user} | {message}");
            });
            #endregion
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Connected to chat");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter your message");
            try
            {
                connection.StartAsync().GetAwaiter().GetResult();

                while (true)
                {
                    string message = Console.ReadLine();
                    
                    await connection.SendAsync("SendMessageAsync", Username,message);
                    
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
