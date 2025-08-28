using interview.Data;
using interview.Data.Repositories;
using interview.Models;
using interview.Services.Interfaces;
using interview.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace interview.Services.Implementations
{
    public class OrderMvcService : IOrderMvcService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILookupRepository _lookupRepository;

        public OrderMvcService(IOrderRepository orderRepository, ILookupRepository lookupRepository)
        {
            _orderRepository = orderRepository;
            _lookupRepository = lookupRepository;
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(o => ToViewModel(o));
        }

        public async Task<OrderViewModel?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order == null ? null : ToViewModel(order);
        }

        public async Task<OrderViewModel?> GetForEditAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            var vm = ToViewModel(order);
            vm.Customers = await _lookupRepository.GetCustomersAsync(order.CustomerId);
            vm.Employees = await _lookupRepository.GetEmployeesAsync(order.EmployeeId);
            vm.Shippers = await _lookupRepository.GetShippersAsync(order.ShipVia);
            return vm;
        }

        public async Task<OrderViewModel> BuildCreateModelAsync()
        {
            var vm = new OrderViewModel
            {
                Customers = await _lookupRepository.GetCustomersAsync(),
                Employees = await _lookupRepository.GetEmployeesAsync(),
                Shippers = await _lookupRepository.GetShippersAsync()
            };
            return vm;
        }

        public async Task<bool> CreateAsync(OrderViewModel vm)
        {
            var entity = ToEntity(vm);
            await _orderRepository.CreateAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(OrderViewModel vm)
        {
            var entity = ToEntity(vm);
            return await _orderRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

        private OrderViewModel ToViewModel(Order o) => new OrderViewModel
        {
            OrderId = o.OrderId,
            CustomerId = o.CustomerId,
            EmployeeId = o.EmployeeId,
            OrderDate = o.OrderDate,
            RequiredDate = o.RequiredDate,
            ShippedDate = o.ShippedDate,
            ShipVia = o.ShipVia,
            Freight = o.Freight,
            ShipName = o.ShipName,
            ShipAddress = o.ShipAddress,
            ShipCity = o.ShipCity,
            ShipRegion = o.ShipRegion,
            ShipPostalCode = o.ShipPostalCode,
            ShipCountry = o.ShipCountry
        };

        private Order ToEntity(OrderViewModel vm) => new Order
        {
            OrderId = vm.OrderId,
            CustomerId = vm.CustomerId,
            EmployeeId = vm.EmployeeId,
            OrderDate = vm.OrderDate,
            RequiredDate = vm.RequiredDate,
            ShippedDate = vm.ShippedDate,
            ShipVia = vm.ShipVia,
            Freight = vm.Freight,
            ShipName = vm.ShipName,
            ShipAddress = vm.ShipAddress,
            ShipCity = vm.ShipCity,
            ShipRegion = vm.ShipRegion,
            ShipPostalCode = vm.ShipPostalCode,
            ShipCountry = vm.ShipCountry
        };
    }


}
