using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;

namespace Expense.Controllers
{
	public class UserData {
		public string Email { get; set; }
		public string Password { get; set; }
	}
	
	[RouteAttribute("api/[controller]")]
	public class UsersController : Controller
	{
		private ILogger<UsersController> logger;
		
		public UsersController(ILogger<UsersController> logger)
		{
			this.logger = logger;
		} 
		
        [HttpPut()]
		[ProducesAttribute("application/json", Type = typeof(UserData))]
        public string Signup([FromBody]UserData user)
        {
			this.logger.LogInformation("SignUp");
			
			if (null != user) {
				this.logger.LogInformation(user.Email);
				return "Welcome to Expense Tracker";
			}
			
			return "Try again";
        }

        [HttpPost()]
		[ProducesAttribute("application/json", Type = typeof(UserData))]
        public string Login([FromBody]UserData user)
        {
			this.logger.LogInformation("Login");
						
			if (null != user) {
				this.logger.LogInformation(user.Email);
				this.Response.StatusCode = 200;
				return user.Email;
			}
			
			this.Response.StatusCode = 540;
			return "Wrong email or password.";
        }
	}
}