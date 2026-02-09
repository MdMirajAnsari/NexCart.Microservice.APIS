using AutoMapper;
using NexCart.Products.DTO;
using NexCart.Products.Entities;
using System;


namespace NexCart.Products.Mappers
{
    public class ProductToProductResponseMappingProfile : Profile
    {
        public ProductToProductResponseMappingProfile()
        {
            CreateMap<Product, ProductResponse>()
                .ConstructUsing(src => new ProductResponse(
                    src.ProductID,
                    src.ProductName,
                    ParseCategory(src.Category),
                    src.UnitPrice,
                    src.QuantityInStock
                ));
        }

        private static CategoryOptions ParseCategory(string? category)
        {
            if (string.IsNullOrWhiteSpace(category)) return CategoryOptions.Accessories;
            if (Enum.TryParse<CategoryOptions>(category, true, out var result)) return result;
            // fallback: try mapping common synonyms
            var normalized = category.Trim();
            return normalized switch
            {
                "Computer" or "Computers" => CategoryOptions.Electronics,
                "Home Appliance" or "Home Appliances" => CategoryOptions.HomeAppliances,
                _ => CategoryOptions.Accessories,
            };
        }
    }
}
