using System.Collections.Generic;
using DbLayer.Models;

namespace UILayerWebsite.Models
{
	public class PortfoliosGetOneModel
	{
		public List<Portfolio> Portfolios { get; set; }
		public int Idx { get; set; }
		public bool IsError { get; set; }
		public bool IsResult { get; set; }
		public string ErrMessage { get; set; }
	}

	public class PortfoliosCreateModel
	{
		public List<Stock> Stocks { get; set; }
		public bool IsResult { get; set; }
		public bool IsError { get; set; }
		public string ErrMessage { get; set; }
	}

	public class PortfoliosDeleteModel
	{
		public List<Portfolio> Portfolios { get; set; }
		public bool IsResult { get; set; }
		public bool IsError { get; set; }
		public string ErrMessage { get; set; }
	}

	public class PortfoliosEditModel
	{
		public List<Portfolio> Portfolios { get; set; }
		public List<Stock> Stocks { get; set; }
		public bool IsResult { get; set; }
		public bool IsError { get; set; }
		public string ErrMessage { get; set; }
	}
}