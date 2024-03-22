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

        [HttpPost("send-message")]
        public async Task<string> SendMessage([FromBody]MessageRequestDto request)
        {
            var result = "";
            var allowedToSend = await _chatGPTService.AllowedToSendAsync(request.UserId);

            if (allowedToSend)
            {
                var instructions = await _chatGPTService.GetInstructionsAsync(request);
                result = await _chatGPTService.SendMessage(instructions);
            }
            else
            {
                return "Количество запросов в неделю превышено, попробуйте завтра";
            }

            return result;
        }

        [HttpGet("get-messages/{id}")]
        public async Task<List<Message>> GetMessages(int id)
        {
            var result = await _chatGPTService.GetMessages(id);

            return result;
        }
    }
}
