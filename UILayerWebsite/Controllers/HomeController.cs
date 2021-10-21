using System;
using System.Diagnostics;
using System.Linq;
using BusinessLayer;
using DbLayer;
using DbLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UILayerWebsite.Models;

namespace UILayerWebsite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly Db _db;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;

			// Init database
			_db = new Db(new StockRepository(Helpers.DefaultStocksDbPath),
				new PortfolioRepository(Helpers.DefaultPortfolioDbPath));
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(
				new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
		}

		public IActionResult First(string owner, string date)
		{
			FirstModel model = new FirstModel();

			try
			{
				if (!String.IsNullOrWhiteSpace(owner) && !String.IsNullOrWhiteSpace(date))
				{
					DateTime parsedDate = DateTime.Parse(date);
					Solver slvr = new Solver(_db);

					model.Result = slvr.SolveOne(owner, parsedDate);
					model.Owner = owner;
					model.Date = parsedDate;
					model.IsRes = true;
				}
			}
			catch (NullReferenceException e)
			{
				_logger.LogError(e.ToString());
				model.IsError = true;
				model.ErrMessage = "Выбранная дата отсутствует";
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				model.IsError = true;
				model.ErrMessage = "Внутренняя ошибка";
			}

			model.Portfolios = _db.PortfolioRepository.GetAll();
			return View(model);
		}

		public IActionResult Second(string[] stockNames)
		{
			SecondModel model = new SecondModel();
			try
			{
				if (stockNames.Length > 0)
				{
					Solver slvr = new Solver(_db);
					model.Result = slvr.SolveTwo(stockNames.ToList());
					model.IsRes = true;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				model.IsError = true;
				model.ErrMessage = "Внутренняя ошибка";
			}

			model.Stocks = _db.StockRepository.GetAll();
			return View(model);
		}

		public IActionResult Third(string owner, string dateFrom, string dateTo)
		{
			ThirdModel model = new ThirdModel();

			try
			{
				if (!String.IsNullOrWhiteSpace(owner) && !String.IsNullOrWhiteSpace(dateFrom) &&
				    !String.IsNullOrWhiteSpace(dateTo))
				{
					DateTime dFrom = DateTime.Parse(dateFrom);
					DateTime dTo = DateTime.Parse(dateTo);
					Solver slvr = new Solver(_db);

					model.Result = slvr.SolveThree(owner, dFrom, dTo);
					model.Owner = owner;
					model.DateFrom = dFrom;
					model.DateTo = dTo;
					model.IsRes = true;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				model.IsError = true;
				model.ErrMessage = "Внутренняя ошибка";
			}

			model.Portfolios = _db.PortfolioRepository.GetAll();
			return View(model);
		}
	}
}