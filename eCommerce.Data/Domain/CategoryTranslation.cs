using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Data.Domain
{
    public class CategoryTranslation
    {
        public int Id { set; get; }
        public int CategoryId { set; get; }
        public Category Category { get; set; }
        public string Name { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string SeoAlias { set; get; }
        public string LanguageId { set; get; }                
        public Language Language { get; set; }
    }
}
