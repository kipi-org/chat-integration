using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenAI.Chat;

namespace ChatGPTIntegration.Models.Response
{
	public class ChatGPTResponseChoice
	{
		public ChatGPTResponseChoice(
            int index,
            Message message,
            object logprobs,
            string finish_reason
            )
		{
            Index = index;
            Message = message;
            Logprobs = logprobs;
            FinishReason = finish_reason;
		}

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; }

        [JsonPropertyName("logprobs")]
        public object Logprobs { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }

        // Пустой конструктор для десериализации JSON
        [JsonConstructor]
        public ChatGPTResponseChoice()
        {
        }

        public static ChatGPTResponseChoice FromJson(string json)
        {
            return JsonSerializer.Deserialize<ChatGPTResponseChoice>(json);
        }
    }
}

