using eCommerce.Application.Catalog.Products;
using eCommerce.Model.Catalog.ProductImages;
using eCommerce.Model.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{paging}")]
        public async Task<IActionResult> GetAll([FromQuery] ProductAdminGetPagingRequest request)
        {
            var product = await _productService.ProductGetAllPaging(request);
            return Ok(product);
        }

        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _productService.ProductGetById(productId, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }
        //GET PRODUCT BY CATEGORY ID
        [HttpGet("{languageId}")]
        public async Task<IActionResult> ProductGetByCategoryID(string languageId, [FromQuery] ProductGuiGetPagingRequest request)
        {
            var product = await _productService.ProductGetByCategoryID(languageId, request);
            return Ok(product);
        }

        //CREATE PRODUCT
        [HttpPost]
        public async Task<IActionResult> ProductCreate([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _productService.ProductCreate(request);
            if (product == 0)
                return BadRequest();

            var result = await _productService.ProductGetById(product, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = product }, result);
        }
        //UPDATE PRODUCT
        [HttpPut]
        public async Task<IActionResult> ProductUpdate([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = await _productService.ProductUpdate(request);
            if (query == 0)
                return BadRequest();

            return Ok();
        }
        //DELETE PRODUCT
        [HttpDelete("{productId}")]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            var query = await _productService.ProductDelete(productId);
            if (query == 0)
                return BadRequest();

            return Ok();
        }
        //UPDATE PRICE
        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> ProductUpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _productService.ProductUpdatePrice(productId, newPrice);
            if (isSuccessful)
                return Ok();
            return BadRequest();
        }
        //CREATE IMAGE
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> ImageCreate(int productId, [FromForm] ProductImageCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _productService.ProductImageAdd(productId, request);
            if (imageId == 0)
                return BadRequest();

            var image = await _productService.ProductImageGetByID(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }
        //GET IMAGE BY IMAGE ID
        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _productService.ProductImageGetByID(imageId);
            if (image == null)
                return BadRequest("Cannot find product");
            return Ok(image);
        }
        //UPDATE IMAGE
        [HttpPut("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.ProductImageUpdate(imageId, request);
            if (result == 0)
                return BadRequest();

            return Ok();
        }
        //DELETE IMAGE
        [HttpDelete("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.ProductImageRemove(imageId);
            if (result == 0)
                return BadRequest();

            return Ok();
        }
    }
}
