using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Server.Models;

public partial class DatingappContext : DbContext
{
    public DatingappContext()
    {
    }

    public DatingappContext(DbContextOptions<DatingappContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Name=DatingApp");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FavouriteLanguage).HasColumnName("favourite_language");
            entity.Property(e => e.Likes).HasColumnName("likes");
            entity.Property(e => e.Matches).HasColumnName("matches");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.PhotoUrl).HasColumnName("photo_url");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
