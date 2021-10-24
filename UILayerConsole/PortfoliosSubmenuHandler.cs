using System;
using BusinessLayer;
using DbLayer.Models;

namespace UILayerConsole
{
	public static class PortfolioSubmenuHandler
	{
		public static void GetPorfolio(Solver solver)
		{
			try
			{
				var ownerName = HelperFuncs.ReadString("Введите имя владельца портфеля");

				var p = solver.GetPortfolio(ownerName);
				if (p == null)
				{
					Console.WriteLine($"Портфеля с владельцем {ownerName} не существует\n");
					return;
				}

				Console.WriteLine("{0}\n", p);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		public static void GetPorfolios(Solver solver)
		{
			try
			{
				Console.WriteLine("Все портфели:");
				foreach (var port in solver.GetAllPortfolios())
				{
					Console.WriteLine("{0}\n", port);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		public static void CreatePortfolio(Solver solver)
		{
			try
			{
				string newOwnerName = HelperFuncs.ReadString("Введите имя владельца нового портфеля");

				// Check for existing
				if (solver.GetPortfolio(newOwnerName) != null)
				{
					Console.WriteLine("Портфель с таким владельцем уже существует\n");
					return;
				}

				// Create empty portfolio
				Portfolio newPortfolio = new Portfolio(newOwnerName, null);

				Console.WriteLine("Введите данные для первой сделки");

				var stocks = solver.GetAllStocks();
				for (int i = 0; i < stocks.Count; i++)
				{
					Console.WriteLine($"{i}.   {stocks[i].Name}");
				}

				// Read index
				uint idx = HelperFuncs.ReadUint("Введите номер ценной бумаги", (uint) stocks.Count);

				// Read new deal type
				DealType dealType;
				switch (HelperFuncs.ReadInt("Выберите тип сделки:\n1. Покупка\n2. Продажа"))
				{
					case 1:
						dealType = DealType.Purchase;
						break;
					case 2:
						dealType = DealType.Sale;
						break;
					default:
						throw new Exception("Введен недопустимый номер");
				}

				// Read new date
				DateTime newDate = HelperFuncs.ReadDate("Введите дату сделки");

				// Read new amount 
				int newAmount = HelperFuncs.ReadInt(
					"Введите количество бумаг"
				);
				if (newAmount <= 0)
				{
					Console.WriteLine("Количество бумаг должно быть больше 0");
					return;
				}

				// Create new deal
				Deal newDeal = new Deal(stocks[(int) idx], dealType, newDate, newAmount);

				// Add new deal 
				newPortfolio.AddDeal(newDeal);

				// Save portfolio
				if (solver.InsertPortfolio(newPortfolio))
				{
					Console.WriteLine("Новый портфель создан\n");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		public static void DeletePortfolio(Solver solver)
		{
			try
			{
				// Read owner's name
				var ownerName = HelperFuncs.ReadString("Введите имя владельца портфеля");

				// Delete portfolio by owner's name
				solver.DeletePortfolio(ownerName);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		public static void EditPortfolio(Solver solver)
		{
			try
			{
				// Read owner's name
				var ownerName = HelperFuncs.ReadString("Введите имя владельца портфеля");

				// Find portfolio
				Portfolio foundPortfolio = solver.GetPortfolio(ownerName);
				if (foundPortfolio == null)
				{
					Console.WriteLine("Портфеля с таким владельцем не существует");
					return;
				}

				// Read values for new deal
				Console.WriteLine("Введите данные для новой сделки");

				var stocks = solver.GetAllStocks();
				for (int i = 0; i < stocks.Count; i++)
				{
					Console.WriteLine($"{i}.   {stocks[i].Name}");
				}

				// Read index
				uint idx = HelperFuncs.ReadUint("Введите номер ценной бумаги", (uint) stocks.Count);

				// Read new deal type
				DealType dealType;
				switch (HelperFuncs.ReadInt("Выберите тип сделки:\n1. Покупка\n2. Продажа")
				)
				{
					case 1:
						dealType = DealType.Purchase;
						break;
					case 2:
						dealType = DealType.Sale;
						break;
					default:
						throw new Exception("Введен недопустимый номер");
				}

				// Read new date
				DateTime newDate = HelperFuncs.ReadDate("Введите дату сделки");

				// Read new amount 
				int newAmount = HelperFuncs.ReadInt(
					"Введите количество бумаг"
				);
				if (newAmount <= 0)
				{
					Console.WriteLine("Количество бумаг должно быть больше 0");
					return;
				}

				// Create new deal
				Deal newDeal = new Deal(
					stocks[(int) idx], dealType,
					newDate, newAmount
				);

				// Add new deal 
				foundPortfolio.AddDeal(newDeal);

				// Save updated portfolio
				solver.UpdatePortfolio(foundPortfolio);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}
	}
}