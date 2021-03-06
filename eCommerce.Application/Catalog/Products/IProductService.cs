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
        Task<int> ProductDelete(int productId);
        Task<ProductView> ProductGetAll();
        Task<bool> ProductUpdatePrice(int productId, decimal newPrice);
        Task<bool> ProductUpdateQuantity(int productId, int quantity);
        PagedResult<ProductView> ProductPaging(ProductAdminGetPagingRequest request);
        Task<int> ProductImageAdd(int productId, ProductImageCreate request);

        Task<int> ProductImageRemove(int imageId);

        Task<int> ProductImageUpdate(int imageId, ProductImageUpdate request);
        Task<ProductImageView> ProductImageGetByID(int imageId);

        Task<List<ProductView>> GetAll();
    }
}
