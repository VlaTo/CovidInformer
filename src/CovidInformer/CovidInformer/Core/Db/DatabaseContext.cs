using CovidInformer.Core.Db.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;

namespace CovidInformer.Core.Db
{
    public sealed class DatabaseContext : DbContext
    {
        private const string Filename = "coviddata.db";

        public DbSet<Country> Countries
        {
            get;
            set;
        }
        
        public DbSet<Update> Updates
        {
            get;
            set;
        }
        
        public DbSet<Counter> Counters
        {
            get;
            set;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var connectionBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = Path.Combine(basePath, Filename),
                Cache = SqliteCacheMode.Private,
                Mode = SqliteOpenMode.ReadWriteCreate
            };

            Debug.WriteLine($"Database path: \"{connectionBuilder.DataSource}\"");

            optionsBuilder.UseSqlite(connectionBuilder.ToString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Country>()
                .HasIndex(entity => entity.Id)
                .IsUnique(true);
            modelBuilder
                .Entity<Country>()
                .Property(entity => entity.DisplayName)
                .IsRequired(true);
            modelBuilder
                .Entity<Country>()
                .HasIndex(entity => entity.DisplayName)
                .IsUnique(true);

            modelBuilder
                .Entity<Update>()
                .Property(entity => entity.Updated)
                .IsRequired(true);
            modelBuilder
                .Entity<Update>()
                .HasIndex(entity => entity.Updated)
                .IsUnique(false);

            modelBuilder
                .Entity<Counter>()
                .HasOne(entity => entity.Country)
                .WithMany(entity => entity.Counters);
            modelBuilder
                .Entity<Counter>()
                .HasOne(entity => entity.Update)
                .WithMany(entity => entity.Counters);
        }
    }
}