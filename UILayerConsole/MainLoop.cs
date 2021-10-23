using System;
using System.Collections.Generic;
using BusinessLayer;
using DbLayer.Models;

namespace UILayerConsole
{
	public class MainLoop
	{
		private readonly Solver _slvr;

		public MainLoop(Solver slvr)
		{
			_slvr = slvr;
		}

		public void Run()
		{
			bool isRun = true;
			while (isRun)
			{
				List<Portfolio> portfolios;
				List<Stock> stocks;
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
						portfolios = _slvr.GetAllPortfolios();
						for (int i = 0; i < portfolios.Count; i++)
						{
							Console.WriteLine($"{i}.   {portfolios[i].Owner}");
						}

						try
						{
							// Read owners' index
							uint idx = HelperFuncs.ReadUint(
								"Введите номер пользователя",
								(uint) portfolios.Count
							);

							// Read date
							DateTime date =
								HelperFuncs.ReadDate("Введите интересующую дату");

							// Solve firsh assignment
							var resultOne = _slvr.SolveOne(portfolios[(int) idx].Owner,
								date);
							Console.WriteLine(
								$"Пользователь: {portfolios[(int) idx].Owner}," +
								$" Дата: {date:yyyy MMMM dd}, Стоимость: {resultOne}\n"
							);
						}
						catch (NullReferenceException)
						{
							Console.WriteLine(
								"Выбранная дата отсутсвует\n"
							);
						}
						catch (Exception e)
						{
							Console.WriteLine($"Error: {e.Message}");
						}

						break;

					case 2:
						stocks = _slvr.GetAllStocks();
						for (int i = 0; i < stocks.Count; i++)
						{
							Console.WriteLine($"{i}.   {stocks[i].Name}");
						}

						try
						{
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

							int i = 1;
							foreach (var r in resultTwo)
							{
								Console.WriteLine(
									$"{i}.  {r.Key}   {r.Value.Rate}   {r.Value.Percent}"
								);
								i++;
							}
						}
						catch (Exception e)
						{
							Console.WriteLine($"Error: {e.Message}");
						}

						break;

					case 3:
						portfolios = _slvr.GetAllPortfolios();
						for (int i = 0; i < portfolios.Count; i++)
						{
							Console.WriteLine($"{i}.   {portfolios[i].Owner}");
						}

						try
						{
							// Read use index
							uint idx = HelperFuncs.ReadUint(
								"Введите номер пользователя",
								(uint) portfolios.Count
							);

							// Read start date
							DateTime dateFrom = HelperFuncs.ReadDate(
								"Введите начальную дату"
							);

							// Read end date
							DateTime dateTo = HelperFuncs.ReadDate(
								"Введите начальную дату"
							);

							// Solve third assignment
							var resultThree = _slvr.SolveThree(
								portfolios[(int) idx].Owner,
								dateFrom,
								dateTo
							);

							Console.WriteLine(
								$"Покупки: {resultThree.pTurnover}," +
								$" продажи: {resultThree.sTurnover}\n"
							);
						}
						catch (Exception e)
						{
							Console.WriteLine($"Error: {e.Message}");
						}

						break;

					case 4:
						// Print submenu and read command number
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
							break;
						}

						string stockName; // store single stock name
						switch (cmdNum)
						{
							case 1:
								try
								{
									stockName = HelperFuncs.ReadString(
										"Введите название ценной бумаги"
									).ToUpper();

									var s = _slvr.GetStock(stockName);
									if (s == null)
									{
										Console.WriteLine(
											$"Ценной бумаги с именем {stockName} не существует\n"
										);
										break;
									}

									Console.WriteLine("{0}\n", s);
								}
								catch (Exception e)
								{
									Console.WriteLine($"Error: {e.Message}");
								}

								break;

							case 2:
								Console.WriteLine("Все ценные бумаги:");
								foreach (
									var stock in _slvr.GetAllStocks()
								)
								{
									Console.WriteLine("{0}\n", stock);
								}

								break;

							case 3:
								try
								{
									string newStockName =
										HelperFuncs.ReadString(
											"Введите название новой ценной бумаги"
										).ToUpper();

									// Check for existing
									if (_slvr.GetStock(newStockName) != null)
									{
										Console.WriteLine(
											"Ценная бумага с таким именем уже существует\n"
										);
										break;
									}

									// Create empty stock
									Stock newStock = new Stock(newStockName,
										null);

									// Read date
									DateTime newDate = HelperFuncs.ReadDate(
										"Введите начальные курс и дату\n" +
										"Введите дату"
									);

									// Read rate
									decimal newRate =
										HelperFuncs.ReadDecimal(
											"Введите курс"
										);

									if (newRate < 0)
									{
										Console.WriteLine(
											"Начальный курс должен быть >= 0\n"
										);
										break;
									}

									// Add new rate to new stock
									newStock.AddRate(new Rate(
										newDate,
										newRate)
									);

									// Save stock
									if (_slvr.InsertStock(newStock))
									{
										Console.WriteLine(
											"Новая ценная бумага добавлена\n"
										);
									}
								}
								catch (Exception e)
								{
									Console.WriteLine(
										$"Error: {e.Message}"
									);
								}

								break;

							case 4:
								try
								{
									// Read stock name
									stockName = HelperFuncs.ReadString(
										"Введите название ценной бумаги"
									).ToUpper();

									// Delete stock with entered name
									_slvr.DeleteStock(stockName);
								}
								catch (Exception e)
								{
									Console.WriteLine(
										$"Error: {e.Message}"
									);
								}

								break;

							case 5:
								try
								{
									// Read stock name
									stockName = HelperFuncs.ReadString(
										"Введите название ценной бумаги"
									).ToUpper();

									// Find stock
									Stock foundStock = _slvr.GetStock(stockName);
									if (foundStock == null)
									{
										Console.WriteLine(
											"Ценная бумага с таким именем не существует\n"
										);
										break;
									}

									// Read new date
									DateTime newDate = HelperFuncs.ReadDate(
										"Введите дату"
									);

									// Read new rate
									decimal newRate =
										HelperFuncs.ReadDecimal(
											"Введите курс"
										);

									// Add new rate
									foundStock.AddRate(new Rate(
										newDate,
										newRate)
									);

									// Save updated stock
									_slvr.UpdateStock(foundStock);
								}
								catch (Exception e)
								{
									Console.WriteLine(
										$"Error: {e.Message}"
									);
								}

								break;

							case 6:
								Console.Clear();
								break;

							default:
								Console.WriteLine(
									"Введен неверный номер операции"
								);
								break;
						}

						break;

					case 5:
						// Print submenu and read command number
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

						string ownerName; // Handle one owner's name
						switch (cmdNum)
						{
							case 1:
								try
								{
									ownerName = HelperFuncs.ReadString(
										"Введите имя владельца портфеля"
									);

									var p = _slvr.GetPortfolio(ownerName);
									if (p == null)
									{
										Console.WriteLine(
											$"Портфеля с владельцем {ownerName} не существует\n"
										);
										break;
									}

									Console.WriteLine("{0}\n", p);
								}
								catch (Exception e)
								{
									Console.WriteLine($"Error: {e.Message}");
								}

								break;

							case 2:
								Console.WriteLine("Все портфели:");
								foreach (var port in _slvr.GetAllPortfolios())
								{
									Console.WriteLine("{0}\n", port);
								}

								break;

							case 3:
								try
								{
									string newOwnerName = HelperFuncs.ReadString(
										"Введите имя владельца нового портфеля"
									);

									// Check for existing
									if (_slvr.GetPortfolio(newOwnerName) != null)
									{
										Console.WriteLine(
											"Портфель с таким владельцем уже существует\n"
										);
										break;
									}

									// Create empty portfolio
									Portfolio newPortfolio =
										new Portfolio(newOwnerName, null);

									Console.WriteLine(
										"Введите данные для первой сделки"
									);

									stocks = _slvr.GetAllStocks();
									for (int i = 0; i < stocks.Count; i++)
									{
										Console.WriteLine(
											$"{i}.   {stocks[i].Name}"
										);
									}

									// Read index
									uint idx = HelperFuncs.ReadUint(
										"Введите номер ценной бумаги",
										(uint) stocks.Count
									);

									// Read new deal type
									DealType dealType;
									switch (HelperFuncs.ReadInt(
										"Выберите тип сделки:\n" +
										"1. Покупка\n" +
										"2. Продажа")
									)
									{
										case 1:
											dealType = DealType.Purchase;
											break;
										case 2:
											dealType = DealType.Sale;
											break;
										default:
											throw new Exception(
												"Введен недопустимый номер"
											);
									}

									// Read new date
									DateTime newDate = HelperFuncs.ReadDate(
										"Введите дату сделки"
									);

									// Read new amount 
									int newAmount = HelperFuncs.ReadInt(
										"Введите количество бумаг"
									);
									if (newAmount <= 0)
									{
										Console.WriteLine(
											"Количество бумаг должно быть больше 0"
										);
										break;
									}

									// Create new deal
									Deal newDeal = new Deal(
										stocks[(int) idx], dealType,
										newDate, newAmount
									);

									// Add new deal 
									newPortfolio.AddDeal(newDeal);

									// Save portfolio
									if (_slvr.InsertPortfolio(newPortfolio))
									{
										Console.WriteLine(
											"Новый портфель создан\n"
										);
									}
								}
								catch (Exception e)
								{
									Console.WriteLine($"Error: {e.Message}");
								}

								break;

							case 4:
								try
								{
									// Read owner's name
									ownerName = HelperFuncs.ReadString(
										"Введите имя владельца портфеля"
									);

									// Delete portfolio by owner's name
									_slvr.DeletePortfolio(ownerName);
								}
								catch (Exception e)
								{
									Console.WriteLine(
										$"Error: {e.Message}"
									);
								}

								break;

							case 5:
								try
								{
									// Read owner's name
									ownerName = HelperFuncs.ReadString(
										"Введите имя владельца портфеля"
									);

									// Find portfolio
									Portfolio foundPortfolio =
										_slvr.GetPortfolio(ownerName);
									if (foundPortfolio == null)
									{
										Console.WriteLine(
											"Портфеля с таким владельцем не существует"
										);
										break;
									}

									// Read values for new deal
									Console.WriteLine(
										"Введите данные для новой сделки"
									);

									stocks = _slvr.GetAllStocks();
									for (int i = 0; i < stocks.Count; i++)
									{
										Console.WriteLine(
											$"{i}.   {stocks[i].Name}"
										);
									}

									// Read index
									uint idx = HelperFuncs.ReadUint(
										"Введите номер ценной бумаги",
										(uint) stocks.Count
									);

									// Read new deal type
									DealType dealType;
									switch (HelperFuncs.ReadInt(
										"Выберите тип сделки:\n" +
										"1. Покупка\n" +
										"2. Продажа")
									)
									{
										case 1:
											dealType = DealType.Purchase;
											break;
										case 2:
											dealType = DealType.Sale;
											break;
										default:
											throw new Exception(
												"Введен недопустимый номер"
											);
									}

									// Read new date
									DateTime newDate = HelperFuncs.ReadDate(
										"Введите дату сделки"
									);

									// Read new amount 
									int newAmount = HelperFuncs.ReadInt(
										"Введите количество бумаг"
									);
									if (newAmount <= 0)
									{
										Console.WriteLine(
											"Количество бумаг должно быть больше 0"
										);
										break;
									}

									// Create new deal
									Deal newDeal = new Deal(
										stocks[(int) idx], dealType,
										newDate, newAmount
									);

									// Add new deal 
									foundPortfolio.AddDeal(newDeal);

									// Save updated portfolio
									_slvr.UpdatePortfolio(foundPortfolio);
								}
								catch (Exception e)
								{
									Console.WriteLine($"Error: {e.Message}");
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