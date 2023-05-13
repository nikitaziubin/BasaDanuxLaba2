using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Laba2;

public partial class BasaDanuxLaba2Context : DbContext
{
    public BasaDanuxLaba2Context()
    {
    }

    public BasaDanuxLaba2Context(DbContextOptions<BasaDanuxLaba2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Participate> Participates { get; set; }

    public virtual DbSet<Stadium> Stadiums { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-J5R1H9E6; Database=BasaDanuxLaba2; Trusted_Connection=True; Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Club>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("create_date");
            entity.Property(e => e.ManagerId).HasColumnName("managerID");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("name");

            entity.HasOne(d => d.Manager).WithMany(p => p.Clubs)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clubs_Managers");
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DivisoinOrLeague).HasColumnName("divisoin_or_league");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("name");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.DivisionId).HasColumnName("DivisionID");
            entity.Property(e => e.StadiumId).HasColumnName("StadiumID");

            entity.HasOne(d => d.Division).WithMany(p => p.Matches)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matches_Divisions");

            entity.HasOne(d => d.Stadium).WithMany(p => p.Matches)
                .HasForeignKey(d => d.StadiumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matches_Stadiums");
        });

        modelBuilder.Entity<Participate>(entity =>
        {
            entity.ToTable("Participate");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BallOwnershipTime).HasColumnName("ball_ownership_time");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.Offsides).HasColumnName("offsides");
            entity.Property(e => e.Passes).HasColumnName("passes");
            entity.Property(e => e.RedCards).HasColumnName("red_cards");
            entity.Property(e => e.ScoredGoals).HasColumnName("Scored_goals");
            entity.Property(e => e.Shots).HasColumnName("shots");
            entity.Property(e => e.ShotsOnTarget).HasColumnName("shots_on_target");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.TeamRole)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("team_role");
            entity.Property(e => e.YellowCards).HasColumnName("yellow_cards");

            entity.HasOne(d => d.Match).WithMany(p => p.Participates)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Participate_Matches");

            entity.HasOne(d => d.Team).WithMany(p => p.Participates)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Participate_Teams");
        });

        modelBuilder.Entity<Stadium>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adress)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("adress");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.MaxCapacity).HasColumnName("max_capacity");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("name");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("name");

            entity.HasOne(d => d.Club).WithMany(p => p.Teams)
                .HasForeignKey(d => d.ClubId)
                .HasConstraintName("FK_Teams_Clubs");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
