using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Data.Domain
{
    public class ProductTranslation
    {
        public int Id { set; get; }
        public int ProductId { set; get; }
        public Product Product { get; set; }
        public string Name { set; get; }
        public string ShortDescription { set; get; }
        public string FullDescription { set; get; }        
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }        
        public Language Language { get; set; }
    }
}
