using eCommerce.Model.Common;
using eCommerce.Model.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Model.Catalog.ProductImages;

namespace eCommerce.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<int> ProductCreate(ProductCreateRequest request);

        Task<int> ProductUpdate(ProductUpdateRequest request);
        Task<bool> ProductUpdatePrice(int productId, decimal newPrice);
        Task<bool> ProductUpdateQuantity(int productId, int quantity);

        Task<int> ProductDelete(int productId);

        Task<ProductView> ProductGetById(int productId, string languageId);
        Task<PagedResult<ProductView>> ProductGetByCategoryID(string languageId, ProductGuiGetPagingRequest request);
        Task<PagedResult<ProductView>> ProductGetAllPaging(ProductAdminGetPagingRequest request);

        Task<List<ProductImageView>> GetListImage(int productId);
        Task<int> ProductImageAdd(int productId, ProductImageCreate request);
        Task<int> ProductImageRemove(int imageId);
        Task<int> ProductImageUpdate(int imageId, ProductImageUpdate request);
        Task<ProductImageView> ProductImageGetByID(int imageId);
    }
}
