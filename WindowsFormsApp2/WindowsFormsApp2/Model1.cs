namespace WindowsFormsApp2
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>()
                .HasMany(e => e.Branches)
                .WithOptional(e => e.Bank)
                .HasForeignKey(e => e.BankId_ID);
        }
    }
}
