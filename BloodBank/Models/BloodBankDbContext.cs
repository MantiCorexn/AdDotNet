using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Models;

public partial class BloodBankDbContext : DbContext
{
    public BloodBankDbContext()
    {
    }

    public BloodBankDbContext(DbContextOptions<BloodBankDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Donation> Donations { get; set; }

    public virtual DbSet<Donor> Donors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-R4H132T\\SQLEXPRESS;Database=BloodBankDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__Donation__C5082EFB5F47161F");

            entity.ToTable("Donation");

            entity.Property(e => e.CampName).HasMaxLength(100);

            entity.HasOne(d => d.Donor).WithMany(p => p.Donations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Donation__DonorI__4CA06362");
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.DonorId).HasName("PK__Donor__052E3F78A8B198C5");

            entity.ToTable("Donor");

            entity.Property(e => e.BloodGroup).HasMaxLength(5);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.ContactNo).HasMaxLength(20);
            entity.Property(e => e.FullName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
