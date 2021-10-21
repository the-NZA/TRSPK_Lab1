using System;
using System.Collections.Generic;
using BusinessLayer;
using DbLayer;
using DbLayer.Models;
using DbLayer.Repository;

namespace UILayerConsole
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
					"4.   Работа с ценными бумагами\n" +
					"5.   Работа с портфелями пользователей\n" +
					"6.   Выход из программы\n> "
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
						catch (NullReferenceException)
						{
							Console.WriteLine(
								"Выбранная дата отсутсвует\n");
						}
						catch (Exception)
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
						catch (Exception)
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
						catch (Exception)
						{
							Console.WriteLine(
								"Error during solving: internal error\n");
						}

						break;

					case 4:
						Console.Write(
							"Подменю ценных бумаг:\n" +
							"1.   Получить одну\n" +
							"2.   Получить все\n" +
							"3.   Создать\n" +
							"4.   Удалить\n" +
							"5.   Редактировать\n" +
							"6.   Назад\n> "
						);

						switch (Convert.ToInt32(Console.ReadLine()))
						{
							case 1:
								Console.Write("Введите название ценной бумаги\n> ");
								tmp = Console.ReadLine().ToUpper();
								if (String.IsNullOrWhiteSpace(tmp))
								{
									Console.WriteLine(
										"Error during reading: entered empty string\n"
									);
									break;
								}

								var s = db.StockRepository.Get(tmp);
								if (s == null)
								{
									Console.WriteLine(
										$"Ценной бумаги с именем {tmp} не существует\n"
									);
									break;
								}

								Console.WriteLine("{0}\n", s);

								break;

							case 2:
								Console.WriteLine("Все ценные бумаги:");
								foreach (var stock in db.StockRepository.GetAll())
								{
									Console.WriteLine("{0}\n", stock);
								}

								break;

							case 3:
								Console.Write(
									"Введите название для новой ценной бумаги\n> "
								);
								tmp = Console.ReadLine().ToUpper();
								if (String.IsNullOrWhiteSpace(tmp))
								{
									Console.WriteLine(
										"Error during reading: entered empty string\n"
									);
									break;
								}

								if (db.StockRepository.Get(tmp) != null)
								{
									Console.WriteLine(
										"Ценная бумага с таким именем уже существует\n"
									);
									break;
								}

								try
								{
									// Create empty stock
									Stock newStock = new Stock(tmp, null);
									Console.WriteLine(
										"Введите начальные курс и дату"
									);

									Console.Write(
										"Введите дату(формат 01.22.2021)\n> "
									);

									// Read date
									tmp = Console.ReadLine();
									if (String.IsNullOrWhiteSpace(tmp))
									{
										Console.WriteLine(
											"Error during reading: entered empty string");
										break;
									}

									// Get date
									DateTime newDate;
									if (!DateTime.TryParse(tmp, out newDate))
									{
										Console.WriteLine(
											"Error during reading: entered not allowed date format\n");
										break;
									}

									Console.Write(
										"Введите курс\n> "
									);

									// Read rate 
									tmp = Console.ReadLine();
									if (String.IsNullOrWhiteSpace(tmp))
									{
										Console.WriteLine(
											"Error during reading: entered empty string");
										break;
									}

									// Get rate
									decimal newRate = Convert.ToDecimal(tmp);
									if (newRate < 0)
									{
										Console.WriteLine(
											"Начальный курс должен быть >= 0\n");
										break;
									}

									// Add new rate
									newStock.AddRate(new Rate(newDate, newRate));

									// Save stock
									if (db.StockRepository.Insert(newStock))
									{
										Console.WriteLine(
											"Новая ценная бумага добавлена\n"
										);
									}
								}
								catch (Exception e)
								{
									Console.WriteLine(
										$"Error during create new stock: {e.Message}\n");
								}

								break;

							case 4:
								Console.Write("Введите название ценной бумаги\n> ");
								tmp = Console.ReadLine().ToUpper();
								if (String.IsNullOrWhiteSpace(tmp))
								{
									Console.WriteLine(
										"Error during reading: entered empty string\n"
									);
									break;
								}

								// Delete stock with entered name
								db.StockRepository.Delete(tmp);

								break;

							case 5:
								Console.Write("Введите название ценной бумаги\n> ");
								tmp = Console.ReadLine().ToUpper();
								if (String.IsNullOrWhiteSpace(tmp))
								{
									Console.WriteLine(
										"Error during reading: entered empty string\n"
									);
									break;
								}

								Stock foundStock = db.StockRepository.Get(tmp);
								if (foundStock == null)
								{
									Console.WriteLine(
										"Ценная бумага с таким именем не существует\n"
									);
									break;
								}

								try
								{
									Console.Write(
										"Введите дату(формат 01.22.2021)\n> "
									);

									// Read date
									tmp = Console.ReadLine();
									if (String.IsNullOrWhiteSpace(tmp))
									{
										Console.WriteLine(
											"Error during reading: entered empty string");
										break;
									}

									// Get date
									DateTime newDate;
									if (!DateTime.TryParse(tmp, out newDate))
									{
										Console.WriteLine(
											"Error during reading: entered not allowed date format\n");
										break;
									}

									Console.Write(
										"Введите курс\n> "
									);

									// Read rate 
									tmp = Console.ReadLine();
									if (String.IsNullOrWhiteSpace(tmp))
									{
										Console.WriteLine(
											"Error during reading: entered empty string");
										break;
									}

									// Get rate
									decimal newRate = Convert.ToDecimal(tmp);
									if (newRate < 0)
									{
										Console.WriteLine(
											"Начальный курс должен быть >= 0\n");
										break;
									}
									
									foundStock.AddRate(new Rate(newDate, newRate));
									
									db.StockRepository.Update(foundStock);
								}
								catch (Exception e)
								{
									Console.WriteLine(
										$"Error during updating: {e.Message}");
								}

								break;

							case 6:
								Console.Clear();
								break;

							default:
								Console.WriteLine("Введен неверный номер операции");
								break;
						}

						break;

					case 5:
						Console.Write(
							"Подменю портфелей:\n" +
							"1.   Получить один\n" +
							"2.   Получить все\n" +
							"3.   Создать\n" +
							"4.   Удалить\n" +
							"5.   Редактировать\n" +
							"6.   Назад\n> "
						);

						break;

					case 6:
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