using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Telegram.Bot.Mashi.Services
{
    public class GlobalService : IGlobalService
    {
        public string Command { get; set; }
        public IData Data { get; set; }
    }
}
