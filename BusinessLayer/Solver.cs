using System;
using System.Collections.Generic;
using DbLayer;
using DbLayer.Models;


namespace BusinessLayer
{
	public struct RateWithPercent
	{
		public decimal Rate { get; }
		public decimal Percent { get; }

		public RateWithPercent(decimal rate, decimal percent)
		{
			if (rate < 0)
			{
				throw new Exception("Rate must be greater than zero");
			}

			Rate = rate;
			Percent = percent;
		}

		public override string ToString()
		{
			return $"Rate: {Rate}, Percent: {Percent}";
		}
	}

	public class Solver
	{
		private readonly Db _db;

		public Solver(Db db)
		{
			if (db == null)
			{
				throw new Exception("Db can't be null");
			}

			_db = db;
		}

		// Portfolio CRUD operations
		public List<Portfolio> GetAllPortfolios()
		{
			return _db.PortfolioRepository.GetAll();
		}

		public Portfolio GetPortfolio(string ownerName)
		{
			if (String.IsNullOrWhiteSpace(ownerName))
			{
				throw new Exception("Owner name can't be null or empty");
			}

			return _db.PortfolioRepository.Get(ownerName);
		}
		
		public bool InsertPortfolio(Portfolio portfolio)
		{
			if (portfolio == null)
			{
				throw new Exception("Portfolio can't be null");
			}

			return _db.PortfolioRepository.Insert(portfolio);
		}
		
		public void DeletePortfolio(string ownerName)
		{
			_db.PortfolioRepository.Delete(ownerName);
		}

		public void UpdatePortfolio(Portfolio updatedPortfolio)
		{
			if (updatedPortfolio == null)
			{
				throw new Exception("Updated portfolio can't be null");
			}

			_db.PortfolioRepository.Update(updatedPortfolio);
		}
		
		// Stock CRUD operations
		public List<Stock> GetAllStocks()
		{
			return _db.StockRepository.GetAll();
		}

		public Stock GetStock(string stockName)
		{
			if (String.IsNullOrWhiteSpace(stockName))
			{
				throw new Exception("Stock name can't be null or empty");
			}

			return _db.StockRepository.Get(stockName);
		}

		public bool InsertStock(Stock stock)
		{
			if (stock == null)
			{
				throw new Exception("Stock can't be null");
			}

			return _db.StockRepository.Insert(stock);
		}

		public void DeleteStock(string stockName)
		{
			_db.StockRepository.Delete(stockName);
		}

		public void UpdateStock(Stock updatedStock)
		{
			if (updatedStock == null)
			{
				throw new Exception("Updated Stock can't be null");
			}

			_db.StockRepository.Update(updatedStock);
		}


		public decimal SolveOne(string ownerName, DateTime date)
		{
			Portfolio one = _db.PortfolioRepository.Get(ownerName);
			decimal sum = 0;

			foreach (var item in one.CountStock(date))
			{
				Stock s = _db.StockRepository.Get(item.Key);
				sum += s.GetRateByDate(date).Price * item.Value;
			}

			return sum;
		}

		public Dictionary<string, RateWithPercent> SolveTwo(List<string> stockNames)
		{
			if (stockNames == null)
			{
				throw new Exception("stockNames list can't be null");
			}

			Dictionary<string, RateWithPercent> result = new Dictionary<string, RateWithPercent>();
			DateTime today = DateTime.Today;
			DateTime prevday = today.AddDays(-1);

			foreach (string name in stockNames)
			{
				Stock stock = _db.StockRepository.Get(name);

				// If stock with given name doesn't exist than just continue
				if (stock == null)
				{
					continue;
				}

				// Avoiding duplicates in result dict
				if (!result.ContainsKey(name))
				{
					Rate todayRate = stock.GetRateByDate(today);
					Rate prevdayRate = stock.GetRateByDate(prevday);
					decimal percent;

					// Calculate percents
					if (todayRate.Price > prevdayRate.Price)
					{
						percent = prevdayRate.Price * 100 / todayRate.Price;
						percent = 100 - percent;
					}
					else
					{
						percent = todayRate.Price * 100 / prevdayRate.Price;
						percent -= 100;
					}

					result[name] = new RateWithPercent(todayRate.Price, percent);
				}
			}


			return result;
		}

		public (decimal pTurnover, decimal sTurnover) SolveThree(string ownerName, DateTime from, DateTime to)
		{
			if (from > to)
			{
				throw new Exception("From date must be less or equal than to date");
			}

			var port = _db.PortfolioRepository.Get(ownerName);
			var deals = port.GetDealByDate(from, to);
			decimal purchaseTurnover = 0;
			decimal saleTurnover = 0;

			foreach (var deal in deals)
			{
				if (deal.Type == DealType.Purchase)
				{
					purchaseTurnover += deal.Cost;
				}
				else
				{
					saleTurnover += deal.Cost;
				}
			}

			return (purchaseTurnover, saleTurnover);
		}
	}
}