using System;
using System.Collections.Generic;
using System.Text;

namespace DbLayer.Models
{
	public class Portfolio
	{
		public string Owner { get; } // Portfolio owner's name
		private List<Deal> _deals = new List<Deal>(); // List of deals

		public Portfolio(string owner, List<Deal> deals)
		{
			if (String.IsNullOrWhiteSpace(owner))
			{
				throw new Exception("Owner name is required");
			}

			Owner = owner;

			if (deals != null)
			{
				_deals = deals;
			}
		}

		public static Portfolio ParseFormat(string fmt)
		{
			string parsedOwner = "";
			List<Deal> parsedDeals = new List<Deal>();

			foreach (string part in fmt.Split(";"))
			{
				switch (part[0])
				{
					case 'o':
						string name = part.Substring(1);
						if (String.IsNullOrWhiteSpace(name))
						{
							throw new Exception("Owner's name is required field");
						}

						parsedOwner = name;
						break;
					case 'd':
						string[] ratesString = part.Substring(1).Split(',');
						if (ratesString.Length > 0)
						{
							foreach (string str in ratesString)
							{
								parsedDeals.Add(new Deal(str));
							}
						}

						break;
					default:
						throw new Exception("Unknown field");
				}
			}

			if (String.IsNullOrWhiteSpace(parsedOwner))
			{
				throw new Exception("Stock name is required field");
			}

			return new Portfolio(parsedOwner, parsedDeals);
		}

		public void AddDeal(Deal newDeal)
		{
			if (newDeal == null)
			{
				throw new Exception("Deal can't be null");
			}

			// Save new deal
			_deals.Add(newDeal);

			// Sort deals by date
			_deals.Sort((d1, d2) => DateTime.Compare(d1.Date, d2.Date));
		}

		// Return dict with stocks' count 
		public Dictionary<string, int> CountStock()
		{
			Dictionary<string, int> stocks = new Dictionary<string, int>();

			foreach (Deal deal in _deals)
			{
				if (stocks.ContainsKey(deal.StockName))
				{
					stocks[deal.StockName] += (deal.Type == DealType.Purchase
						? deal.Amount
						: -1 * deal.Amount);
				}
				else
				{
					stocks[deal.StockName] = deal.Type == DealType.Purchase
						? deal.Amount
						: -1 * deal.Amount;
				}
			}


			return stocks;
		}

		// Return dict with stocks' count which Deal.Date <= date
		public Dictionary<string, int> CountStock(DateTime date)
		{
			Dictionary<string, int> stocks = new Dictionary<string, int>();

			foreach (Deal deal in _deals)
			{
				if (deal.Date > date)
				{
					continue;
				}

				if (stocks.ContainsKey(deal.StockName))
				{
					stocks[deal.StockName] += (deal.Type == DealType.Purchase
						? deal.Amount
						: -1 * deal.Amount);
				}
				else
				{
					stocks[deal.StockName] = deal.Type == DealType.Purchase
						? deal.Amount
						: -1 * deal.Amount;
				}
			}

			return stocks;
		}

		public List<Deal> GetDealByDate(DateTime from, DateTime to)
		{
			List<Deal> deals = new List<Deal>();
			foreach (Deal deal in _deals)
			{
				if (deal.Date >= from && deal.Date <= to)
				{
					deals.Add(deal);
				}
			}

			return deals;
		}

		public string Marshal()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append($"o{Owner};d");

			for (int i = 0; i < _deals.Count; i++)
			{
				sb.Append(string.Format((i + 1) == _deals.Count ? "{0}" : "{0},", _deals[i].Marshal()));
			}

			sb.Append('\n');

			return sb.ToString();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append($"Owner: {Owner}, Deals:[");

			for (int i = 0; i < _deals.Count; i++)
			{
				sb.Append(string.Format((i + 1) == _deals.Count ? "({0})" : "({0}),",
					_deals[i].Marshal()));
			}

			sb.Append(']');

			return sb.ToString();
		}
	}
}