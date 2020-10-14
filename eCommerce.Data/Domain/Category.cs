using eCommerce.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Data.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public Status Status { get; set; }
    }
}
