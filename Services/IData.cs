using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Mashi.Models;
using Telegram.Bot.Types;

namespace Telegram.Bot.Mashi.Services
{
    public interface IData
    {
        Task Init(CallbackQuery msg, BotData bData, TelegramBotClient client);
        Task<bool> Next(CallbackQuery msg, BotData bData, TelegramBotClient client);

        Task Done(long id, string msg, TelegramBotClient client);
    }
}
