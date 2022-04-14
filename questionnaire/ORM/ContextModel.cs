using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace questionnaire.ORM
{
    public partial class ContextModel : DbContext
    {
        public ContextModel()
            : base("name=ContextModel1")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Content> Contents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Account1)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.PWD)
                .IsUnicode(false);
        }
    }
}
