using POS_SYSTEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByBarcodeAsync(string barcode);
        Task<List<Product>> GetByIdsAsync(List<int> ids);
        Task<int> GetMaxProductIdAsync();

        Task<List<Category>> GetCategoriesAsync();
        Task<List<Brand>> GetBrandsAsync();
        Task<List<Unit>> GetUnitsAsync();

        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<bool> SaveChangesAsync();
    }
}
