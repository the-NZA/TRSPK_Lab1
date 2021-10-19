using System.Collections.Generic;
using BusinessLayer;
using DbLayer.Models;

namespace UILayerWebsite.Models
{
	public class SecondModel 
	{
		public List<Stock> Stocks{ get; set; }
		public Dictionary<string, RateWithPercent> Result { get; set; }
		public bool IsRes { get; set; }
	}
}