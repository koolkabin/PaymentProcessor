using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PaymentProcessor.Model
{
    public class PaymentDBContext : DbContext
    {

        public PaymentDBContext(DbContextOptions<PaymentDBContext> options) : base(options) { }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

}

