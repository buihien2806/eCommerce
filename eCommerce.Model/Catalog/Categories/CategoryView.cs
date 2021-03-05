using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Model.Catalog.Categories
{
    public class CategoryView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
    }
}
