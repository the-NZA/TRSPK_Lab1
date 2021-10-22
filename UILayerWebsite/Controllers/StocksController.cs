using System;
using System.Collections.Generic;
using DbLayer;
using DbLayer.Models;
using DbLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UILayerWebsite.Controllers
{
	public class StocksGetOneModel
	{
		public List<Stock> Stocks { get; set; }
		public int Idx { get; set; }
		public bool IsError { get; set; }
		public bool IsResult { get; set; }
		public string ErrMessage { get; set; }
	}

	public class StocksController : Controller
	{
		private readonly ILogger<StocksController> _logger;
		private readonly Db _db;

		public StocksController(ILogger<StocksController> logger)
		{
			_logger = logger;
			// Init db
			_db = new Db(
				new StockRepository(Helpers.DefaultStocksDbPath),
				new PortfolioRepository(Helpers.DefaultPortfolioDbPath)
			);
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
				model.Stocks = _db.StockRepository.GetAll();
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
			return View();
		}

		// Create 
		public IActionResult Create()
		{
			return View();
		}

		// Delete
		public IActionResult Delete()
		{
			return View();
		}

		// Edit
		public IActionResult Edit()
		{
			return View();
		}
	}
}