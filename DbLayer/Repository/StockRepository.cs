using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DbLayer.Models;

namespace DbLayer.Repository
{
	public class StockRepository : IStockRepository
	{
		private readonly string _stocksDbPath;

		public StockRepository(string stockDbPath)
		{
			if (String.IsNullOrWhiteSpace(stockDbPath))
			{
				throw new Exception("Stock DB path is required");
			}

			_stocksDbPath = stockDbPath;
		}

		public bool Insert(Stock stock)
		{
			// Lookup stock and exit if it exists
			if (Get(stock.Name) != null)
			{
				throw new Exception("Item already exist");
			}

			// Insert stock otherwise
			using (StreamWriter stockWriter = new StreamWriter(_stocksDbPath, true))
			{
				stockWriter.Write(stock.Marshal());
			}

			return true;
		}

		public Stock Get(string name)
		{
			using (StreamReader stockReader = new StreamReader(_stocksDbPath))
			{
				// Lookup stock by it's name
				string line;
				while ((line = stockReader.ReadLine()) != null)
				{
					Stock stock = Stock.ParseFormat(line);
					if (stock.Name == name)
					{
						// Return if found
						return stock;
					}
				}
			}

			// Return null otherwise
			return null;
		}

		public void Delete(string name)
		{
			// Get unique tmp file path
			string tmpPath = Path.GetTempPath() + Guid.NewGuid();

			using (StreamReader stockReader = new StreamReader(_stocksDbPath))
			using (StreamWriter tmpWriter = new StreamWriter(tmpPath))
			{
				// Copy all lines except line which contains name as stock's name
				string line;
				while ((line = stockReader.ReadLine()) != null)
				{
					Stock stock = Stock.ParseFormat(line);
					if (stock.Name != name)
					{
						tmpWriter.WriteLine(line);
					}
				}
			}

			// Copy tmp file to current stock file
			File.Delete(_stocksDbPath);
			File.Copy(tmpPath, _stocksDbPath, true);
		}

		public void Update(Stock update)
		{
			// Get old value of stock
			Stock old = Get(update.Name);

			// If old exist then delete it 
			if (old != null)
			{
				Delete(update.Name);
			}

			// Insert updated one
			Insert(update);
		}

		public List<Stock> GetAll()
		{
			return File.ReadLines(_stocksDbPath).ToList().ToStocks();
		}

		public void PrintAll()
		{
			using (StreamReader stockReader = new StreamReader(_stocksDbPath))
			{
				string line;
				while ((line = stockReader.ReadLine()) != null)
				{
					Console.WriteLine(line);
				}
			}

			// List<string> lines = File.ReadLines(_stocksDbPath).ToList();
			// foreach (string line in lines)
			// {
			// 	Console.WriteLine(line);
			// }
		}
	}
}