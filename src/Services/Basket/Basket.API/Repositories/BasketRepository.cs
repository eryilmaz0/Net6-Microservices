using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _cache;

    public BasketRepository(IDistributedCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<ShoppingCart> GetBasket(string userName)
    {
        var basket = await this._cache.GetStringAsync(userName);

        if (string.IsNullOrEmpty(basket))
            return null;

        return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    
    public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
    {
        await this._cache.SetStringAsync(shoppingCart.UserName, JsonConvert.SerializeObject(shoppingCart));
        return await GetBasket(shoppingCart.UserName);
    }

    
    public async Task DeleteBasket(string userName)
    {
        await this._cache.RemoveAsync(userName);
    }
}