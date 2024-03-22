using System;
namespace ChatGPTIntegration.Models
{
	public class Category
	{
		public Category()
		{
		}

		public string Name { get; set; }

		public int Id { get; set; }

        public override string ToString()
        {
			return $"Id:{Id}-Name:{Name}";
        }
    }
}

