using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Mashi.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Mashi.Services
{
    public class UpdateService : IUpdateService
    {
        readonly IBotService _botService;
        readonly IGlobalService _gService;
        readonly BotData _bData;
        private const string _botName = "/mashi";
        public UpdateService(IBotService botService, IGlobalService gService, BotData bData)
        {
            _botService = botService;
            _gService = gService;
            _bData = bData;
        }

        public async Task Echo(Update update)
        {
            var message = update.Message;

            if (update.Type == UpdateType.MessageUpdate)
            {
                Console.WriteLine("Received Message from {0}", message.Chat.Id);
                if (message.Type == MessageType.TextMessage)
                {
                    if (message.Text.ToLower().StartsWith(_botName))
                    {
                        if (message.Text.Trim().Equals(_botName, StringComparison.OrdinalIgnoreCase))
                        {
                            var keyboard = new InlineKeyboardMarkup(new[]
                            {
                                new[] // first row
                                {
                                    new InlineKeyboardCallbackButton("Đố vui", "Quiz")
                                }
                            });
                            await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Mashi chào các bạn. Hiện nay mashi bot hỗ trợ các tính năng sau:", ParseMode.Default, false, false, 0, keyboard);
                        }
                        else
                        {
                            var command = GetCommand(message.Text);
                            switch (command)
                            {
                                //case "đố vui":
                                //    await StartQuiz(message);
                                //    break;
                                case "thoát":
                                    Done(message.Chat.Id);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            else if (update.Type == UpdateType.CallbackQueryUpdate)
            {
                var query = update.CallbackQuery;
                switch (query.Data)
                {
                    case "Quiz":
                        await StartQuiz(query);
                        break;
                    case "Quit":
                        _gService.Command = string.Empty;
                        _gService.Data = null;
                        await _botService.Client.SendTextMessageAsync(query.Message.Chat.Id, "Cám ơn bạn đã sử dụng @mashi");
                        break;
                    default:
                        await CheckCommand(query);
                        break;
                }
            }
        }

        private async void Done(long id)
        {
            if (_gService.Command == "Quiz")
            {
                var keyboard = new InlineKeyboardMarkup(new[]
                {
                    new[] // first row
                    {
                        new InlineKeyboardCallbackButton("Đồng ý", "Quit")
                    }
                });
                await _botService.Client.SendTextMessageAsync(id, "Bạn có đồng ý thoát trò chơi ?", ParseMode.Default, false, false, 0, keyboard);
            }
        }

        private async Task StartQuiz(CallbackQuery query)
        {
            _gService.Command = "Quiz";
            _gService.Data = new Quiz();
            await _gService.Data.Init(query, _bData, _botService.Client);

        }
        private async Task CheckCommand(CallbackQuery query)
        {
            if (!string.IsNullOrEmpty(_gService.Command))
            {
                var result = await _gService.Data.Next(query, _bData, _botService.Client);
                if (!result)
                {
                    _gService.Command = string.Empty;
                    _gService.Data = null;
                }
            }
        }

        public string GetCommand(string text)
        {
            return text.Replace(_botName, string.Empty).Trim().ToLower();
        }
    }
}
