using System;
using System.Linq;
using BusinessLayer;
using DbLayer;
using DbLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UILayerWebsite.Models;

namespace UILayerWebsite.Controllers
{
	public class SolverController : Controller
	{
		private readonly ILogger<SolverController> _logger;
		private readonly Solver _solver;

		public SolverController(ILogger<SolverController> logger)
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

		// First assignment solver
		public IActionResult First(string owner, string date)
		{
			FirstAssignmentModel assignmentModel = new FirstAssignmentModel();

			try
			{
				if (!String.IsNullOrWhiteSpace(owner) && !String.IsNullOrWhiteSpace(date))
				{
					DateTime parsedDate = DateTime.Parse(date);
					
					assignmentModel.Result = _solver.SolveOne(owner, parsedDate);
					assignmentModel.Owner = owner;
					assignmentModel.Date = parsedDate;
					assignmentModel.IsRes = true;
				}
			}
			catch (NullReferenceException e)
			{
				_logger.LogError(e.ToString());
				assignmentModel.IsError = true;
				assignmentModel.ErrMessage = "Выбранная дата отсутствует";
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				assignmentModel.IsError = true;
				assignmentModel.ErrMessage = "Внутренняя ошибка";
			}

			assignmentModel.Portfolios = _solver.GetAllPortfolios();
			return View(assignmentModel);
		}

		// Second assignment solver
		public IActionResult Second(string[] stockNames)
		{
			SecondAssignmentModel assignmentModel = new SecondAssignmentModel();
			try
			{
				if (stockNames.Length > 0)
				{
					assignmentModel.Result = _solver.SolveTwo(stockNames.ToList());
					assignmentModel.IsRes = true;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				assignmentModel.IsError = true;
				assignmentModel.ErrMessage = "Внутренняя ошибка";
			}

			assignmentModel.Stocks = _solver.GetAllStocks();
			return View(assignmentModel);
		}

		// Third assignment solver
		public IActionResult Third(string owner, string dateFrom, string dateTo)
		{
			ThirdAssignmentModel assignmentModel = new ThirdAssignmentModel();
			try
			{
				if (!String.IsNullOrWhiteSpace(owner) && !String.IsNullOrWhiteSpace(dateFrom) &&
				    !String.IsNullOrWhiteSpace(dateTo))
				{
					DateTime dFrom = DateTime.Parse(dateFrom);
					DateTime dTo = DateTime.Parse(dateTo);

					assignmentModel.Result = _solver.SolveThree(owner, dFrom, dTo);
					assignmentModel.Owner = owner;
					assignmentModel.DateFrom = dFrom;
					assignmentModel.DateTo = dTo;
					assignmentModel.IsRes = true;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				assignmentModel.IsError = true;
				assignmentModel.ErrMessage = "Внутренняя ошибка";
			}

			assignmentModel.Portfolios = _solver.GetAllPortfolios();
			return View(assignmentModel);
		}
	}
}