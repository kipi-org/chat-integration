using System;
namespace ChatGPTIntegration.Models
{
	public class Category
	{
		public Category()
		{
		}

		public string Name { get; set; }

        public override string ToString()
        {
			return Name;
        }
    }
}

