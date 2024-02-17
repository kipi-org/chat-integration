using System;
using OpenAI;
using OpenAI.Chat;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ChatGPTIntegration.Models.Response
{
    public class ChatGPTResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("choices")]
        public ChatGPTResponseChoice[] Choices { get; set; }

        [JsonPropertyName("usage")]
        public ChatGPTResponseUsage Usage { get; set; }

        [JsonPropertyName("system_fingerprint")]
        public string SystemFingerprint { get; set; }

        public ChatGPTResponse(string id, string objectValue, int created, string model, ChatGPTResponseChoice[] choices, ChatGPTResponseUsage usage, string systemFingerprint)
        {
            Id = id;
            Object = objectValue;
            Created = created;
            Model = model;
            Choices = choices;
            Usage = usage;
            SystemFingerprint = systemFingerprint;
        }

        // Добавлены свойства для обхода ограничения десериализации через конструктор
        [JsonIgnore]
        public int SomeOtherProperty { get; set; }

        // Пустой конструктор для десериализации JSON
        [JsonConstructor]
        public ChatGPTResponse()
        {
        }
    }
}