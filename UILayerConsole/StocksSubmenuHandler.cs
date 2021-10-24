using System;
using BusinessLayer;
using DbLayer.Models;

namespace UILayerConsole
{
	public static class StocksSubmenuHandler
	{
		public static void GetStock(Solver solver)
		{
			try
			{
				var stockName = HelperFuncs.ReadString("Введите название ценной бумаги").ToUpper();

				var s = solver.GetStock(stockName);
				if (s == null)
				{
					Console.WriteLine($"Ценной бумаги с именем {stockName} не существует\n");
					return;
				}

				Console.WriteLine("{0}\n", s);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		public static void GetStocks(Solver solver)
		{
			try
			{
				Console.WriteLine("Все ценные бумаги:");
				foreach (var stock in solver.GetAllStocks())
				{
					Console.WriteLine("{0}\n", stock);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		public static void CreateStock(Solver solver)
		{
			try
			{
				string newStockName =
					HelperFuncs.ReadString("Введите название новой ценной бумаги").ToUpper();

				// Check for existing
				if (solver.GetStock(newStockName) != null)
				{
					Console.WriteLine("Ценная бумага с таким именем уже существует\n");
					return;
				}

				// Create empty stock
				Stock newStock = new Stock(newStockName, null);

				// Read date
				DateTime newDate = HelperFuncs.ReadDate("Введите начальные курс и дату\nВведите дату");

				// Read rate
				decimal newRate = HelperFuncs.ReadDecimal("Введите курс");

				if (newRate < 0)
				{
					Console.WriteLine("Начальный курс должен быть >= 0\n");
					return;
				}

				// Add new rate to new stock
				newStock.AddRate(new Rate(newDate, newRate));

				// Save stock
				if (solver.InsertStock(newStock))
				{
					Console.WriteLine("Новая ценная бумага добавлена\n");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		public static void DeleteStock(Solver solver)
		{
			try
			{
				// Read stock name
				var stockName = HelperFuncs.ReadString("Введите название ценной бумаги").ToUpper();

				// Delete stock with entered name
				solver.DeleteStock(stockName);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		public static void EditStock(Solver solver)
		{
			try
			{
				// Read stock name
				var stockName = HelperFuncs.ReadString("Введите название ценной бумаги").ToUpper();

				// Find stock
				Stock foundStock = solver.GetStock(stockName);
				if (foundStock == null)
				{
					Console.WriteLine("Ценная бумага с таким именем не существует\n");
					return;
				}

				// Read new date
				DateTime newDate = HelperFuncs.ReadDate("Введите дату");

				// Read new rate
				decimal newRate = HelperFuncs.ReadDecimal("Введите курс");

				// Add new rate
				foundStock.AddRate(new Rate(newDate, newRate));

				// Save updated stock
				solver.UpdateStock(foundStock);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}
	}
}