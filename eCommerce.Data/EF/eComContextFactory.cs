using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eCommerce.Data.EF
{
    public class eComContextFactory : IDesignTimeDbContextFactory<eComContext>
    {
        public eComContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("eCommerceDatabase");

            var optionsBuilder = new DbContextOptionsBuilder<eComContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new eComContext(optionsBuilder.Options);
        }
    }
}
