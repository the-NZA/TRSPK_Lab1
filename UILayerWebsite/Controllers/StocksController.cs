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
	public class StocksController : Controller
	{
		private readonly ILogger<StocksController> _logger;
		private readonly Solver _solver;

		public StocksController(ILogger<StocksController> logger)
		{
			_logger = logger;
			// Init db
			var db = new Db(
				new StockRepository(Helpers.DefaultStocksDbPath),
				new PortfolioRepository(Helpers.DefaultPortfolioDbPath)
			);

			_solver = new Solver(db);
		}

		// Index
		public IActionResult Index()
		{
			return View();
		}

		// GetOne
		public IActionResult GetOne(string stockName)
		{
			StocksGetOneModel model = new StocksGetOneModel();
			try
			{
				// Get all stocks
				model.Stocks = _solver.GetAllStocks();
				if (!String.IsNullOrWhiteSpace(stockName))
				{
					// Find index of selected stock
					model.Idx = model.Stocks.FindIndex(s => s.Name == stockName);
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
			List<Stock> stocks;
			try
			{
				stocks = _solver.GetAllStocks();
			}
			catch (Exception e)
			{
				stocks = null;
				_logger.LogError(e.ToString());
			}

			return View(stocks);
		}

		// Create 
		public IActionResult Create(string stockName, string date, decimal rate)
		{
			StocksCreateModel model = new StocksCreateModel();
			try
			{
				if (!String.IsNullOrWhiteSpace(stockName) && !String.IsNullOrWhiteSpace(date))
				{
					DateTime newDate;
					if (!DateTime.TryParse(date, out newDate))
					{
						throw new Exception("Введена некорректная дата");
					}

					if (rate <= 0)
					{
						throw new Exception("Стоимость должна быть > 0");
					}

					// Create new stock
					Stock newStock = new Stock(stockName.ToUpper(), new List<Rate>
					{
						new Rate(newDate, rate)
					});

					// save new stock
					if (_solver.InsertStock(newStock))
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

		// Delete
		public IActionResult Delete(string stockName)
		{
			StocksDeleteModel model = new StocksDeleteModel();
			try
			{
				// If stock name selected than delete it
				if (!String.IsNullOrWhiteSpace(stockName))
				{
					_solver.DeleteStock(stockName);
					model.IsResult = true;
				}

				// Get all stocks
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

		// Edit
		public IActionResult Edit(string stockName, string date, decimal rate)
		{
			StocksEditModel model = new StocksEditModel();
			try
			{
				if (!String.IsNullOrWhiteSpace(stockName) && !String.IsNullOrWhiteSpace(date))
				{
					DateTime newDate;
					if (!DateTime.TryParse(date, out newDate))
					{
						throw new Exception("Введена некорректная дата");
					}

					if (rate <= 0)
					{
						throw new Exception("Стоимость должна быть > 0");
					}

					// Create new stock
					Stock foundStock = _solver.GetStock(stockName);
					if (foundStock == null)
					{
						throw new Exception("Ценная бумага не существует");
					}

					// Add new rate
					foundStock.AddRate(new Rate(newDate, rate));

					// save updated stock
					_solver.UpdateStock(foundStock);
					model.IsResult = true;
				}

				// Get all stocks
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