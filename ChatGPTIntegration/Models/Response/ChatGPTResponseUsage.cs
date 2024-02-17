using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenAI;

namespace ChatGPTIntegration.Models.Response
{
	public class ChatGPTResponseUsage
	{
		public ChatGPTResponseUsage(
			int prompt_tokens,
			int completion_tokens,
			int total_tokens
			)
		{
			PromptTokens = prompt_tokens;
			CompletionTokens = completion_tokens;
			TotalTokens = total_tokens;
		}

        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }

        // Пустой конструктор для десериализации JSON
        [JsonConstructor]
        public ChatGPTResponseUsage()
        {
        }

        public static ChatGPTResponseUsage FromJson(string json)
        {
            return JsonSerializer.Deserialize<ChatGPTResponseUsage>(json);
        }
    }
}

