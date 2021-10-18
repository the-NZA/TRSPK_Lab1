using System;
using System.Collections.Generic;
using System.Text;

namespace DbLayer.Models
{
	public class Stock
	{
		public string Name { get; }
		private readonly List<Rate> _rates = new List<Rate>();

		public Stock(string name, List<Rate> rates)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				throw new Exception("Stock name is required field");
			}

			// If rates were passed
			if (rates.Count > 0)
			{
				_rates = rates;
			}

			// Save all names in uppercase
			Name = name.ToUpper();
		}

		// Static method for converting formated string to Stock object
		public static Stock ParseFormat(string fmt)
		{
			string parsedName = "";
			List<Rate> parsedRates = new List<Rate>();

			foreach (string part in fmt.Split(";"))
			{
				switch (part[0])
				{
					case 'n':
						string name = part.Substring(1);
						if (String.IsNullOrWhiteSpace(name))
						{
							throw new Exception("Stock name is required field");
						}

						parsedName = name;
						break;
					case 'r':
						string[] ratesString = part.Substring(1).Split(',');
						if (ratesString.Length > 0)
						{
							foreach (string str in ratesString)
							{
								parsedRates.Add(new Rate(str));
							}
						}

						break;
					default:
						throw new Exception("Unknown field");
				}
			}

			if (String.IsNullOrWhiteSpace(parsedName))
			{
				throw new Exception("Stock name is required field");
			}

			return new Stock(parsedName, parsedRates);
		}

		public void AddRate(Rate newRate)
		{
			if (newRate == null)
			{
				throw new Exception("Rate can't be null");
			}

			// Find item by date
			if (_rates.FindIndex(r => r.Date == newRate.Date) != -1)
			{
				throw new Exception("Rate with the specified date already exists");
			}

			// Save new rate
			_rates.Add(newRate);

			// Sort rates by date
			_rates.Sort((r1, r2) => DateTime.Compare(r1.Date, r2.Date));
		}

		public Rate GetRateByDate(DateTime date)
		{
			return _rates.Find(r => r.Date == date);
		}

		public string Marshal()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append($"n{Name};r");

			for (int i = 0; i < _rates.Count; i++)
			{
				sb.Append(string.Format((i + 1) == _rates.Count ? "{0}" : "{0},", _rates[i].Marshal()));
			}

			sb.Append('\n');

			return sb.ToString();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append($"Name:{Name}; Rates:[");

			for (int i = 0; i < _rates.Count; i++)
			{
				sb.Append(string.Format((i + 1) == _rates.Count ? "({0})" : "({0}),", _rates[i]));
			}

			sb.Append(']');

			return sb.ToString();
		}
	}
}