using eCommerce.Data.EF;
using eCommerce.Model.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly eComContext _context;
        public CategoryService(eComContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryView>> CategoryGetAll(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        select new { c, ct };
            return await query.Select(x => new CategoryView()
            {
                Id = x.c.Id,
                ParentId = x.c.ParentId,
                Name = x.ct.Name
            }).ToListAsync();
        }
    }
}
