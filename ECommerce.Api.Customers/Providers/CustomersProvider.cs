using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext _dbContext;

        private readonly ILogger<CustomersProvider> _logger;

        private readonly IMapper _mapper;

        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._logger = logger;
            this._mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!_dbContext.Customers.Any())
            {
                _dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Jessica Smith", Address = "20 Elm St." });
                _dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "John Smith", Address = "30 Main St." });
                _dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "William Johnson", Address = "100 10th St." });
                _dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, CustomerModel Customer, string ErrorMessage)> GetCustomerByIdAsync(int id)
        {
            try
            {
                _logger?.LogInformation("Querying customers");

                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);

                if (customer != null)
                {
                    _logger?.LogInformation("Customer found");

                    var result = _mapper.Map<Customer, CustomerModel>(customer);

                    return (true, result, null);
                }

                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());

                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<CustomerModel> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                _logger?.LogInformation("Querying customers");

                var customers = await _dbContext.Customers.ToListAsync();

                if (customers != null && customers.Any())
                {
                    _logger?.LogInformation($"{customers.Count} customer(s) found");

                    var result = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerModel>>(customers);

                    return (true, result, null);
                }

                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());

                return (false, null, ex.Message);
            }
        }
    }
}