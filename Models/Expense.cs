using System;

namespace Expense.Models
{
	public class Expense
	{
		public int ExpenseId { get; set; }
		public double Amount { get; set; }
		public DateTime Date { get; set; }
		public string Note { get; set; }
		
		public int UserId { get; set; }
		public User User { get; set; }
	}	
}