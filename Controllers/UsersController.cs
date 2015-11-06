using System;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using Expense.Models;
using System.Linq;

namespace Expense.Controllers
{	
	[RouteAttribute("api/[controller]")]
	public class UsersController : Controller
	{
		private ExpenseContext db;
		
		private ILogger<UsersController> logger;
		
		public UsersController(ExpenseContext db, ILogger<UsersController> logger)
		{
			this.db = db;
			this.logger = logger;
		}
		
        [HttpPut()]
		[ProducesAttribute("application/json", Type = typeof(User))]
        public string Signup([FromBody]User user)
        {
			this.logger.LogInformation("SignUp");
			
			if (null == user)
			{				
				this.Response.StatusCode = 500;
				return "Try again";
			}
						
			this.logger.LogInformation(user.Email);
			this.logger.LogInformation(user.Password);
			
			user.Password = Common.Util.ToSha256(user.Password);
			
			this.logger.LogInformation(user.Password);
			
			this.db.Users.Add(user);
			this.db.SaveChanges();
			
			this.Response.StatusCode = 200;
			return "Welcome to Expense Tracker";
        }

        [HttpPost()]
		[ProducesAttribute("application/json", Type = typeof(User))]
        public string Login([FromBody]User user)
        {
			this.logger.LogInformation("Login");
									
			if (null != user) {
				
				this.logger.LogInformation(user.Email);
				this.logger.LogInformation(user.Password);

				User foundUser = this.db.Users.FirstOrDefault(u => 
					u.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase) && 
					u.Password.Equals(user.Password, StringComparison.CurrentCultureIgnoreCase));

				if (null != foundUser)
				{
					this.Response.StatusCode = 200;
					return user.Email;
				}
			}
			
			this.Response.StatusCode = 540;
			return "Wrong email or password.";
        }
	}
}