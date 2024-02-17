using System.Net.Http;
using System.Threading.Tasks;
using ChatGPTIntegration.Models;
using ChatGPTIntegration.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatGptService.Controllers
{
    [Route("api/[controller]")]
    [TypeFilter(typeof(ApiKeyAuthorizationFilter))]
    public class ChatController : ControllerBase
    {
        private readonly ChatGPTService _chatGPTService;

        public ChatController(ChatGPTService chatGPTService)
        {
            _chatGPTService = chatGPTService;
        }

        [HttpPost("SendMessage")]
        public async Task<string> Post([FromBody]MessageRequestDto request)
        {
            var instructions = await _chatGPTService.GetInstructionsAsync(request);

            var result = await _chatGPTService.SendMessage(instructions);

            return result;
        }
    }
}
