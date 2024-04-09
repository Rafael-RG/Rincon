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
                var count = await databaseContext.UpsertRange<ProductStock>(stock).RunAsync();
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
        public async Task<List<Product>> LoadItemsAsync()
        {
            using (var databaseContext = new DatabaseContext())
            {
                var items = await databaseContext.Product.ToListAsync().ConfigureAwait(false);
                return items;
            }
        }
    }
}
