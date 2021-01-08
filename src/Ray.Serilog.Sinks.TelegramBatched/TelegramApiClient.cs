﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ray.Infrastructure.Extensions;
using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.TelegramBatched
{
    public class TelegramApiClient : IPushService
    {
        private readonly string _chatId;
        private const string TelegramBotApiUrl = "https://api.telegram.org/bot";

        /// <summary>
        /// The API URL.
        /// </summary>
        private readonly Uri _apiUrl;

        /// <summary>
        /// The HTTP client.
        /// </summary>
        private readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramClient"/> class.
        /// </summary>
        /// <param name="botToken">The Telegram bot token.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <exception cref="ArgumentException">Thrown if the bot token is null or empty.</exception>
        public TelegramApiClient(string botToken, string chatId, int timeoutSeconds = 10)
        {
            if (string.IsNullOrWhiteSpace(botToken))
            {
                throw new ArgumentException("The bot token mustn't be empty.", nameof(botToken));
            }

            _chatId = chatId;

            this._apiUrl = new Uri($"{TelegramBotApiUrl}{botToken}/sendMessage");
            this._httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        }

        public async Task<HttpResponseMessage> PushMessageAsync(string message)
        {
            var json = new { chat_id = _chatId, text = message, parse_mode = "HTML" }.ToJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this._httpClient.PostAsync(this._apiUrl, content);
            return response;
        }
    }
}
