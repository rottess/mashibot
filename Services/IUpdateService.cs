using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Mashi.Services
{
    public interface IUpdateService
    {
        Task Echo(Update update);
    }
}
