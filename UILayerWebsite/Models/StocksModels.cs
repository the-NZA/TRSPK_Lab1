using System.Collections.Generic;
using DbLayer.Models;

namespace UILayerWebsite.Models
{
	public class StocksGetOneModel
	{
		public List<Stock> Stocks { get; set; }
		public int Idx { get; set; }
		public bool IsError { get; set; }
		public bool IsResult { get; set; }
		public string ErrMessage { get; set; }
	}

	public class StocksCreateModel
	{
		public bool IsResult { get; set; }
		public bool IsError { get; set; }
		public string ErrMessage { get; set; }
	}

	public class StocksDeleteModel
	{
		public List<Stock> Stocks { get; set; }
		public bool IsResult { get; set; }
		public bool IsError { get; set; }
		public string ErrMessage { get; set; }
	}

	public class StocksEditModel
	{
		public List<Stock> Stocks { get; set; }
		public bool IsResult { get; set; }
		public bool IsError { get; set; }
		public string ErrMessage { get; set; }
	}
}