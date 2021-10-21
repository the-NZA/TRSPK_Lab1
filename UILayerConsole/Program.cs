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

			// Init repositories, db and solver
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
				int indx;
				string tmp;
				List<Portfolio> owners;
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
						owners = db.PortfolioRepository.GetAll();
						for (int i = 0; i < owners.Count; i++)
						{
							Console.WriteLine($"{i}.   {owners[i].Owner}");
						}

						try
						{
							// Read owners' index
							uint idx = HelperFuncs.ReadUint(
								"Введите номер пользователя",
								(uint) owners.Count
							);

							// Read date
							DateTime date =
								HelperFuncs.ReadDate("Введите интересующую дату");

							// Solve firsh assignment
							var resultOne = slvr.SolveOne(owners[(int) idx].Owner, date);
							Console.WriteLine(
								$"Пользователь: {owners[(int) idx].Owner}, Дата: {date:yyyy MMMM dd}, Стоимость: {resultOne}\n"
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
						stocks = db.StockRepository.GetAll();
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
									"Введите номера ценных бумаг через запятую:w"
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
							var resultTwo = slvr.SolveTwo(stocksNames);

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
						owners = db.PortfolioRepository.GetAll();
						for (int i = 0; i < owners.Count; i++)
						{
							Console.WriteLine($"{i}.   {owners[i].Owner}");
						}

						try
						{
							// Read use index
							uint idx = HelperFuncs.ReadUint(
								"Введите номер пользователя",
								(uint) owners.Count
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
							var resultThree = slvr.SolveThree(
								owners[(int) idx].Owner,
								dateFrom,
								dateTo
							);

							Console.WriteLine(
								$"Покупки: {resultThree.pTurnover}, продажи: {resultThree.sTurnover}\n"
							);
						}
						catch (Exception e)
						{
							Console.WriteLine($"Error: {e.Message}");
						}

						break;

					case 4:
						try
						{
							cmdNum = HelperFuncs.ReadInt(
								"Подменю ценных бумаг:\n" +
								"1.   Получить одну\n" +
								"2.   Получить все\n" +
								"3.   Создать\n" +
								"4.   Удалить\n" +
								"5.   Редактировать\n" +
								"6.   Назад\n> "
							);

							string stockName; // store single stock name
							switch (cmdNum)
							{
								case 1:
									stockName = HelperFuncs.ReadString(
										"Введите название ценной бумаги"
									);

									var s = db.StockRepository.Get(stockName);
									if (s == null)
									{
										Console.WriteLine(
											$"Ценной бумаги с именем {stockName} не существует\n"
										);
										break;
									}

									Console.WriteLine("{0}\n", s);
									break;

								case 2:
									Console.WriteLine("Все ценные бумаги:");
									foreach (
										var stock in db.StockRepository.GetAll()
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
											);
										newStockName = newStockName.ToUpper();

										// Check for existing
										if (db.StockRepository
											.Get(newStockName) != null)
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
											"Введите начальные курс и дату" +
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
										);

										// Delete stock with entered name
										db.StockRepository.Delete(stockName);
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
										);
										stockName = stockName.ToUpper();

										// Find stock
										Stock foundStock =
											db.StockRepository.Get(
												stockName);
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
										db.StockRepository.Update(foundStock);
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
						}
						catch (Exception e)
						{
							Console.WriteLine($"Error: {e.Message}");
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

						switch (Convert.ToInt32(Console.ReadLine()))
						{
							case 1:
								Console.Write("Введите имя владельца портфеля\n> ");
								tmp = Console.ReadLine();
								if (String.IsNullOrWhiteSpace(tmp))
								{
									Console.WriteLine(
										"Error during reading: entered empty string\n"
									);
									break;
								}

								var p = db.PortfolioRepository.Get(tmp);
								if (p == null)
								{
									Console.WriteLine(
										$"Портфеля с владельцем {tmp} не существует\n"
									);
									break;
								}

								Console.WriteLine("{0}\n", p);
								break;

							case 2:
								Console.WriteLine("Все портфели:");
								foreach (var port in db.PortfolioRepository.GetAll())
								{
									Console.WriteLine("{0}\n", port);
								}

								break;

							case 3:
								Console.Write(
									"Введите имя владельца нового портфеля\n> ");
								tmp = Console.ReadLine();
								if (String.IsNullOrWhiteSpace(tmp))
								{
									Console.WriteLine(
										"Error during reading: entered empty string\n"
									);
									break;
								}


								// Try to create and save new portfolio
								try
								{
									Console.WriteLine(
										"Введите данные для первой сделки\n" +
										"Выберите номер ценной бумаги\n> "
									);

									stocks = db.StockRepository.GetAll();
									for (int i = 0; i < stocks.Count; i++)
									{
										Console.WriteLine(
											$"{i}.   {stocks[i].Name}"
										);
									}

									Console.Write("> ");

									// Read index
									tmp = Console.ReadLine();
									if (String.IsNullOrWhiteSpace(tmp))
									{
										Console.WriteLine(
											"Error during reading: entered empty string");
										break;
									}

									// Get index
									indx = Convert.ToInt32(tmp);

									if (indx < 0 || indx >= stocks.Count)
									{
										Console.WriteLine(
											"Error during reading: entered non-valid number"
										);
										break;
									}

									Console.WriteLine(
										"Выберите тип сделки:\n" +
										"1.   Покупка\n" +
										"2.   Продажа\n> "
									);

									// Read DealType 
									tmp = Console.ReadLine();
									if (String.IsNullOrWhiteSpace(tmp))
									{
										Console.WriteLine(
											"Error during reading: entered empty string"
										);
										break;
									}

									// Get DealType
									DealType dt;
									switch (Convert.ToInt32(tmp))
									{
										case 1:
											dt = DealType.Purchase;
											break;
										case 2:
											dt = DealType.Sale;
											break;
										default:
											Console.WriteLine(
												"Введено недопустимой значение"
											);
											continue;
									}

									Console.Write(
										"Введите дату сделки(формат 01.22.2021)\n> "
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
										"Введите количество бумаг\n> "
									);
									// Read amount 
									tmp = Console.ReadLine();
									if (String.IsNullOrWhiteSpace(tmp))
									{
										Console.WriteLine(
											"Error during reading: entered empty string");
										break;
									}

									// Get amount
									int newAmount = Convert.ToInt32(tmp);

									// Create empty portfolio
									Portfolio newPortfolio =
										new Portfolio(tmp, null);

									Deal newDeal = new Deal(stocks[indx], dt,
										newDate,
										newAmount);

									// Add new deal 
									newPortfolio.AddDeal(newDeal);

									// Save portfolio
									if (db.PortfolioRepository.Insert(newPortfolio))
									{
										Console.WriteLine(
											"Новый портфель создан\n");
									}
								}
								catch (Exception e)
								{
									Console.WriteLine(
										$"Error during creating: {e.Message}\n"
									);
								}

								break;

							case 4:
								// delete
								break;

							case 5:
								// edit
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