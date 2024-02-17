using System;
using System.Runtime.InteropServices;

namespace ChatGPTIntegration.Models
{
	public class Transaction
	{
		public Transaction()
		{
		}

        public Decimal Amount { get; set; }

		public DateTime Date { get; set; }

		public Category Category { get; set; }

		public string? Desc { get; set; }

        public override string ToString()
        {
            return $"{Amount}-{Date}-{Category.ToString()}-{Desc}";
        }
    }
}