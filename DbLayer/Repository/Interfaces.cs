using DbLayer.Models;
using System.Collections.Generic;

namespace DbLayer.Repository
{
	public interface IStockRepository
	{
		bool Insert(Stock stock);
		Stock Get(string name);
		void Delete(string name);
		void Update(Stock update);
		List<Stock> GetAll();

		void PrintAll();
	}

	public interface IPortfolioRepository
	{
		bool Insert(Portfolio portfolio);
		Portfolio Get(string owner);
		void Update(Portfolio portfolio);
		List<Portfolio> GetAll();

		void PrintAll();
	}
}