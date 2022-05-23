using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products { get; }
    
    public CatalogContext(IConfiguration config)
    {
        var client = new MongoClient(config.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase(config.GetValue<string>("DatabaseSettings:DatabaseName"));
        this.Products = database.GetCollection<Product>(config.GetValue<string>("DatabaseSettings:CollectionName"));
        CatalogContextSeed.SeedData(this.Products);
    }
}