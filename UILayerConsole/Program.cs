using System;
using System.Collections.Generic;
using DbLayer;
using DbLayer.Repository;
using BusinessLayer;
using DbLayer.Models;

namespace Runner
{
	class Program
	{
		static void Main()
		{
			Db db = null;
			Solver slvr = null;

			// Init repositories and db
			try
			{
				StockRepository stockRepo = new StockRepository(Helpers.DefaultStocksDbPath);
				PortfolioRepository portfolioRepo =
					new PortfolioRepository(Helpers.DefaultPortfolioDbPath);
				db = new Db(stockRepo, portfolioRepo);
				slvr = new Solver(db);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error during init: {e}");
			}

			bool isRun = true;
			while (isRun)
			{
				Console.Write(
					"Меню:\n" +
					"1.   Стоимость портфеля на выбранный день\n" +
					"2.   Стоимость ценных бумаг на текущий день\n" +
					"3.   Оборот за выбранный период\n" +
					"4.   Выход из программы\n> "
				);

				string tmp;
				List<Portfolio> owners;

				int idx;
				switch (Convert.ToInt32(Console.ReadLine()))
				{
					case 1:
						Console.WriteLine("Введите номер пользователя");
						owners = db.PortfolioRepository.GetAll();

						for (int i = 0; i < owners.Count; i++)
						{
							Console.WriteLine($"{i}.   {owners[i].Owner}");
						}

						Console.Write("> ");

						// Read index
						tmp = Console.ReadLine();
						if (String.IsNullOrWhiteSpace(tmp))
						{
							Console.WriteLine("Error during reading: entered empty string");
							break;
						}

						// Get index
						idx = Convert.ToInt32(tmp);

						if (idx < 0 || idx >= owners.Count)
						{
							Console.WriteLine(
								"Error during reading: entered non-valid number"
							);
							break;
						}

						Console.Write("Введите интересующую вас дату(формат 01.22.2021)\n> ");
						
						// Read date
						tmp = Console.ReadLine();
						if (String.IsNullOrWhiteSpace(tmp))
						{
							Console.WriteLine("Error during reading: entered empty string");
							break;
						}

						// Get date
						DateTime date;
						if (!DateTime.TryParse(tmp, out date))
						{
							Console.WriteLine(
								"Error during reading: entered not allowed date format\n");
							break;
						}

						// Try to solve first assigment or handle errors
						try
						{
							var resultOne = slvr.SolveOne(owners[idx].Owner, date);
							Console.WriteLine(
								$"Пользователь: {owners[idx].Owner}, Дата: {date:yyyy MMMM dd}, Стоимость: {resultOne}\n");
						}
						catch (NullReferenceException e)
						{
							Console.WriteLine(
								"Выбранная дата отсутсвует\n");
						}
						catch (Exception e)
						{
							Console.WriteLine(
								"Error during solving: internal error\n");
						}

						break;

					case 2:
						Console.WriteLine("Введите номера ценных бумаг через запятую");
						var stocks = db.StockRepository.GetAll();

						for (int i = 0; i < stocks.Count; i++)
						{
							Console.WriteLine($"{i}.   {stocks[i].Name}");
						}

						Console.Write("> ");
						
						// Read indexes
						tmp = Console.ReadLine();
						if (String.IsNullOrWhiteSpace(tmp))
						{
							Console.WriteLine(
								"Error during reading: entered empty string\n"
							);
							break;
						}

						// Create dict with string and int representation of index
						// Avoiding duplicates
						Dictionary<string, int> indexes = new Dictionary<string, int>();
						foreach (var s in tmp.Split(","))
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


						// Try solve second assigment or handle errors
						try
						{
							var resultTwo = slvr.SolveTwo(stocksNames);
							int i = 1;
							foreach (var r in resultTwo)
							{
								Console.WriteLine(
									$"{i}.  {r.Key}   {r.Value.Rate}   {r.Value.Percent}");
								i++;
							}
						}
						catch (Exception e)
						{
							Console.WriteLine(
								"Error during solving: internal error\n");
						}

						break;

					case 3:
						Console.WriteLine("Введите номер пользователя");
						owners = db.PortfolioRepository.GetAll();

						for (int i = 0; i < owners.Count; i++)
						{
							Console.WriteLine($"{i}.   {owners[i].Owner}");
						}

						Console.Write("> ");

						// Get index
						tmp = Console.ReadLine();
						if (String.IsNullOrWhiteSpace(tmp))
						{
							Console.WriteLine("Error during reading: entered empty string");
							break;
						}

						idx = Convert.ToInt32(tmp);

						if (idx < 0 || idx >= owners.Count)
						{
							Console.WriteLine(
								"Error during reading: entered non-valid number"
							);
							break;
						}

						Console.Write("Введите начальную дату(формат 01.22.2021)\n> ");
						tmp = Console.ReadLine();
						if (String.IsNullOrWhiteSpace(tmp))
						{
							Console.WriteLine("Error during reading: entered empty string");
							break;
						}

						// Get date from
						DateTime dateFrom;
						if (!DateTime.TryParse(tmp, out dateFrom))
						{
							Console.WriteLine(
								"Error during reading: entered not allowed date format\n");
							break;
						}

						Console.Write("Введите конечную дату(формат 01.22.2021)\n> ");
						tmp = Console.ReadLine();
						if (String.IsNullOrWhiteSpace(tmp))
						{
							Console.WriteLine("Error during reading: entered empty string");
							break;
						}

						// Get date to
						DateTime dateTo;
						if (!DateTime.TryParse(tmp, out dateTo))
						{
							Console.WriteLine(
								"Error during reading: entered not allowed date format\n");
							break;
						}

						try
						{
							var resultThree = slvr.SolveThree(owners[idx].Owner, dateFrom,
								dateTo);
							Console.WriteLine(
								$"Покупки: {resultThree.pTurnover}, продажи: {resultThree.sTurnover}\n"
							);
						}
						catch (Exception e)
						{
							Console.WriteLine(
								"Error during solving: internal error\n");
						}

						break;
					case 4:
						Console.Write(
							"Вы действительно хотите выйти из программы?\n" +
							"1-Да\n0-Нет\n> "
						);
						if (Convert.ToInt32(Console.ReadLine()) == 1)
						{
							isRun = false;
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