using System.Collections.Generic;

namespace Telegram.Bot.Mashi.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
        List<int> MailReceiver { get; }
    }
}