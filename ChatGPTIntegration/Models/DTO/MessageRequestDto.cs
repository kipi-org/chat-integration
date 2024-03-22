using System;
namespace ChatGPTIntegration.Models
{
	public class MessageRequestDto
	{
		public MessageRequestDto()
		{
		}

		public int UserId { get; set; }

		public string Message { get; set; }

        public List<Transaction> Transactions { get; set; }

		public List<Category> Categories { get; set; }
	}
}