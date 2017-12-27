using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Mashi.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Mashi.Services
{
    public class Quiz : IData
    {
        internal IList<int> SkippedRows { get; set; }
        internal int Score { get; set; }
        internal int QuestionCount { get; set; }

        internal Question CurrentQuestion { get; set; }

        public async Task Done(long id, string msg, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(id, msg);
        }

        public async Task Init(CallbackQuery query, BotData bData, TelegramBotClient client)
        {
            QuestionCount = bData.Questions.Count();
            SkippedRows = new List<int>();
            await LoadRandomQuestion(query, bData, client);
        }

        public async Task<bool> Next(CallbackQuery query, BotData bData, TelegramBotClient client)
        {
            var answer = CurrentQuestion.Answers.FirstOrDefault(x => x.Id.ToString() == query.Data);
            if (answer != null)
            {
                if (answer.IsCorrect)
                {
                    Score++;
                    await client.SendTextMessageAsync(query.Message.Chat.Id, string.Format("Bạn {0} thật thông minh, trả lời đúng rồi. Điểm số của bạn là: {1}", query.From.Username, (Score * 100).ToString()));
                }
                else
                {
                    if (Score > 0)
                        Score--;
                    await client.SendTextMessageAsync(query.Message.Chat.Id, string.Format("Câu trả lời của bạn {0} đã được ghi nhận, bạn rất tốt nhưng chúng tôi rất tiếc. Điểm của bạn hiện giờ: {1}", query.From.Username, (Score * 100).ToString()));
                    return false;
                }

                QuestionCount--;
                if (QuestionCount == 0)
                {
                    await Done(query.Message.Chat.Id, "Bạn đã hoành thành tất cả câu hỏi. Chúc mừng bạn.", client);
                    return false;
                }
                await LoadRandomQuestion(query, bData, client);
            }
            return true;
        }

        private async Task LoadRandomQuestion(CallbackQuery query, BotData bData, TelegramBotClient client)
        {
            var totalQuestions = bData.Questions.Count();
            var random = new Random();
            var skipRow = random.Next(0, totalQuestions - 1);
            while (SkippedRows.Contains(skipRow))
            {
                skipRow = random.Next(0, totalQuestions - 1);
            }
            SkippedRows.Add(skipRow);
            CurrentQuestion = bData.Questions.Include(x => x.Answers).Skip(skipRow).FirstOrDefault();
            var btns = new List<InlineKeyboardCallbackButton>();
            foreach (var answer in CurrentQuestion.Answers)
            {
                var inlineK = new InlineKeyboardCallbackButton(answer.Text, answer.Id.ToString());
                btns.Add(inlineK);
            }

            InlineKeyboardButton[][] btnArray = new InlineKeyboardButton[btns.Count][];
            for (int i = 0; i < btns.Count; i++)
            {
                btnArray[i] = new InlineKeyboardButton[1];
                btnArray[i][0] = btns[i];
            }
            var keyboard = new InlineKeyboardMarkup(btnArray);
            await client.SendTextMessageAsync(query.Message.Chat.Id, CurrentQuestion.Text, ParseMode.Default, false, false, 0, keyboard);
        }
    }
}
