using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;

namespace Expense.Controllers
{
	public class ExpenseData
	{
		public int Amount { get; set; }
		public DateTime Date { get; set; }
		public string Note { get; set; }
	}
	
	[RouteAttribute("api/[controller]")]
	public class ExpensesController : Controller
	{
		private ILogger<ExpensesController> logger;
		
		public ExpensesController(ILogger<ExpensesController> logger)
		{
			this.logger = logger;
		}
		
		[HttpPut]
		[ProducesAttribute("application/json", Type = typeof(ExpenseData))]
		public string Add([FromBodyAttribute] ExpenseData data)
		{
			if (null == data) {
				this.Response.StatusCode = 502;
				return "Please, Check your input.";
			}
			
			this.logger.LogInformation(data.Date.ToLocalTime().ToString());
			this.logger.LogInformation(data.Amount.ToString());
			this.logger.LogInformation(data.Note);
			
			this.Response.StatusCode = 200;
			return "Data is added successfully.";
		}
		
		[HttpGet]
		[RouteAttribute("Report")]
		public JsonResult GetReportData()
		{
			this.logger.LogInformation("GetReportData");
			
			var expenses = new List<ExpenseData>() {
				new ExpenseData() { Amount = 10, Date = new DateTime(2015, 1, 10), Note = "lunch" },
				new ExpenseData() { Amount = 15, Date = new DateTime(2015, 2, 12), Note = "lunch" },
				new ExpenseData() { Amount = 20, Date = new DateTime(2015, 3, 15), Note = "lunch" },
				new ExpenseData() { Amount = 10, Date = new DateTime(2015, 4, 22), Note = "lunch" },
				new ExpenseData() { Amount = 15, Date = new DateTime(2015, 5, 8), Note = "lunch" },
				new ExpenseData() { Amount = 20, Date = new DateTime(2015, 6, 28), Note = "lunch" },
				new ExpenseData() { Amount = 10, Date = new DateTime(2015, 7, 7), Note = "lunch" },
				new ExpenseData() { Amount = 10, Date = new DateTime(2015, 8, 3), Note = "lunch" },
				new ExpenseData() { Amount = 20, Date = new DateTime(2015, 9, 21), Note = "lunch" }
			};
			
			return Json(expenses.Select(x => new { amount = x.Amount, date = x.Date.ToString("yyyy-MM-dd"), note = x.Note }));
		}

		[HttpGet]
		[RouteAttribute("Graph")]
		public JsonResult GetGraphData()
		{
			this.logger.LogInformation("GetGraphData");
			
			var monthlyData = new [] {
				new object[] { "2015-01", 550 },
				new object[] { "2015-02", 700 },
				new object[] { "2015-03", 900 },
				new object[] { "2015-04", 500 },
				new object[] { "2015-05", 850 },
				new object[] { "2015-06", 1200 },
				new object[] { "2015-07", 1500 },
				new object[] { "2015-08", 750 },
				new object[] { "2015-09", 670 },
				new object[] { "2015-10", 880 },
				new object[] { "2015-11", 740 },
				new object[] { "2015-12", 560 },
			};
			
			return Json(monthlyData);
		}
	}
}