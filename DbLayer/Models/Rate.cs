using System;

namespace DbLayer.Models
{
	public class Rate
	{
		public DateTime Date { get; }
		public decimal Price { get; }

		public Rate(DateTime date, decimal price)
		{
			Date = date;
			Price = price;
		}

		public Rate(string fmt)
		{
			string[] parts = fmt.Split(':');
			if (parts.Length != 2)
			{
				throw new Exception("Rate must contains two fields");
			}

			foreach (string part in parts)
			{
				switch (part[0])
				{
					case 'd':
						Date = DateTime.Parse(part.Substring(1));
						break;
					case 'p':
						Price = Convert.ToDecimal(part.Substring(1));
						break;
					default:
						throw new Exception("Unknown field");
				}
			}
		}

		public string Marshal()
		{
			return $"d{Date:MM/dd/yyyy}:p{Price}";
		}

		public override string ToString()
		{
			return $"Date: {Date:MM/dd/yyyy}: Price: {Price}";
		}
	}
}