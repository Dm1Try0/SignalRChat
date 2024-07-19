using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chat.ConsoleApplication
{
    public static class TokenLoader
    {
        /// <summary>
        /// Get token(request)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="tokenServerUrl"></param>
        /// <returns></returns>
        public static Task<SecurityToken> RequestToken(string userName, string password, string tokenServerUrl)
        {
            var content = GetContent(userName, password);
            return GetTokenAsync(content, tokenServerUrl);
        }
        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static FormUrlEncodedContent GetContent(string userName, string password)
        {
            var values = new List<KeyValuePair<string, string>>{
                new("grant_type","password"),
                new("username", userName),
                new("password", password),
                new("client_secret", "client-secret-sts"), // sts/code
                new("client_id", "client-id-sts"), // sts/code
                new("scope", "custom")
            };

            return new FormUrlEncodedContent(values);
        }
        /// <summary>
        /// Get your Token
        /// </summary>
        /// <param name="content"></param>
        /// <param name="tokenServerUrl"></param>
        /// <returns></returns>
        private static async Task<SecurityToken> GetTokenAsync(FormUrlEncodedContent content, string tokenServerUrl)
        {
            string responseResult;
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync($"{tokenServerUrl}", content);
                if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseText))
                    {
                        Console.WriteLine(responseText);
                        return null;
                    }
                }

                response.EnsureSuccessStatusCode();
                responseResult = await response.Content.ReadAsStringAsync();
            }
            try
            {
                if (!string.IsNullOrEmpty(responseResult))
                {
                    return JsonSerializer.Deserialize<SecurityToken>(responseResult);
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return null;
        }
    }
}
