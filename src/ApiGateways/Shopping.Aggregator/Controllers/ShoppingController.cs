using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingController : ControllerBase
{
    private readonly ICatalogService _catalogService;
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;
    private readonly ILogger<ShoppingController> _logger;

    public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService, ILogger<ShoppingController> logger)
    {
        _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{username}", Name = "GetShopping")]
    [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingModel>> GetShopping(string username)
    {
        // get basket with username
        // iterate basket items and consume products with basket item productId member
        // map product related members into basketitem dto with extended columns
        // consume ordering microservices in order to retrieve order list
        // return root ShoppngModel dto class which including all responses

        var basket = await _basketService.GetBasket(username);

        foreach (var item in basket.Items)
        {
            var product = await _catalogService.GetCatalog(item.ProductId);

            // set additional product fields onto basket item
            item.ProductName = product.Name;
            item.Category = product.Category;
            item.Summary = product.Summary;
            item.Description = product.Description;
            item.ImageFile = product.ImageFile;
        }

        var orders = await _orderService.GetOrdersByUserName(username);

        var shoppingModel = new ShoppingModel
        {
            UserName = username,
            BasketWithProducts = basket,
            Orders = orders
        };

        return Ok(shoppingModel);
    }


    [HttpGet]
    public IActionResult TestLog()
    {
        _logger.LogInformation("Test Log");
        return Ok();
    }
}