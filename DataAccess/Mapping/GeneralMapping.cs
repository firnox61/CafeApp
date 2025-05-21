using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.Ingredients;
using Entities.DTOs.Orders;
using Entities.DTOs.Payments;
using Entities.DTOs.Products;
using Entities.DTOs.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapping
{
    public class GeneralMapping : Profile
    {


        public GeneralMapping()
        {
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<ProductIngredient, ProductIngredientCreateDto>().ReverseMap();

            CreateMap<Table, TableCreateDto>().ReverseMap();
            CreateMap<Table, TableUpdateDto>().ReverseMap();

            CreateMap<Payment, PaymentCreateDto>().ReverseMap();
            CreateMap<Payment, PaymentGetDto>().ReverseMap();

            CreateMap<Order, OrderCreateDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemsCreateDto>().ReverseMap();

          //  CreateMap<Order, OrderGetDto>().ReverseMap();
         //   CreateMap<OrderItem, OrderItemGetDto>().ReverseMap();


            // AutoMapper profile:
            CreateMap<Order, OrderGetDto>()
                .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.Table.Name))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemGetDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));


            CreateMap<Ingredient, IngredientCreateDto>().ReverseMap();
            CreateMap<Ingredient, IngredientUpdateDto>().ReverseMap();
            CreateMap<Ingredient, IngredientDto>().ReverseMap();

            CreateMap<ProductIngredient, ProductIngredientCreateDto>().ReverseMap();
            CreateMap<ProductIngredient, ProductIngredientGetDto>().ReverseMap();



        }
    }
}
