using ECommerce.Api.Products.Providers;
using ECommerce.Api.Products.Db;
using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using ECommerce.Api.Products.Profiles;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProductsAsync()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProductsAsync))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);


            var productProfile = new ProductProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(config);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var products = await productsProvider.GetProductsAsync();

            Assert.True(products.IsSuccess);
            Assert.True(products.Products.Any());
            Assert.Null(products.ErrorMessage);
        }

        [Fact]
        public async Task GetProductsReturnsProductsUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsProductsUsingValidId))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);


            var productProfile = new ProductProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(config);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(1);

            Assert.True(product.IsSuccess);
            Assert.True(product.Product.Id == 1);
            Assert.NotNull(product.Product);
            Assert.Null(product.ErrorMessage);
        }


        [Fact]
        public async Task GetProductsReturnsProductsUsingInValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsProductsUsingInValidId))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);


            var productProfile = new ProductProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(config);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(-1);

            Assert.False(product.IsSuccess);
            Assert.Null(product.Product);
            Assert.NotNull(product.ErrorMessage);
        }


        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)    
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });
            }
            dbContext.SaveChanges();
        }
    }
}
