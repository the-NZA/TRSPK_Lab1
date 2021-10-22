using System;
using System.Collections.Generic;
using DbLayer.Models;

namespace UILayerWebsite.Models
{
	public class ThirdAssignmentModel 
	{
		public List<Portfolio> Portfolios { get; set; }
		public string Owner { get; set; }
		public DateTime DateFrom { get; set; }
		public DateTime DateTo { get; set; }
		public (decimal pTurnover, decimal sTurnover) Result { get; set; }
		public bool IsRes { get; set; }
		public bool IsError { get; set; }
		public string ErrMessage { get; set; }
	}
}