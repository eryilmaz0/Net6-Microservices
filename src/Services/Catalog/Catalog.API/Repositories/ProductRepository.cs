using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    
    public ProductRepository(ICatalogContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _context.Products.Find(x => true).ToListAsync();
    }
    

    public async Task<Product> GetProduct(string id)
    {
        return await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
    

    public async Task<IEnumerable<Product>> GetProductByName(string productName)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Name, productName);
        return await _context.Products.Find(filter).ToListAsync();
    }
    

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Category, categoryName);
        return await _context.Products.Find(filter).ToListAsync();
    }
    

    public async Task CreateProduct(Product product)
    {
        await _context.Products.InsertOneAsync(product);
    }
    

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult =
            await _context.Products.ReplaceOneAsync(filter: x => x.Id == product.Id, replacement: product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 1;
    }
    

    public async Task<bool> DeleteProduct(string productId)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Id, productId);
        DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}