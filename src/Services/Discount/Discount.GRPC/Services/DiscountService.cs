using AutoMapper;
using Discount.GRPC.Entities;
using Discount.Grpc.Protos;
using Discount.GRPC.Repositories;
using Grpc.Core;

namespace Discount.GRPC.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _discountRepository;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
    {
        _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _discountRepository.GetDiscount(request.ProductName);
        
        if(coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} Not Found."));

        var couponModel = _mapper.Map<CouponModel>(coupon);
        _logger.LogInformation($"Discount is retreived for ProductName: {couponModel.ProductName}, Amount : {couponModel.Amount}");
        return couponModel;
    }
    
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);

        await _discountRepository.CreateDiscount(coupon);
        _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);

        await _discountRepository.UpdateDiscount(coupon);
        _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await _discountRepository.DeleteDiscount(request.ProductName);
        var response = new DeleteDiscountResponse
        {
            Success = deleted
        };

        return response;
    }
}