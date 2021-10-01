using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Accounts.Models
{
    public partial class KDGIDENTITYContext : DbContext
    {

        public KDGIDENTITYContext()
        {
        }

        public KDGIDENTITYContext(DbContextOptions<KDGIDENTITYContext> options) : base(options) { }

        public virtual DbSet<AccountType> AccountType { get; set; }
        public virtual DbSet<AccountTypeRole> AccountTypeRole { get; set; }
        public virtual DbSet<Logging> Loggings { get; set; }
        public virtual DbSet<Kdgaccount> Kdgaccount { get; set; }
        public virtual DbSet<vwKDGAccountLU> vwKDGAccountLU { get; set; }
        public virtual DbSet<vwKDGEmailLU> vwKDGEmailLU { get; set; }
        public virtual DbSet<Kdggroup> Kdggroup { get; set; }
        public virtual DbSet<KdgrightsGroup> KdgrightsGroup { get; set; }
        public virtual DbSet<KdgrightsGroupPropertie> KdgrightsGroupPropertie { get; set; }
        public virtual DbSet<PeopleLogging> PeopleLogging { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountExt).HasMaxLength(10);

                entity.Property(e => e.EmailExt).HasMaxLength(10);

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.DeleteAfter);

                entity.Property(e => e.CreateAllowed);
            });

            modelBuilder.Entity<AccountTypeRole>(entity =>
            {
                entity.ToTable("AccountTypeRole");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountType)
                    .IsRequired()
                    .HasColumnName("AccountType");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("Role");

                entity.HasOne(d => d.AccountTypeNavigation)
                    .WithMany(p => p.AccountTypeRole)
                    .HasForeignKey(d => d.AccountType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountTypeRole_AccountType");
            });

            modelBuilder.Entity<Kdgaccount>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.ToTable("KDGAccount");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Aanvrager)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.InitialPassword)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MifareSerial).HasMaxLength(20);

                entity.Property(e => e.Office).HasMaxLength(50);

                entity.Property(e => e.Opmerking).HasColumnType("text");

                entity.Property(e => e.PrivateMail).HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Telephone).HasMaxLength(20);

                entity.Property(e => e.Ticketnr)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.AccountTypeNavigation)
                    .WithMany(p => p.Kdgaccount)
                    .HasForeignKey(d => d.AccountType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KDGAccount__AccountType");

                entity.HasOne(d => d.RightsGroupNavigation)
                    .WithMany(p => p.Kdgaccount)
                    .HasForeignKey(d => d.RightsGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KDGAccount_KDGRightsGroup");
            });

            modelBuilder.Entity<vwKDGAccountLU>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<vwKDGEmailLU>(entity =>
            {
                entity.HasKey(e => e.Id);

            });

            modelBuilder.Entity<Kdggroup>(entity =>
            {
                entity.ToTable("KDGGroup");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.RGId)
                    .IsRequired()
                    .HasColumnName("RGId");

                entity.HasOne(d => d.KdgRightsGroupNavigation)
                    .WithMany(p => p.Kdggroup)
                    .HasForeignKey(d => d.RGId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KDGGroup_KDGRightsGroup");
            });

            modelBuilder.Entity<KdgrightsGroup>(entity =>
            {
                entity.ToTable("KDGRightsGroup");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MaxEndDate)
                .HasDefaultValue(52)
                .IsRequired();

                entity.HasOne(d => d.AccountTypeNavigation)
                    .WithMany(p => p.KdgrightsGroup)
                    .HasForeignKey(d => d.AccountType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KDGRightsGroup_AccountType");

            });

            modelBuilder.Entity<KdgrightsGroupPropertie>(entity =>
            {
                entity.ToTable("KDGRightsGroupPropertie");

                entity.Property(e => e.RGId)
                    .IsRequired()
                    .HasColumnName("RGId");

                entity.Property(e => e.Field)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.KdgRightsGroupNavigation)
                    .WithMany(p => p.Kdgrightsgrouppropertie)
                    .HasForeignKey(d => d.RGId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KDGRightsGroupPropertie_KDGRightsGroup");
                
            });

            modelBuilder.Entity<PeopleLogging>(entity =>
            {
                entity.ToTable("PeopleLogging");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.UniqueID)
                    .HasMaxLength(100);

                entity.Property(e => e.App)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Event)
                    .HasMaxLength(100);

                entity.Property(e => e.Logdate)
                    .HasColumnType("datetime");

            });

            modelBuilder.Entity<Logging>(entity =>
            {
                entity.ToTable("Logging");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.App)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Event)
                    .HasMaxLength(100);

                entity.Property(e => e.Logdate)
                    .HasColumnType("datetime");

            });
        }
    }
}
