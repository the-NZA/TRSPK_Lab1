using System;
using System.Collections.Generic;
using BusinessLayer;
using DbLayer;
using DbLayer.Models;
using DbLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UILayerWebsite.Models;

namespace UILayerWebsite.Controllers
{
	public class PortfoliosController : Controller
	{
		private readonly ILogger<PortfoliosController> _logger;
		private readonly Solver _solver;

		public PortfoliosController(ILogger<PortfoliosController> logger)
		{
			_logger = logger;
			// Init db
			var db = new Db(
				new StockRepository(Helpers.DefaultStocksDbPath),
				new PortfolioRepository(Helpers.DefaultPortfolioDbPath)
			);

			// Init solver
			_solver = new Solver(db);
		}

		// Index
		public IActionResult Index()
		{
			return View();
		}

		// GetOne
		public IActionResult GetOne(string ownerName)
		{
			PortfoliosGetOneModel model = new PortfoliosGetOneModel();
			try
			{
				// Get all portfolios
				model.Portfolios = _solver.GetAllPortfolios();

				if (!String.IsNullOrWhiteSpace(ownerName))
				{
					// Find index of selected portfolio
					model.Idx = model.Portfolios.FindIndex(
						s => s.Owner == ownerName
					);
					if (model.Idx >= 0)
					{
						model.IsResult = true;
					}
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				model.IsError = true;
				model.ErrMessage = e.Message;
			}

			return View(model);
		}

		// GetAll
		public IActionResult GetAll()
		{
			List<Portfolio> portfolios;
			try
			{
				portfolios = _solver.GetAllPortfolios();
			}
			catch (Exception e)
			{
				portfolios = null;
				_logger.LogError(e.ToString());
			}

			return View(portfolios);
		}

		// Create 
		public IActionResult Create(string ownerName, string stockName, string date, int amount, string dealStr)
		{
			PortfoliosCreateModel model = new PortfoliosCreateModel();
			try
			{
				if (!String.IsNullOrWhiteSpace(ownerName) &&
				    !String.IsNullOrWhiteSpace(date) &&
				    !String.IsNullOrWhiteSpace(dealStr))
				{
					// Check for existing
					if (_solver.GetPortfolio(ownerName) != null)
					{
						throw new Exception("Портфель уже существует");
					}

					// Get date
					DateTime newDate;
					if (!DateTime.TryParse(date, out newDate))
					{
						throw new Exception("Введена некорректная дата");
					}

					// Check amount
					if (amount <= 0)
					{
						throw new Exception("Количество должно быть > 0");
					}

					// Get deal type
					DealType dealType;
					switch (dealStr)
					{
						case "Покупка":
							dealType = DealType.Purchase;
							break;

						case "Продажа":
							dealType = DealType.Sale;
							break;

						default:
							throw new Exception("Введен неверный тип сделки");
					}

					// Get stock
					Stock stock = _solver.GetStock(stockName);
					if (stock == null)
					{
						throw new Exception("Выбрана несуществующая ценная бумага");
					}

					// Create new portfolio
					Portfolio newPortfolio = new Portfolio(ownerName, new List<Deal>
					{
						new Deal(stock, dealType, newDate, amount)
					});

					// Save new portfolio
					if (_solver.InsertPortfolio(newPortfolio))
					{
						model.IsResult = true;
					}
				}

				model.Stocks = _solver.GetAllStocks();
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				model.IsError = true;
				model.ErrMessage = e.Message;
			}

			return View(model);
		}

		// Delete
		public IActionResult Delete(string ownerName)
		{
			PortfoliosDeleteModel model = new PortfoliosDeleteModel();
			try
			{
				// If portfolio name selected than delete it
				if (!String.IsNullOrWhiteSpace(ownerName))
				{
					_solver.DeletePortfolio(ownerName);
					model.IsResult = true;
				}

				// Get all portfolios
				model.Portfolios = _solver.GetAllPortfolios();
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				model.IsError = true;
				model.ErrMessage = e.Message;
			}

			return View(model);
		}

		// Edit
		public IActionResult Edit(string ownerName, string stockName, string date, int amount, string dealStr)
		{
			PortfoliosEditModel model = new PortfoliosEditModel();
			try
			{
				if (!String.IsNullOrWhiteSpace(ownerName) &&
				    !String.IsNullOrWhiteSpace((dealStr)) &&
				    !String.IsNullOrWhiteSpace(date))
				{
					// Check for existing
					if (_solver.GetPortfolio(ownerName) == null)
					{
						throw new Exception("Портфель не существует");
					}

					// Get date
					DateTime newDate;
					if (!DateTime.TryParse(date, out newDate))
					{
						throw new Exception("Введена некорректная дата");
					}

					// Check amount
					if (amount <= 0)
					{
						throw new Exception("Количество должно быть > 0");
					}

					// Get deal type
					DealType dealType;
					switch (dealStr)
					{
						case "Покупка":
							dealType = DealType.Purchase;
							break;

						case "Продажа":
							dealType = DealType.Sale;
							break;

						default:
							throw new Exception("Введен неверный тип сделки");
					}

					// Get stock
					Stock stock = _solver.GetStock(stockName);
					if (stock == null)
					{
						throw new Exception("Выбрана несуществующая ценная бумага");
					}

					// Find portfolio
					Portfolio foundPortfolio = _solver.GetPortfolio(ownerName);
					if (foundPortfolio == null)
					{
						throw new Exception("Портфеля не существует");
					}

					foundPortfolio.AddDeal(new Deal(stock, dealType, newDate, amount));

					// Update portfolio
					_solver.UpdatePortfolio(foundPortfolio);
					model.IsResult = true;
				}

				model.Portfolios = _solver.GetAllPortfolios();
				model.Stocks = _solver.GetAllStocks();
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				model.IsError = true;
				model.ErrMessage = e.Message;
			}

			return View(model);
		}
	}
}