using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Data.Domain
{
    public class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
