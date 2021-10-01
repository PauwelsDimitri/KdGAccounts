using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Accounts.Models
{
    public partial class KDGVIEWSContext : DbContext
    {

        public KDGVIEWSContext()
        {
        }

        public KDGVIEWSContext(DbContextOptions<KDGVIEWSContext> options) : base(options) { }

        public virtual DbSet<vwPeopleLogging> vwPeopleLogging { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vwPeopleLogging>(entity =>
            {
                entity.HasKey(e => e.Logdate);
            });
        }
    }
}
