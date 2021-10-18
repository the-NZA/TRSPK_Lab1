using System;

namespace DbLayer.Models
{
	public enum DealType
	{
		Purchase,
		Sale
	}

	public class Deal
	{
		public DealType Type { get; }
		public DateTime Date { get; }
		public int Amount { get; }
		public decimal Cost { get; }
		public string StockName { get; }

		public Deal(Stock stock, DealType type, DateTime date, int amount)
		{
			if (amount <= 0)
			{
				throw new Exception("Amount must be greater then zero");
			}


			Rate rate = stock.GetRateByDate(date);
			if (rate == null)
			{
				throw new Exception("Date doesn't set for these stock");
			}

			Cost = amount * rate.Price;

			StockName = stock.Name;
			Amount = amount;
			Type = type;
			Date = date;
		}

		public Deal(string fmt)
		{
			string[] parts = fmt.Split(':');
			if (parts.Length != 5)
			{
				throw new Exception("Deal must contains five fields");
			}

			foreach (string part in parts)
			{
				switch (part[0])
				{
					case 't':
						Type = Enum.Parse<DealType>(part.Substring(1));
						break;
					case 'd':
						Date = DateTime.Parse(part.Substring(1));
						break;
					case 'a':
						Amount = Convert.ToInt32(part.Substring(1));
						break;
					case 'c':
						Cost = Convert.ToDecimal(part.Substring(1));
						break;
					case 's':
						StockName = part.Substring(1);
						break;
					default:
						throw new Exception("Unknown field");
				}
			}
		}

		public string Marshal()
		{
			return $"s{StockName}:t{Type}:d{Date:MM/dd/yyyy}:a{Amount}:c{Cost}";
		}

		public override string ToString()
		{
			return
				$"StockName: {StockName}, Type: {Type}, Date: {Date:MM/dd/yyyy}, Amount: {Amount}, Cost: {Cost}";
		}
	}
}