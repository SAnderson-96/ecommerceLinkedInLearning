using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private IOrderService _orderService;
        private IProductsService _productsService;
        private ICustomersService _customersService;

        public SearchService(IOrderService orderService, IProductsService productsService, ICustomersService customersService)
        {
            _orderService = orderService;
            _productsService = productsService;
            _customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var orderResult = await _orderService.GetOrdersAsync(customerId);
            var productResult = await _productsService.GetProductsAsync();
            var customerResult = await _customersService.GetCustomerAsync(customerId);
            if (orderResult.IsSuccess)
            {
                foreach (var order in orderResult.orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productResult.IsSuccess ?
                            productResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name
                            : "Product Information is not available";
                    }
                }
                if (!customerResult.IsSuccess)
                {
                    customerResult.Customer.Name = "Customer Information is not available";
                }
                var result = new
                {
                    Customer = customerResult.Customer,
                    Orders = orderResult.orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
