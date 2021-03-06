using System;
using System.Collections.Generic;
using DbLayer.Models;

namespace UILayerWebsite.Models
{
	public class FirstAssignmentModel
	{
		public List<Portfolio> Portfolios { get; set; }
		public string Owner { get; set; }
		public DateTime Date { get; set; }
		public decimal Result { get; set; }
		public bool IsRes { get; set; }
		public bool IsError { get; set; }
		public string ErrMessage { get; set; }
	}
}