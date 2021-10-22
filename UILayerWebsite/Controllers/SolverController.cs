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
		private readonly Db _db;

		public SolverController(ILogger<SolverController> logger)
		{
			_logger = logger;

			// Init db
			_db = new Db(
				new StockRepository(Helpers.DefaultStocksDbPath),
				new PortfolioRepository(Helpers.DefaultPortfolioDbPath)
			);
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
					Solver slvr = new Solver(_db);

					assignmentModel.Result = slvr.SolveOne(owner, parsedDate);
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

			assignmentModel.Portfolios = _db.PortfolioRepository.GetAll();
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
					Solver slvr = new Solver(_db);
					assignmentModel.Result = slvr.SolveTwo(stockNames.ToList());
					assignmentModel.IsRes = true;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				assignmentModel.IsError = true;
				assignmentModel.ErrMessage = "Внутренняя ошибка";
			}
		
			assignmentModel.Stocks = _db.StockRepository.GetAll();
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
					Solver slvr = new Solver(_db);
		
					assignmentModel.Result = slvr.SolveThree(owner, dFrom, dTo);
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
		
			assignmentModel.Portfolios = _db.PortfolioRepository.GetAll();
			return View(assignmentModel);
		}
	}
}