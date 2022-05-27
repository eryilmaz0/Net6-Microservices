using AutoMapper;
using Discount.GRPC.Entities;
using Discount.Grpc.Protos;

namespace Discount.GRPC.MapperProfile;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Coupon, CouponModel>().ReverseMap();
    }
}