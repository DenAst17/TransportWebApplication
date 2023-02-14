using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TransportWebApplication.Models;

public partial class TransportContext : DbContext
{
    public TransportContext()
    {
    }

    public TransportContext(DbContextOptions<TransportContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auto> Autos { get; set; }

    public virtual DbSet<AutoOwner> AutoOwners { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-97Q88N1;Database=transport; Trusted_Connection=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auto>(entity =>
        {
            entity.ToTable("Auto");

            entity.Property(e => e.Color).HasMaxLength(255);
            entity.Property(e => e.RegisterCode).HasMaxLength(255);
            entity.Property(e => e.Vin)
                .HasMaxLength(255)
                .HasColumnName("VIN");

            entity.HasOne(d => d.Model).WithMany(p => p.Autos)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auto_Models");
        });

        modelBuilder.Entity<AutoOwner>(entity =>
        {
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.IncidentsInfo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Auto).WithMany(p => p.AutoOwners)
                .HasForeignKey(d => d.AutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AutoOwners_Auto");

            entity.HasOne(d => d.Owner).WithMany(p => p.AutoOwners)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AutoOwners_AutoOwners");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Model");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Owner");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Surname).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
