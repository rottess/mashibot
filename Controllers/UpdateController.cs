using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Mashi.Services;
using System.Threading.Tasks;

namespace Telegram.Bot.Mashi.Controllers
{
    [Route("api/[controller]/[action]")]
	public class UpdateController : Controller
	{
        readonly IUpdateService _updateService;
        readonly BotConfiguration _config;

        public UpdateController(IUpdateService updateService, BotConfiguration config)
		{
			_updateService = updateService;
            _config = config;
		}

		// POST api/update
		[HttpPost]
		public async Task Post([FromBody]Update update)
		{
			await _updateService.Echo(update);
		}

        [HttpGet]
        public string Test()
        {
            return "OK";
        }
    }
}
