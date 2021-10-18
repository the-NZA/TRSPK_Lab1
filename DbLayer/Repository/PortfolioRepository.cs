using System;
using System.Collections.Generic;
using DbLayer.Models;
using System.IO;
using System.Linq;


namespace DbLayer.Repository
{
	public class PortfolioRepository : IPortfolioRepository
	{
		private readonly string _portfolioDbPath;

		public PortfolioRepository(string portfolioDbPath)
		{
			if (String.IsNullOrWhiteSpace(portfolioDbPath))
			{
				throw new Exception("Portfolio DB path is required");
			}

			_portfolioDbPath = portfolioDbPath;
		}

		public bool Insert(Portfolio portfolio)
		{
			if (Get(portfolio.Owner) != null)
			{
				throw new Exception("Item already exist");
			}

			using (StreamWriter portfolioWriter = new StreamWriter(_portfolioDbPath, true))
			{
				portfolioWriter.Write(portfolio.Marshal());
			}

			return true;
		}

		public Portfolio Get(string owner)
		{
			// Get portfolio by owner name
			using (StreamReader portfolioReader = new StreamReader(_portfolioDbPath))
			{
				// Lookup portfolio by owner's name
				string line;
				while ((line = portfolioReader.ReadLine()) != null)
				{
					Portfolio portfolio = Portfolio.ParseFormat(line);
					if (portfolio.Owner == owner)
					{
						return portfolio;
					}
				}
			}

			return null;
		}

		public void Delete(string owner)
		{
			string tmpPath = Path.GetTempPath() + Guid.NewGuid();

			using (StreamReader portfolioReader = new StreamReader(_portfolioDbPath))
			using (StreamWriter tmpWriter = new StreamWriter(tmpPath))
			{
				string line;
				while ((line = portfolioReader.ReadLine()) != null)
				{
					Portfolio portfolio = Portfolio.ParseFormat(line);
					if (portfolio.Owner != owner)
					{
						tmpWriter.WriteLine(line);
					}
				}
			}

			File.Delete(_portfolioDbPath);
			File.Copy(tmpPath, _portfolioDbPath, true);
		}

		public void Update(Portfolio update)
		{
			// Get old value of portfolio
			Portfolio old = Get(update.Owner);

			// If old exists then delete it
			if (old != null)
			{
				Delete(update.Owner);
			}

			// Insert updated one
			Insert(update);
		}

		public List<Portfolio> GetAll()
		{
			return File.ReadLines(_portfolioDbPath).ToList().ToPortfolio();
		}

		public void PrintAll()
		{
			using (StreamReader portfolioReader = new StreamReader(_portfolioDbPath))
			{
				string line;
				while ((line = portfolioReader.ReadLine()) != null)
				{
					Console.WriteLine(line);
				}
			}
		}
	}
}