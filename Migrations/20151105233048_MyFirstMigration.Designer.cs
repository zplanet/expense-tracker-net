using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Expense.Models;

namespace Expense.Migrations
{
    [DbContext(typeof(ExpenseContext))]
    [Migration("20151105233048_MyFirstMigration")]
    partial class MyFirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta8-15964");

            modelBuilder.Entity("Expense.Models.Expense", b =>
                {
                    b.Property<int>("ExpenseId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Note");

                    b.Property<int>("UserId");

                    b.HasKey("ExpenseId");
                });

            modelBuilder.Entity("Expense.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Password");

                    b.HasKey("UserId");
                });

            modelBuilder.Entity("Expense.Models.Expense", b =>
                {
                    b.HasOne("Expense.Models.User")
                        .WithMany()
                        .ForeignKey("UserId");
                });
        }
    }
}
