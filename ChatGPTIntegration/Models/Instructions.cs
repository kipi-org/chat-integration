using System;
namespace ChatGPTIntegration.Models
{
	public class Instructions
	{
		public Instructions()
		{
		}

		public string Model { get; set; }

		public List<Message> Messages { get; set; }

		public float Temperature { get; set; }
	}
}

