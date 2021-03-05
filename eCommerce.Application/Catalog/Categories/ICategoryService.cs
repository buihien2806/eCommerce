using eCommerce.Model.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryView>> CategoryGetAll(string languageId);
    }
}
