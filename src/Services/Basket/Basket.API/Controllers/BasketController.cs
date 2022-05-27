﻿using System.Net;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
    {
        this._discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
    }


    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await this._basketRepository.GetBasket(userName);
        return Ok(basket ?? new ShoppingCart(userName));
    }


    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        //Communicate with Discount grpc server and calculate latest price of product into shopping cart
        foreach (var basketItem in basket.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(basketItem.ProductName);
            basketItem.Price -= coupon.Amount; //We setting the product price for every product with discounted
        }
        
        return Ok(await this._basketRepository.UpdateBasket(basket));
    }


    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await this._basketRepository.DeleteBasket(userName);
        return Ok();
    }
}