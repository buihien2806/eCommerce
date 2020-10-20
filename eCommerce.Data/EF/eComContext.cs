using eCommerce.Data.Configurations;
using eCommerce.Data.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Data.EF
{
    public class eComContext: DbContext
    {
        public eComContext(DbContextOptions<eComContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API            
            modelBuilder.ApplyConfiguration(new AppConfigConfiguration());
            //base.OnModelCreating(modelBuilder);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
