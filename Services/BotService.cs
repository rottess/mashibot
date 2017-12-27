﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Telegram.Bot.Mashi.Services
{
    public class BotService : IBotService
    {
        readonly BotConfiguration _config;

        public BotService(BotConfiguration config)
        {
            _config = config;
            Client = new TelegramBotClient(_config.BotToken);
        }

        public TelegramBotClient Client { get; }

        public List<int> MailReceiver => new List<int>() { 69818703 };
    }
}