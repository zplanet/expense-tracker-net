using System.Collections.Generic;

namespace Expense.Models
{
	public class User
	{
		public int UserId { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		
		public List<Expense> Expenses { get; set; }
	}	
}