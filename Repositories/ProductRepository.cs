using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;
        public ProductRepository(ProductContext context)
        {
            _context = context;
        }
        public async Task<Product> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Delete(int productId)
        {
            var productToDelete = await _context.Products.FindAsync(productId);
            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> Get(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<IEnumerable<Product>> SearchByName(string name)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.ProductName.Contains(name));
            }
            return await query.ToListAsync();
        }

        public async Task<Product> Update(Product product)
        {
            var result = await _context.Products
                .FirstOrDefaultAsync(e => e.Id == product.Id);

            if (result != null)
            {
                result.ProductName = product.ProductName;
                result.AverageWeeklySales = product.AverageWeeklySales;
                result.MaximumWeeklySales = product.MaximumWeeklySales;
                result.AverageDeliveryTimeInWeeks = product.AverageDeliveryTimeInWeeks;
                result.MaximumDeliveryTimeInWeeks = product.MaximumDeliveryTimeInWeeks;
                result._ReorderPoint = product.ReorderPoint;
                result._ReorderQuantity = product.ReorderQuantity;

                await _context.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
