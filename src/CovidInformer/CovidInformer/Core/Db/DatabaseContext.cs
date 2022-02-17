using System;
using System.IO;
using CovidInformer.Core.Db.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

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
            string dataSource;

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                {
                    var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    
                    dataSource = Path.Combine(basePath, Filename);

                    break;
                }

                case Device.UWP:
                {
                    var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    
                    dataSource = Path.Combine(basePath, Filename);
                        
                    break;
                }

                default:
                {
                    throw new PlatformNotSupportedException();
                }
            }

            var connectionBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = dataSource,
                Cache = SqliteCacheMode.Private,
                Mode = SqliteOpenMode.ReadWriteCreate
            };

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