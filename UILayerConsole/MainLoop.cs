using System;
using System.Collections.Generic;
using BusinessLayer;

namespace UILayerConsole
{
	public class MainLoop
	{
		private readonly Solver _slvr;

		public MainLoop(Solver slvr)
		{
			_slvr = slvr;
		}

		// Do first assignment
		private void FirstAssignment()
		{
			try
			{
				var portfolios = _slvr.GetAllPortfolios();
				for (int i = 0; i < portfolios.Count; i++)
				{
					Console.WriteLine($"{i}.   {portfolios[i].Owner}");
				}

				// Read owners' index
				uint idx = HelperFuncs.ReadUint("Введите номер пользователя", (uint) portfolios.Count);

				// Read date
				DateTime date = HelperFuncs.ReadDate("Введите интересующую дату");

				// Solve firsh assignment
				var resultOne = _slvr.SolveOne(portfolios[(int) idx].Owner, date);

				Console.WriteLine(
					$"Пользователь: {portfolios[(int) idx].Owner}," +
					$" Дата: {date:yyyy MMMM dd}, Стоимость: {resultOne}\n"
				);
			}
			catch (NullReferenceException)
			{
				Console.WriteLine("Выбранная дата отсутсвует\n");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		// Do second assignment
		private void SecondAssignment()
		{
			try
			{
				var stocks = _slvr.GetAllStocks();
				int i;
				for (i = 0; i < stocks.Count; i++)
				{
					Console.WriteLine($"{i}.   {stocks[i].Name}");
				}

				// Read string with indexies
				// Create dict with string and int representation of index
				// Avoiding duplicates
				Dictionary<string, int> indexes = new Dictionary<string, int>();
				foreach (
					var s in HelperFuncs.ReadString(
						"Введите номера ценных бумаг через запятую"
					).Split(",")
				)
				{
					if (!indexes.ContainsKey(s))
					{
						indexes[s] = Convert.ToInt32(s);
					}
				}

				// Get list of stock names
				List<string> stocksNames = new List<string>();
				foreach (var item in indexes)
				{
					stocksNames.Add(stocks[item.Value].Name);
				}

				// Solve second assignment
				var resultTwo = _slvr.SolveTwo(stocksNames);

				i = 1;
				foreach (var r in resultTwo)
				{
					Console.WriteLine($"{i}.  {r.Key}   {r.Value.Rate}   {r.Value.Percent}");
					i++;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		// Do third assignment
		private void ThirdAssignment()
		{
			try
			{
				var portfolios = _slvr.GetAllPortfolios();
				for (int i = 0; i < portfolios.Count; i++)
				{
					Console.WriteLine($"{i}.   {portfolios[i].Owner}");
				}

				// Read use index
				uint idx = HelperFuncs.ReadUint("Введите номер пользователя", (uint) portfolios.Count);

				// Read start date
				DateTime dateFrom = HelperFuncs.ReadDate("Введите начальную дату");

				// Read end date
				DateTime dateTo = HelperFuncs.ReadDate("Введите конечную дату");

				// Solve third assignment
				var resultThree = _slvr.SolveThree(portfolios[(int) idx].Owner, dateFrom, dateTo);

				Console.WriteLine(
					$"Покупки: {resultThree.pTurnover}," +
					$" продажи: {resultThree.sTurnover}\n"
				);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}
		}

		// Do stocks submenu staff
		private void StocksSubmenu()
		{
			// Print submenu and read command number
			int cmdNum = 0;
			try
			{
				cmdNum = HelperFuncs.ReadInt(
					"Подменю ценных бумаг:\n" +
					"1.   Получить одну\n" +
					"2.   Получить все\n" +
					"3.   Создать\n" +
					"4.   Удалить\n" +
					"5.   Редактировать\n" +
					"6.   Назад"
				);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}

			switch (cmdNum)
			{
				case 1:
					// Handle get one stock
					StocksSubmenuHandler.GetStock(_slvr);
					break;

				case 2:
					// Handle get all stocks
					StocksSubmenuHandler.GetStocks(_slvr);
					break;

				case 3:
					// Handle create new stock
					StocksSubmenuHandler.CreateStock(_slvr);
					break;

				case 4:
					// Handle delete stock
					StocksSubmenuHandler.DeleteStock(_slvr);
					break;

				case 5:
					// Handle edit stock
					StocksSubmenuHandler.EditStock(_slvr);
					break;

				case 6:
					// Go back to main menu and clear screen
					Console.Clear();
					break;

				default:
					Console.WriteLine("Введен неверный номер операции");
					break;
			}
		}

		// Do portfolios submenu staff
		private void PortfoliosSubmenu()
		{
			// Print submenu and read command number
			int cmdNum = 0;
			try
			{
				cmdNum = HelperFuncs.ReadInt(
					"Подменю портфелей:\n" +
					"1.   Получить один\n" +
					"2.   Получить все\n" +
					"3.   Создать\n" +
					"4.   Удалить\n" +
					"5.   Редактировать\n" +
					"6.   Назад"
				);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.Message}");
			}

			switch (cmdNum)
			{
				case 1:
					// Handle get one portfolio
					PortfolioSubmenuHandler.GetPorfolio(_slvr);
					break;

				case 2:
					// Handle get all portfolios
					PortfolioSubmenuHandler.GetPorfolios(_slvr);
					break;

				case 3:
					// Handle create new portfolio
					PortfolioSubmenuHandler.CreatePortfolio(_slvr);
					break;

				case 4:
					// Handle delete portfolio
					PortfolioSubmenuHandler.DeletePortfolio(_slvr);
					break;

				case 5:
					// Handle edit portfolio
					PortfolioSubmenuHandler.EditPortfolio(_slvr);
					break;

				case 6:
					// Go back and clear screen
					Console.Clear();
					break;

				default:
					Console.WriteLine("Введен неверный номер операции");
					break;
			}
		}

		// MainLoop runner
		public void Run()
		{
			bool isRun = true;
			while (isRun)
			{
				int cmdNum = 0;

				// Print menu and read command number
				try
				{
					cmdNum = HelperFuncs.ReadInt(
						"Меню:\n" +
						"1.   Стоимость портфеля на выбранный день\n" +
						"2.   Стоимость ценных бумаг на текущий день\n" +
						"3.   Оборот за выбранный период\n" +
						"4.   Работа с ценными бумагами\n" +
						"5.   Работа с портфелями пользователей\n" +
						"6.   Выход из программы"
					);
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error: {e.Message}");
				}

				switch (cmdNum)
				{
					case 1:
						// Handle first assignment
						FirstAssignment();
						break;

					case 2:
						//Handle second assignment 
						SecondAssignment();
						break;

					case 3:
						// Handle third assignment
						ThirdAssignment();
						break;

					case 4:
						// Handle Stocks submenu
						StocksSubmenu();
						break;

					case 5:
						// Handle Portfolios submenu
						PortfoliosSubmenu();
						break;

					case 6:
						try
						{
							cmdNum = HelperFuncs.ReadInt(
								"Вы действительно хотите выйти из программы?\n" +
								"1-Да\n0-Нет"
							);

							if (cmdNum == 1)
							{
								isRun = false;
							}
						}
						catch (Exception e)
						{
							Console.WriteLine($"Error: {e.Message}");
						}

						break;

					default:
						Console.WriteLine("Введен неверный номер операции");
						break;
				}
			}
		}
	}
}