using AutoMapper;
using Entities.Concrete;
using Entities.DTOs;
using Entities.DTOs.Ingredients;
using Entities.DTOs.Orders;
using Entities.DTOs.Payments;
using Entities.DTOs.Products;
using Entities.DTOs.Tables;
using Entities.DTOs.Users;
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
            CreateMap<Product, ProductGetDto>()
    .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.ProductIngredients));


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


            CreateMap<Ingredient, StockAlertDto>().ReverseMap();

            CreateMap<UserCreateDto, User>()
    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
    .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

            CreateMap<User, UserForLoginDto>().ReverseMap();
            CreateMap<User, UserForRegisterDto>().ReverseMap();
            CreateMap<User, UserListDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();


            CreateMap<OperationClaim, OperationClaimCreateDto>().ReverseMap();
            CreateMap<OperationClaim, OperationClaimListDto>().ReverseMap();
            CreateMap<OperationClaim, OperationClaimUpdateDto>().ReverseMap();

            

            CreateMap<UserOperationClaim, UserOperationClaimCreateDto>().ReverseMap();
            CreateMap<UserOperationClaim, UserOperationClaimListDto>().ReverseMap();




















        }
    }
}
