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
	}

	public interface IPortfolioRepository
	{
		bool Insert(Portfolio portfolio);
		Portfolio Get(string owner);
		void Update(Portfolio portfolio);
		public void Delete(string owner);
		List<Portfolio> GetAll();
	}
}