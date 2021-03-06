using eCommerce.Model.Common;
using eCommerce.Data.Domain;
using eCommerce.Data.EF;
using eCommerce.Model.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eCommerce.Application.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using eCommerce.Model.Catalog.ProductImages;

namespace eCommerce.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly eComContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public ProductService(eComContext context, 
                              IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        //CREATE PRODUCT
        public async Task<int> ProductCreate(ProductCreateRequest request)
        {
            var languages = _context.Languages;
            var translations = new List<ProductTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    translations.Add(new ProductTranslation()
                    {
                        Name = request.Name,
                        ShortDescription = request.Description,
                        FullDescription = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    });
                }
                else
                {
                    translations.Add(new ProductTranslation()
                    {
                        //Name = SystemConstants.ProductConstants.NA,
                        //Description = SystemConstants.ProductConstants.NA,
                        //SeoAlias = SystemConstants.ProductConstants.NA,
                        //LanguageId = language.Id
                    });
                }
            }
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = translations
            };
            //Save image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        CreatedDate = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
        
        //UPDATE PRODUCT
        public async Task<int> ProductUpdate(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id
            && x.LanguageId == request.LanguageId);

            if (product == null || productTranslations == null) throw new ErrorMessage($"Cannot find a product with id: {request.Id}");

            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.SeoTitle = request.SeoTitle;
            productTranslations.ShortDescription = request.Description;
            productTranslations.FullDescription = request.Details;

            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }

            return await _context.SaveChangesAsync();
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }
        //DELETE PRODUCT
        public async Task<int> ProductDelete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new ErrorMessage($"Cannot find a product: {productId}");

            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public Task<ProductView> ProductGetAll()
        {
            throw new NotImplementedException();
        }

        public PagedResult<ProductView> ProductPaging()
        {
            throw new NotImplementedException();
        }

        public PagedResult<ProductView> ProductPaging(ProductAdminGetPagingRequest request)
        {
            throw new NotImplementedException();
        }
        //UPDATE PRICE
        public async Task<bool> ProductUpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new ErrorMessage($"Cannot find a product with id: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }
        //UPDATE QUANTITY
        public async Task<bool> ProductUpdateQuantity(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new ErrorMessage($"Cannot find a product with id: {productId}");
            product.Stock += quantity;
            return await _context.SaveChangesAsync() > 0;
        }
        //ADD PRODUCT IMAGE
        public async Task<int> ProductImageAdd(int productId, ProductImageCreate request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                CreatedDate = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }
        //REMOVE PRODUCT IMAGE
        public async Task<int> ProductImageRemove(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new ErrorMessage($"Cannot find an image with id {imageId}");
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }
        //UPDATE PRODUCT IMAGE
        public async Task<int> ProductImageUpdate(int imageId, ProductImageUpdate request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new ErrorMessage($"Cannot find an image with id {imageId}");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }
        //GET PRODUCT IMAGE BY ID
        public async Task<ProductImageView> ProductImageGetByID(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new ErrorMessage($"Cannot find an image with id {imageId}");

            var viewModel = new ProductImageView()
            {
                Caption = image.Caption,
                DateCreated = image.CreatedDate,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }

        public async Task<List<ProductView>> GetAll()
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id                        
                        select new { p, pt, pic };
            var data = await query.Select(x => new ProductView()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.ShortDescription,
                    Details = x.pt.FullDescription,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();
            return data;
        }
    }
}
