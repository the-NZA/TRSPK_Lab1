using System;
using BusinessLayer;
using DbLayer;
using DbLayer.Repository;

namespace UILayerConsole
{
	class Program
	{
		static void Main()
		{
			Solver slvr = null;

			// Init repositories, db and solver
			try
			{
				var stockRepo = new StockRepository(Helpers.DefaultStocksDbPath);
				var portfolioRepo = new PortfolioRepository(Helpers.DefaultPortfolioDbPath);
				var db = new Db(stockRepo, portfolioRepo);
				
				slvr = new Solver(db);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error during init: {e}");
			}

			// Create main loop
			MainLoop mainLoop = new MainLoop(slvr);

			// Run main loop
			mainLoop.Run();
		}
	}
}