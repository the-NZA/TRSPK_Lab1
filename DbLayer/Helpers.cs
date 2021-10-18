using System;
using System.Collections.Generic;
using DbLayer.Models;

namespace DbLayer
{
	public static class Helpers
	{
		public static readonly string DefaultStocksDbPath =
			"/Users/romankozlov/RiderProjects/Lab1/DbLayer/Stocks";

		public static readonly string DefaultPortfolioDbPath =
			"/Users/romankozlov/RiderProjects/Lab1/DbLayer/Portfolios";

		public static void Display(this byte[] a)
		{
			Console.Write("len: {0} => [ ", a.Length);
			foreach (byte b in a)
			{
				Console.Write("{0}, ", b);
			}

			Console.Write("]\n");
		}

		public static List<Stock> ToStocks(this List<string> strList)
		{
			List<Stock> stockList = new List<Stock>(strList.Count);

			foreach (string str in strList)
			{
				stockList.Add(Stock.ParseFormat(str));
			}

			return stockList;
		}

		public static List<Portfolio> ToPortfolio(this List<string> strList)
		{
			List<Portfolio> portfolios = new List<Portfolio>(strList.Count);

			foreach (string str in strList)
			{
				portfolios.Add(Portfolio.ParseFormat(str));
			}

			return portfolios;
		}
	}
}