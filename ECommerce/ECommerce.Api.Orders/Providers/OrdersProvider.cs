using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private OrdersDbContext _dbContext;
        private ILogger<OrdersProvider> _logger;
        private IMapper _mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!_dbContext.Orders.Any())
            {
                List<Db.OrderItem> oi = new List<Db.OrderItem>();
                oi.Add(new Db.OrderItem() { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 10 });
                _dbContext.Orders.Add(new Db.Order() { Id = 1, CustomerId = 1, Total = 20, Items = oi});
                oi[0].Id = 2;
                _dbContext.Orders.Add(new Db.Order() { Id = 2, CustomerId = 1, Total = 20, Items = oi});

                _dbContext.SaveChanges();
            }
        }
        
        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersFromCustomerIdAsync(int customerId)
        {
            try
            {
                var order = await _dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
                if (order != null)
                {
                    var result = _mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(order);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, $"{ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync()
        {
            try
            {
                var orders = await _dbContext.Orders.ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = _mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, $"{ex.Message}");
            }
        }
    }
}
