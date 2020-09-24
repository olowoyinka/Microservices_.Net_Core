using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext _dbContext;

        private readonly ILogger<ProductsProvider> _logger;

        private readonly IMapper _mapper;

        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this._logger = logger;
            this._dbContext = dbContext;
            this._mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!_dbContext.Products.Any())
            {
                _dbContext.Products.Add(new Product() { Id = 1, Name = "Keyboard", Price = 20, Inventory = 100 });
                _dbContext.Products.Add(new Product() { Id = 2, Name = "Mouse", Price = 5, Inventory = 200 });
                _dbContext.Products.Add(new Product() { Id = 3, Name = "Monitor", Price = 150, Inventory = 1000 });
                _dbContext.Products.Add(new Product() { Id = 4, Name = "CPU", Price = 200, Inventory = 2000 });
                _dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductModel> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await _dbContext.Products.ToListAsync();

                if (products != null && products.Any())
                {
                    var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(products);

                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());

                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ProductModel Product, string ErrorMessage)> GetProductByIdAsync(int Id)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(s => s.Id == Id);

                if (product != null)
                {
                    var result = _mapper.Map<Product, ProductModel>(product);

                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());

                return (false, null, ex.Message);
            }
        }
    }
}