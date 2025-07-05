using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MoviesWebApi.Models;

public partial class MyMoviesContext : DbContext
{
    public MyMoviesContext()
    {
    }

    public MyMoviesContext(DbContextOptions<MyMoviesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=MyMovies;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("genres");

            entity.Property(e => e.GenreId)
                .ValueGeneratedNever()
                .HasColumnName("genreId");
            entity.Property(e => e.GenreName)
                .HasMaxLength(50)
                .HasColumnName("genreName");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("movies");

            entity.Property(e => e.MovieId)
                .ValueGeneratedNever()
                .HasColumnName("movieId");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasMany(d => d.Genres).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("FK_GENRES_MOVIE"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK_MOVIE_GENRES"),
                    j =>
                    {
                        j.HasKey("MovieId", "GenreId");
                        j.ToTable("movieGenres");
                        j.IndexerProperty<int>("MovieId").HasColumnName("movieId");
                        j.IndexerProperty<int>("GenreId").HasColumnName("genreId");
                    });
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ratings");

            entity.Property(e => e.MovieId).HasColumnName("movieId");
            entity.Property(e => e.Rating1).HasColumnName("rating");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Movie).WithMany()
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK_MOVIES_RATINGS");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_USERS_RATINGS");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
