using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore.models;

namespace AppCore
{
    public interface IExcludeFilter
    {
        /// <summary>
        /// Add Stock to Exclude
        /// </summary>
        /// <param name="stockId">Id of stock to Exclude</param>
        /// <exception cref="ArgumentNullException">Get Null argument</exception>
        /// <exception cref="InvalidOperationException">Stock not found</exception>
        Task Add(int stockId);
        /// <summary>
        /// Get excludes list
        /// </summary>
        /// <returns></returns>
        Task<List<StockDTO>> GetExcludes();
        /// <summary>
        /// Remove Stock from Excludes
        /// </summary>
        /// <param name="stockId">If of Stock to remove Excluding</param>
        /// <exception cref="ArgumentNullException">Get Null argument</exception>
        /// <exception cref="InvalidOperationException">Stock not found</exception>
        /// <returns></returns>
        Task Remove(int stockId);
    }
}
