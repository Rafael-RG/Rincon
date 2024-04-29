using Rincon.Models;

namespace Rincon.Common.Interfaces
{

    /// <summary>
    /// Data access service interface
    /// </summary>
    public interface IDataService
    {

        /// <summary>
        /// Checks if the synchronization is mandatory
        /// </summary>
        /// <returns></returns>
        Task<bool> MustSynchronizeAsync();
     

        /// <summary>
        /// Save a collections of items
        /// </summary>
        Task<int> SaveItemsAsync<T>(IEnumerable<T> items, string tableName) where T : class;

        /// <summary>
        /// Save a collections of items 
        /// </summary>
        Task<int> InsertOrUpdateItemsAsync<T>(T item) where T : class;

        /// <summary>
        /// Save a collections of products stock
        /// </summary>
        Task<int> InsertOrUpdateStockAsync(List<ProductStock> stock);

        /// <summary>
        /// Remove a item
        /// </summary>
        Task<int> DeleteItemAsync<T>(T item) where T : class;


        /// <summary>
        /// Loads products
        /// </summary>
        Task<List<Product>> LoadProductsAsync();

        /// <summary>
        /// Loads stock
        /// </summary>
        Task<List<ProductStock>> LoadStockAsync();

        /// <summary>
        /// Loads user
        /// </summary>
        Task<User> LoadUserAsync(string userName, string password);

        /// <summary>
        /// Load local user
        /// </summary>
        Task<User> LoadLocalUserAsync();

        /// <summary>
        /// Load local user
        /// </summary>
        Task<bool> DeleteLocalUserAsync(User user);

        /// <summary>
        /// Save local user
        /// </summary>
        Task<bool> SaveLocalUserAsync(User user);

        /// <summary>
        /// Loads user
        /// </summary>
        Task<User> LoadRecoveryUserAsync(string userName);
    }
}
