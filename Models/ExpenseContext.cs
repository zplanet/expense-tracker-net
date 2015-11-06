using Microsoft.Data.Entity;
using System.Collections.Generic;

namespace Expense.Models
{
	public class ExpenseContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Expense> Expenses { get; set; }
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
		}
	}	
}