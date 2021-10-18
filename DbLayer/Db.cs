using System;
using System.Collections.Generic;
using DbLayer.Models;
using DbLayer.Repository;

namespace DbLayer
{
	public class Db
	{
		public IStockRepository StockRepository { get; }
		public IPortfolioRepository PortfolioRepository { get; }

		public Db(IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
		{
			StockRepository = stockRepository;
			PortfolioRepository = portfolioRepository;
		}

		public void SetupStocks()
		{
			this.StockRepository.Insert(new Stock("YNDX", new List<Rate>
			{
				new Rate(new DateTime(2021, 10, 1), 79.40m),
				new Rate(new DateTime(2021, 10, 2), 79.83m),
				new Rate(new DateTime(2021, 10, 3), 79.35m),
				new Rate(new DateTime(2021, 10, 4), 79.3m),
				new Rate(new DateTime(2021, 10, 5), 79.51m),
				new Rate(new DateTime(2021, 10, 6), 80m),
				new Rate(new DateTime(2021, 10, 7), 79.98m),
				new Rate(new DateTime(2021, 10, 8), 79.91m),
				new Rate(new DateTime(2021, 10, 9), 80.3m),
				new Rate(new DateTime(2021, 10, 10), 80.12m),
				new Rate(new DateTime(2021, 10, 11), 79.93m),
				new Rate(new DateTime(2021, 10, 12), 78.3m),
				new Rate(new DateTime(2021, 10, 13), 78.6m),
				new Rate(new DateTime(2021, 10, 14), 78.9m),
				new Rate(new DateTime(2021, 10, 15), 79.3m),
				new Rate(new DateTime(2021, 10, 16), 79.8m),
				new Rate(new DateTime(2021, 10, 17), 80.1m),
				new Rate(new DateTime(2021, 10, 18), 80.9m),
			}));

			this.StockRepository.Insert(new Stock("APPL", new List<Rate>
			{
				new Rate(new DateTime(2021, 10, 1), 142.40m),
				new Rate(new DateTime(2021, 10, 2), 141.83m),
				new Rate(new DateTime(2021, 10, 3), 141.35m),
				new Rate(new DateTime(2021, 10, 4), 141.83m),
				new Rate(new DateTime(2021, 10, 5), 141.51m),
				new Rate(new DateTime(2021, 10, 6), 140.80m),
				new Rate(new DateTime(2021, 10, 7), 140.98m),
				new Rate(new DateTime(2021, 10, 8), 140.91m),
				new Rate(new DateTime(2021, 10, 9), 140.83m),
				new Rate(new DateTime(2021, 10, 10), 141.12m),
				new Rate(new DateTime(2021, 10, 11), 141.93m),
				new Rate(new DateTime(2021, 10, 12), 142.34m),
				new Rate(new DateTime(2021, 10, 13), 142.83m),
				new Rate(new DateTime(2021, 10, 14), 141.99m),
				new Rate(new DateTime(2021, 10, 15), 142.16m),
				new Rate(new DateTime(2021, 10, 16), 142.83m),
				new Rate(new DateTime(2021, 10, 17), 142.99m),
				new Rate(new DateTime(2021, 10, 18), 143.64m),
			}));

			this.StockRepository.Insert(new Stock("AMZN", new List<Rate>
			{
				new Rate(new DateTime(2021, 10, 1), 3242.40m),
				new Rate(new DateTime(2021, 10, 2), 3243.83m),
				new Rate(new DateTime(2021, 10, 3), 3244.35m),
				new Rate(new DateTime(2021, 10, 4), 3242.83m),
				new Rate(new DateTime(2021, 10, 5), 3241.51m),
				new Rate(new DateTime(2021, 10, 6), 3240.80m),
				new Rate(new DateTime(2021, 10, 7), 3240.98m),
				new Rate(new DateTime(2021, 10, 8), 3241.91m),
				new Rate(new DateTime(2021, 10, 9), 3242.83m),
				new Rate(new DateTime(2021, 10, 10), 3241.12m),
				new Rate(new DateTime(2021, 10, 11), 3242.93m),
				new Rate(new DateTime(2021, 10, 12), 3243.83m),
				new Rate(new DateTime(2021, 10, 13), 3244.74m),
				new Rate(new DateTime(2021, 10, 14), 3244.84m),
				new Rate(new DateTime(2021, 10, 15), 3245.44m),
				new Rate(new DateTime(2021, 10, 16), 3244.83m),
				new Rate(new DateTime(2021, 10, 17), 3245.89m),
				new Rate(new DateTime(2021, 10, 18), 3246.88m),
			}));

			this.StockRepository.Insert(new Stock("GOOG", new List<Rate>
			{
				new Rate(new DateTime(2021, 10, 1), 2732.40m),
				new Rate(new DateTime(2021, 10, 2), 2733.83m),
				new Rate(new DateTime(2021, 10, 3), 2734.35m),
				new Rate(new DateTime(2021, 10, 4), 2732.83m),
				new Rate(new DateTime(2021, 10, 5), 2731.51m),
				new Rate(new DateTime(2021, 10, 6), 2730.80m),
				new Rate(new DateTime(2021, 10, 7), 2730.98m),
				new Rate(new DateTime(2021, 10, 8), 2731.91m),
				new Rate(new DateTime(2021, 10, 9), 2732.83m),
				new Rate(new DateTime(2021, 10, 10), 2731.12m),
				new Rate(new DateTime(2021, 10, 11), 2732.93m),
				new Rate(new DateTime(2021, 10, 12), 2733.83m),
				new Rate(new DateTime(2021, 10, 13), 2732.74m),
				new Rate(new DateTime(2021, 10, 14), 2732.05m),
				new Rate(new DateTime(2021, 10, 15), 2731.89m),
				new Rate(new DateTime(2021, 10, 16), 2730.88m),
				new Rate(new DateTime(2021, 10, 17), 2730.03m),
				new Rate(new DateTime(2021, 10, 18), 2729.1m),
			}));

			this.StockRepository.Insert(new Stock("MSFT", new List<Rate>
			{
				new Rate(new DateTime(2021, 10, 1), 292.40m),
				new Rate(new DateTime(2021, 10, 2), 293.83m),
				new Rate(new DateTime(2021, 10, 3), 294.35m),
				new Rate(new DateTime(2021, 10, 4), 292.83m),
				new Rate(new DateTime(2021, 10, 5), 291.51m),
				new Rate(new DateTime(2021, 10, 6), 290.80m),
				new Rate(new DateTime(2021, 10, 7), 290.98m),
				new Rate(new DateTime(2021, 10, 8), 291.91m),
				new Rate(new DateTime(2021, 10, 9), 292.83m),
				new Rate(new DateTime(2021, 10, 10), 291.12m),
				new Rate(new DateTime(2021, 10, 11), 292.93m),
				new Rate(new DateTime(2021, 10, 12), 293.83m),
				new Rate(new DateTime(2021, 10, 13), 294.77m),
				new Rate(new DateTime(2021, 10, 14), 294.88m),
				new Rate(new DateTime(2021, 10, 15), 294.89m),
				new Rate(new DateTime(2021, 10, 16), 293.62m),
				new Rate(new DateTime(2021, 10, 17), 293.02m),
				new Rate(new DateTime(2021, 10, 18), 292.24m),
			}));

			this.StockRepository.Insert(new Stock("SONY", new List<Rate>
			{
				new Rate(new DateTime(2021, 10, 1), 102.40m),
				new Rate(new DateTime(2021, 10, 2), 103.83m),
				new Rate(new DateTime(2021, 10, 3), 104.35m),
				new Rate(new DateTime(2021, 10, 4), 102.83m),
				new Rate(new DateTime(2021, 10, 5), 101.51m),
				new Rate(new DateTime(2021, 10, 6), 100.80m),
				new Rate(new DateTime(2021, 10, 7), 100.98m),
				new Rate(new DateTime(2021, 10, 8), 101.91m),
				new Rate(new DateTime(2021, 10, 9), 102.83m),
				new Rate(new DateTime(2021, 10, 10), 101.12m),
				new Rate(new DateTime(2021, 10, 11), 102.93m),
				new Rate(new DateTime(2021, 10, 12), 103.83m),
				new Rate(new DateTime(2021, 10, 13), 104.68m),
				new Rate(new DateTime(2021, 10, 14), 104.91m),
				new Rate(new DateTime(2021, 10, 15), 104.99m),
				new Rate(new DateTime(2021, 10, 16), 105.23m),
				new Rate(new DateTime(2021, 10, 17), 105.66m),
				new Rate(new DateTime(2021, 10, 18), 106.88m),
			}));

			this.StockRepository.Insert(new Stock("SC", new List<Rate>
			{
				new Rate(new DateTime(2021, 10, 1), 385.40m),
				new Rate(new DateTime(2021, 10, 2), 385.3m),
				new Rate(new DateTime(2021, 10, 3), 384.35m),
				new Rate(new DateTime(2021, 10, 4), 385.83m),
				new Rate(new DateTime(2021, 10, 5), 384.51m),
				new Rate(new DateTime(2021, 10, 6), 385.80m),
				new Rate(new DateTime(2021, 10, 7), 385.98m),
				new Rate(new DateTime(2021, 10, 8), 385.91m),
				new Rate(new DateTime(2021, 10, 9), 386.83m),
				new Rate(new DateTime(2021, 10, 10), 387.12m),
				new Rate(new DateTime(2021, 10, 11), 386.93m),
				new Rate(new DateTime(2021, 10, 12), 387.83m),
				new Rate(new DateTime(2021, 10, 13), 388.99m),
				new Rate(new DateTime(2021, 10, 14), 389.92m),
				new Rate(new DateTime(2021, 10, 15), 390.24m),
				new Rate(new DateTime(2021, 10, 16), 391.88m),
				new Rate(new DateTime(2021, 10, 17), 392.11m),
				new Rate(new DateTime(2021, 10, 18), 393.97m),
			}));
		}

		public void SetupPortfolio()
		{
			var yndx = this.StockRepository.Get("YNDX");
			var appl = this.StockRepository.Get("APPL");
			var amzn = this.StockRepository.Get("AMZN");
			var goog = this.StockRepository.Get("GOOG");
			var msft = this.StockRepository.Get("MSFT");
			var sony = this.StockRepository.Get("SONY");
			var sc = this.StockRepository.Get("SC");

			this.PortfolioRepository.Insert(new Portfolio("One", new List<Deal>
			{
				new Deal(yndx, DealType.Purchase, new DateTime(2021, 10, 7), 3),
				new Deal(appl, DealType.Purchase, new DateTime(2021, 10, 7), 5),
				new Deal(amzn, DealType.Purchase, new DateTime(2021, 10, 8), 8),
				new Deal(goog, DealType.Purchase, new DateTime(2021, 10, 9), 4),
				new Deal(msft, DealType.Purchase, new DateTime(2021, 10, 10), 7),
				new Deal(amzn, DealType.Sale, new DateTime(2021, 10, 11), 2),
			}));

			this.PortfolioRepository.Insert(new Portfolio("Two", new List<Deal>
			{
				new Deal(yndx, DealType.Purchase, new DateTime(2021, 10, 6), 8),
				new Deal(appl, DealType.Purchase, new DateTime(2021, 10, 7), 5),
				new Deal(amzn, DealType.Purchase, new DateTime(2021, 10, 8), 10),
				new Deal(goog, DealType.Purchase, new DateTime(2021, 10, 9), 4),
				new Deal(msft, DealType.Purchase, new DateTime(2021, 10, 10), 7),
				new Deal(sc, DealType.Purchase, new DateTime(2021, 10, 11), 2),
				new Deal(sony, DealType.Purchase, new DateTime(2021, 10, 11), 2),
				new Deal(amzn, DealType.Sale, new DateTime(2021, 10, 11), 2),
				new Deal(yndx, DealType.Sale, new DateTime(2021, 10, 12), 3),
			}));

			this.PortfolioRepository.Insert(new Portfolio("Three", new List<Deal>
			{
				new Deal(appl, DealType.Purchase, new DateTime(2021, 10, 7), 15),
				new Deal(amzn, DealType.Purchase, new DateTime(2021, 10, 8), 10),
				new Deal(goog, DealType.Purchase, new DateTime(2021, 10, 9), 14),
				new Deal(msft, DealType.Purchase, new DateTime(2021, 10, 10), 7),
				new Deal(yndx, DealType.Purchase, new DateTime(2021, 10, 6), 8),
				new Deal(sc, DealType.Purchase, new DateTime(2021, 10, 11), 2),
				new Deal(sony, DealType.Purchase, new DateTime(2021, 10, 11), 2),
				new Deal(appl, DealType.Sale, new DateTime(2021, 10, 11), 7),
				new Deal(yndx, DealType.Sale, new DateTime(2021, 10, 12), 8),
			}));
		}
	}
}