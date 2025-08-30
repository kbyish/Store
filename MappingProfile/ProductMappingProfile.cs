using AutoMapper;
using Store.Models;
using Store.Contracts;

namespace Store.MappingProfile;
public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductDto, Product>();
    }
}