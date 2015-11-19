using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Expense.Models;

namespace Expense.Controllers
{
	[RouteAttribute("api/[controller]")]
	public class ExpensesController : Controller
	{
		private ExpenseContext db;
		private ILogger<ExpensesController> logger;
		
		public ExpensesController(ExpenseContext db, ILogger<ExpensesController> logger)
		{
			this.db = db;
			this.logger = logger;
		}
		
		[HttpPut]
		[ProducesAttribute("application/json", Type = typeof(Models.Expense))]
		public string Add([FromBodyAttribute] Models.Expense data)
		{
			if (null == data) {
				this.Response.StatusCode = 502;
				return "Please, Check your input.";
			}
			
			var email = this.HttpContext.Request.Headers["Authorization"];
			
			var user = this.db.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
			
			if (null == user)
			{
				this.Response.StatusCode = 401;
				return "Unauthorized";
			}
			
			this.logger.LogInformation(data.Date.ToLocalTime().ToString());
			this.logger.LogInformation(data.Amount.ToString());
			this.logger.LogInformation(data.Note);
			
			data.UserId = user.UserId;
			
			this.db.Expenses.Add(data);
			this.db.SaveChanges();
			
			this.Response.StatusCode = 200;
			return "Data is added successfully.";
		}
		
		[HttpGet]
		[RouteAttribute("Report")]
		public JsonResult GetReportData()
		{
			this.logger.LogInformation("GetReportData");
			var expenses = from x in this.db.Expenses select x;			
			return Json(expenses.Select(x => new { amount = x.Amount, date = x.Date.ToString("yyyy-MM-dd"), note = x.Note }));
		}

		[HttpGet]
		[RouteAttribute("Graph")]
		public JsonResult GetGraphData()
		{
			this.logger.LogInformation("GetGraphData");
						
			var monthlyData = from x in this.db.Expenses
								group x by x.Date.ToString("yyyy-MM") into monthly
								orderby monthly.Key
								select new object[] { monthly.Key, monthly.Sum(d => d.Amount) };
											
			return Json(monthlyData);
		}
	}
}