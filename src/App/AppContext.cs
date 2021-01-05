using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System;

namespace App
{
	public class AppContext : DbContext
	{
		private readonly string _connectionString = "DataSource=:memory:";
		private readonly SqliteConnection _connection;


		public DbSet<Resource> Resources { get; set; }

		public AppContext()
		{
			_connection = new SqliteConnection(_connectionString);
			_connection.Open();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite(_connection, x =>
			{
				x.UseNetTopologySuite();
			});

			var factory = LoggerFactory.Create(x =>
			{
				x.AddConsole();
				x.SetMinimumLevel(LogLevel.Debug);
			});

			optionsBuilder.UseLoggerFactory(factory);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Resource>().HasKey(x => x.Id);
			modelBuilder.Entity<Resource>().Property(x => x.Id).ValueGeneratedNever();

			// Set 'geography' manually. When using SqlServer, the migration is created is created with this type.
			modelBuilder.Entity<Resource>()
				.Property(x => x.Location).HasColumnType("geography").HasColumnName("GpsMapLocation");
		}
	}

	public class Resource
	{
		public Guid Id { get; set; }

		public Point Location { get; set; }
	}
}
