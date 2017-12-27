using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Telegram.Bot.Mashi.Models
{
    public class BotData: DbContext
    {
        public BotData(DbContextOptions<BotData> options) : base(options)
        {
        }


        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
    }
}
