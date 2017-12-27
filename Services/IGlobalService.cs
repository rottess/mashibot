using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Telegram.Bot.Mashi.Services
{
    public interface IGlobalService
    {
        string Command { get; set; }
        IData Data { get; set; }
    }
}
