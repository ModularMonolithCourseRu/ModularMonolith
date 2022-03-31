using AutoMapper;
using Shop.Entities.Models;
using Shop.UseCases.Orders.Dtos;

namespace Shop.UseCases.Orders.Utils;

public class OrdersProfile : Profile
{
    public OrdersProfile()
    {
        CreateMap<Order, OrderListItemDto>()
            .ForMember(x => x.Price, opt => opt.MapFrom(m => m.Items.Sum(x => x.Count * x.Product.Price)));
        
        CreateMap<CreateOrderDto, Order>();
        CreateMap<OrderItemDto, OrderItem>();

        CreateMap<Order, OrderDetailsDto>()
            .ForMember(x => x.Price, opt => opt.MapFrom(m => m.Items.Sum(x => x.Count * x.Product.Price))); ;
        CreateMap<OrderItem, OrderDetailsItemDto>()
            .ForMember(x => x.ProductName, opt => opt.MapFrom(m => m.Product.Name))
            .ForMember(x => x.ProductPrice, opt => opt.MapFrom(m => m.Product.Price))
            ;
    }
}