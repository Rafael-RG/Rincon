using Rincon.Common;
using Microsoft.EntityFrameworkCore;
using Rincon.Models;

namespace Rincon.DataAccess
{
	public class LocalDataBaseContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public LocalDataBaseContext()
		{
            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();
        }

        /// <summary>
        /// Configure the database
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Constants.LocalDatabaseName);
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
}

