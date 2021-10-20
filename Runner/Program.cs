using System;
using System.Collections.Generic;
using DbLayer;
using DbLayer.Repository;
using BusinessLayer;

namespace Runner
{
	class Program
	{
		static void Main()
		{
			try
			{
				StockRepository stockRepo = new StockRepository(Helpers.DefaultStocksDbPath);
				PortfolioRepository portfolioRepo =
					new PortfolioRepository(Helpers.DefaultPortfolioDbPath);
				Db db = new Db(stockRepo, portfolioRepo);

				db.PortfolioRepository.GetAll();

				Solver slvr = new Solver(db);

				decimal ansOne = slvr.SolveOne("One", DateTime.Today.AddDays(-2));
				Console.WriteLine("First answer: {0}\n", ansOne);

				Dictionary<string, RateWithPercent> ansTwo =  slvr.SolveTwo(new List<string> {"YNDX", "APPL", "GOOG", "SC", "MSFT"});
				Console.WriteLine("Second answer:\n");
				foreach (var item in ansTwo)
				{
					Console.WriteLine(item);
				}

				(decimal, decimal) ansThree = slvr.SolveThree("Three", new DateTime(2021, 10, 01), new DateTime(2021, 10, 17));
				Console.WriteLine("\nThird answer: {0}", ansThree);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}