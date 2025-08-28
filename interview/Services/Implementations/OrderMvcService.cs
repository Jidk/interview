using interview.Data;
using interview.Models;
using interview.Services.Interfaces;
using interview.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace interview.Services.Implementations
{
    public class OrderMvcService : IOrderMvcService
    {
        private readonly NorthwindContext _context;

        public OrderMvcService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders.Select(o => ToViewModel(o));
        }

        public async Task<OrderViewModel?> GetByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return order == null ? null : ToViewModel(order);
        }

        public async Task<OrderViewModel?> GetForEditAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return null;

            var vm = ToViewModel(order);
            vm.Customers = new SelectList(await _context.Customers.Select(c => new { c.CustomerId }).ToListAsync(), "CustomerId", "CustomerId", order.CustomerId);
            vm.Employees = new SelectList(await _context.Employees.Select(e => new { e.EmployeeId }).ToListAsync(), "EmployeeId", "EmployeeId", order.EmployeeId);
            vm.Shippers = new SelectList(await _context.Shippers.Select(s => new { s.ShipperId }).ToListAsync(), "ShipperId", "ShipperId", order.ShipVia);
            return vm;
        }

        public async Task<OrderViewModel> BuildCreateModelAsync()
        {
            var vm = new OrderViewModel();
            vm.Customers = new SelectList(await _context.Customers.Select(c => new { c.CustomerId }).ToListAsync(), "CustomerId", "CustomerId");
            vm.Employees = new SelectList(await _context.Employees.Select(e => new { e.EmployeeId }).ToListAsync(), "EmployeeId", "EmployeeId");
            vm.Shippers = new SelectList(await _context.Shippers.Select(s => new { s.ShipperId }).ToListAsync(), "ShipperId", "ShipperId");

            return vm;
        }

        public async Task<bool> CreateAsync(OrderViewModel vm)
        {
            var entity = ToEntity(vm);
            _context.Orders.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(OrderViewModel vm)
        {
            var entity = await _context.Orders.FindAsync(vm.OrderId);
            if (entity == null) return false;

            entity.CustomerId = vm.CustomerId;
            entity.EmployeeId = vm.EmployeeId;
            entity.OrderDate = vm.OrderDate;
            entity.RequiredDate = vm.RequiredDate;
            entity.ShippedDate = vm.ShippedDate;
            entity.ShipVia = vm.ShipVia;
            entity.Freight = vm.Freight;
            entity.ShipName = vm.ShipName;
            entity.ShipAddress = vm.ShipAddress;
            entity.ShipCity = vm.ShipCity;
            entity.ShipRegion = vm.ShipRegion;
            entity.ShipPostalCode = vm.ShipPostalCode;
            entity.ShipCountry = vm.ShipCountry;

            _context.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Orders.FindAsync(id);
            if (entity == null) return false;

            _context.Orders.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private OrderViewModel ToViewModel(Order o) =>
            new OrderViewModel
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

        private Order ToEntity(OrderViewModel vm) =>
            new Order
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
