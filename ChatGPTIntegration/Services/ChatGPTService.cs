using System.Text;
using System.Text.Json;
using ChatGPTIntegration.Models;
using Microsoft.EntityFrameworkCore;
using ChatGPTIntegration.Models.Response;

namespace ChatGPTIntegration.Services
{
    public class ChatGPTService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MessagesContext _messagesContext;

        public ChatGPTService(IConfiguration configuration, IHttpClientFactory httpClientFactory, MessagesContext messagesContext)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _messagesContext = messagesContext;
        }

        public async Task<List<Message>> GetMessages(int userId)
        {
            var result = await _messagesContext.Messages.Where(x => x.UserId == userId).ToListAsync();

            return result;
        }

        public async Task<string?> SendMessage(Instructions instructions)
        {
            string apiKey = _configuration.GetSection("key").Value.ToString();
            string endpoint = _configuration.GetSection("endpoint").Value.ToString();
            int userId = instructions.Messages.LastOrDefault().UserId;

            using (var client = _httpClientFactory.CreateClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestBody = new
                {
                    model = instructions.Model,
                    messages = instructions.Messages
                        .Select(x => new Dictionary<string, string>
                        {
                            { "role", x.Role },
                            { "content", x.Content }
                        })
                        .ToList(),
                    temperature = instructions.Temperature,
                    max_tokens = Int32.Parse(_configuration.GetSection("max_tokens").Value.ToString())
                };


                var jsonRequest = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endpoint, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ChatGPTResponse>(await response.Content.ReadAsStringAsync());

                    await AddMessageAsync(userId, _configuration.GetSection("role_user").Value.ToString(), instructions.Messages.LastOrDefault().Content);
                    await AddMessageAsync(userId, _configuration.GetSection("role_assistant").Value.ToString(), result.Choices.FirstOrDefault().Message.Content);

                    return result.Choices.FirstOrDefault().Message.Content;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return ($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
        }

        public async Task AddMessageAsync(int userId, string role, string content)
        {
            Message newMessage = new Message
            {
                UserId = userId,
                Role = role,
                Content = content,
                Date = DateTime.UtcNow
            };

            _messagesContext.Messages.Add(newMessage);
            await _messagesContext.SaveChangesAsync();
        }

        public async Task<Instructions> GetInstructionsAsync(MessageRequestDto request)
        {
            var result = new Instructions()
            {
                Model = _configuration.GetSection("model").Value.ToString(),
                Temperature = float.Parse(_configuration.GetSection("temperature").Value.ToString()),
            };

            var transactions = request.Transactions.Select(x => x.ToString());

            List<Message> messages = await _messagesContext.Messages
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Date) // Сортировка по убыванию даты
                .Take(6)
                .Select(x => new Message
                {
                    Role = x.Role,
                    Content = x.Content
                })
                .ToListAsync();

            messages.Add(new Message
            {
                UserId = request.UserId,
                Role = _configuration.GetSection("role_user").Value.ToString(),
                Content = "Мои транзакции: " + string.Join(",", transactions)
            });

            // разворачиваем список сообщений в связи с ТЗ
            messages.Reverse();

            // Задаем prompt для работы модели
            messages.Add(new Message
            {
                UserId = request.UserId,
                Role = _configuration.GetSection("role_system").Value.ToString(),
                Content = _configuration.GetSection("prompt").Value.ToString()
            });

            // Задаем последнее сообщение пользователя
            messages.Add(new Message
            {
                UserId = request.UserId,
                Role = _configuration.GetSection("role_user").Value.ToString(),
                Content = request.Message
            });

            result.Messages = messages;

            return result;
        }

        public async Task<bool> AllowedToSendAsync(int userId)
        {
            var result = true;
            var whiteListJson = _configuration.GetSection("whitelist").Value.ToString();
            List<int> whitelist = GetNumbers(whiteListJson).Select(c => int.Parse(c)).ToList();

            if (whitelist.Contains(userId))
            {
                return result;
            }

            var time = DateTime.UtcNow.AddDays(-7);
            var limitCount = Int32.Parse(_configuration.GetSection("limit_count").Value.ToString());

            var lastWeekMessages = await _messagesContext.Messages
                                                    .Where(x => x.UserId == userId &&
                                                                x.Date >= time)
                                                    .ToListAsync();

            if (lastWeekMessages.Count > limitCount)
            {
                result = false;
            }

            return result;
        }

        private List<string> GetNumbers(string input)
        {
            List<char> temp = input.Where(c => char.IsDigit(c)).ToList();
            var toReturn = temp.Select(c => c.ToString()).ToList();
            return toReturn;
        }
    }
}
