using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace project_signalr_api.Models.Entities;

public partial class TicketsContext : DbContext
{
    public TicketsContext()
    {
    }

    public TicketsContext(DbContextOptions<TicketsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administrador { get; set; }

    public virtual DbSet<Caja> Caja { get; set; }

    public virtual DbSet<Historial> Historial { get; set; }

    public virtual DbSet<Turno> Turno { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=labsystec.net;user=labsyste_ssc;password=S1gu13nt31;database=tickets", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.7-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("administrador");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Contraseña).HasMaxLength(50);
            entity.Property(e => e.NombreUsuario).HasMaxLength(100);
        });

        modelBuilder.Entity<Caja>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("caja");

            entity.HasIndex(e => e.IdTurnoActual, "fkcaja_turno");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdTurnoActual).HasColumnType("int(11)");
            entity.Property(e => e.NumeroCaja).HasColumnType("int(11)");

            entity.HasOne(d => d.IdTurnoActualNavigation).WithMany(p => p.Caja)
                .HasForeignKey(d => d.IdTurnoActual)
                .HasConstraintName("fkcaja_turno");
        });

        modelBuilder.Entity<Historial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("historial");

            entity.HasIndex(e => e.IdCaja, "fkhistorial_caja");

            entity.HasIndex(e => e.IdTurno, "fkhistorial_turno");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.FechaAtencion).HasColumnType("datetime");
            entity.Property(e => e.IdCaja).HasColumnType("int(11)");
            entity.Property(e => e.IdTurno).HasColumnType("int(11)");

            entity.HasOne(d => d.IdCajaNavigation).WithMany(p => p.Historial)
                .HasForeignKey(d => d.IdCaja)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkhistorial_caja");

            entity.HasOne(d => d.IdTurnoNavigation).WithMany(p => p.Historial)
                .HasForeignKey(d => d.IdTurno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkhistorial_turno");
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("turno");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Estado).HasColumnType("enum('Pendiente','Atendiendo','Atendido')");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
