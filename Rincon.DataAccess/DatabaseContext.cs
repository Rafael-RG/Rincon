using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using Rincon.Common;
using Rincon.Models;

namespace Rincon.DataAccess
{
    /// <summary>
    /// SQLlite db context
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ProductStock> ProductStock { get; set; }


        /// <summary>   |
        /// Initializes sqlite
        /// </summary>
        public DatabaseContext()
        {
            SQLitePCL.Batteries_V2.Init();

            this.Database.EnsureCreated();
        }




        /// <summary>
        /// Configure the database
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Constants.DatabaseName);
            //optionsBuilder.UseSqlite($"Filename={databasePath}");

            optionsBuilder.UseSqlServer(Constants.ConnectionString);
        }
    }
}
