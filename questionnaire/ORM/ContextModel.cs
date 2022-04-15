using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace questionnaire.ORM
{
    public partial class ContextModel : DbContext
    {
        public ContextModel()
            : base("name=ContextModel4")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountCheck> AccountChecks { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<CQ> CQs { get; set; }
        public virtual DbSet<QuesDetail> QuesDetails { get; set; }
        public virtual DbSet<QuesType> QuesTypes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Account1)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.PWD)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.AccountChecks)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Content>()
                .HasMany(e => e.AccountChecks)
                .WithRequired(e => e.Content)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Content>()
                .HasMany(e => e.QuesDetails)
                .WithRequired(e => e.Content)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuesType>()
                .Property(e => e.QuesType1)
                .IsFixedLength();

            modelBuilder.Entity<QuesType>()
                .HasMany(e => e.CQs)
                .WithRequired(e => e.QuesType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuesType>()
                .HasMany(e => e.QuesDetails)
                .WithRequired(e => e.QuesType)
                .WillCascadeOnDelete(false);
        }
    }
}
