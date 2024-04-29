using Microsoft.EntityFrameworkCore;
using Rincon.Common.Interfaces;
using Rincon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rincon.DataAccess
{ /// <summary>
  /// Data access service
  /// </summary>
    public class DataService : IDataService
    {

        /// <summary>
        /// Initialization
        /// </summary>
        public DataService()
        {
        }


        ///<inheritdoc/>
        public async Task<bool> MustSynchronizeAsync()
        {
            var count = 0;
            using (var databaseContext = new DatabaseContext())
            {
                count = await databaseContext.Product.CountAsync();
            }
            return count == 0;
        }


        ///<inheritdoc/>
        public async Task<int> SaveItemsAsync<T>(IEnumerable<T> items, string tableName) where T : class
        {
            using (var databaseContext = new DatabaseContext())
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    await databaseContext.Database.ExecuteSqlRawAsync($"DELETE FROM {tableName}");
                }
                await databaseContext.Set<T>().AddRangeAsync(items);
                var itemsCount = await databaseContext.SaveChangesAsync().ConfigureAwait(false);
                return itemsCount;
            }
        }


        ///<inheritdoc/>
        public async Task<int> InsertOrUpdateItemsAsync<T>(T item) where T : class
        {
            using (var databaseContext = new DatabaseContext())
            {
                var itemsCount = await databaseContext.UpsertRange<T>(item).RunAsync();
                return itemsCount;
            }
        }

        ///<inheritdoc/>
        public async Task<int> InsertOrUpdateStockAsync(List<ProductStock> stock)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var count = 0;
                foreach(var item in stock)
                {
                    count += await databaseContext.UpsertRange<ProductStock>(item).RunAsync();
                }
                
                return count;
            }
        }


        ///<inheritdoc/>
        public async Task<int> DeleteItemAsync<T>(T item) where T : class
        {
            using (var databaseContext = new DatabaseContext())
            {
                databaseContext.Remove(item);
                var itemsCount = await databaseContext.SaveChangesAsync().ConfigureAwait(false);
                return itemsCount;
            }
        }


        ///<inheritdoc/>
        public async Task<List<Product>> LoadProductsAsync()
        {
            using (var databaseContext = new DatabaseContext())
            {
                var items = await databaseContext.Product.ToListAsync().ConfigureAwait(false);
                return items;
            }
        }

        ///<inheritdoc/>
        public async Task<List<ProductStock>> LoadStockAsync()
        {
            using (var databaseContext = new DatabaseContext())
            {
                var items = await databaseContext.ProductStock.ToListAsync().ConfigureAwait(false);
                return items;
            }
        }

        ///<inheritdoc/>
        public async Task<User> LoadUserAsync(string userName, string password)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = await databaseContext.User.Where(x => x.Name == userName && x.Password == password).FirstOrDefaultAsync();
                return user;
            }
        }

        ///<inheritdoc/>
        public async Task<User> LoadLocalUserAsync()
        {
            using (var databaseContext = new LocalDataBaseContext())
            {
                var user = await databaseContext.User.FirstOrDefaultAsync();
                return user;
            }
        }

        ///<inheritdoc/>
        public async Task<bool> DeleteLocalUserAsync(User user)
        {
            using (var databaseContext = new LocalDataBaseContext())
            {
                databaseContext.Remove(user);
                var itemsCount = await databaseContext.SaveChangesAsync().ConfigureAwait(false);
                return itemsCount > 0;
            }
        }

        ///<inheritdoc/>
        ///<inheritdoc/>
        public async Task<bool> SaveLocalUserAsync(User user)
        {
            using (var databaseContext = new LocalDataBaseContext())
            {
                var itemsCount = await databaseContext.UpsertRange(user).RunAsync();
                return itemsCount > 0;
            }
        }

        ///<inheritdoc/>
        public async Task<User> LoadRecoveryUserAsync(string userName)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = await databaseContext.User.Where(x => x.Name == userName).FirstOrDefaultAsync();
                return user;
            }
        }
    }
}
