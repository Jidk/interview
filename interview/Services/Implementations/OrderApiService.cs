using interview.Data;
using interview.Data.Repositories;
using interview.DTOs;
using interview.Models;
using interview.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace interview.Services.Implementations
{
    public class OrderApiService : IOrderApiService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderApiService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(ToDto);
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order == null ? null : ToDto(order);
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            var entity = ToEntity(dto);
            var created = await _orderRepository.CreateAsync(entity);
            return ToDto(created);
        }

        public async Task<bool> UpdateAsync(int id, OrderDto dto)
        {
            if (id != dto.OrderId) return false;
            var entity = ToEntity(dto);
            return await _orderRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

        private static OrderDto ToDto(Order o) =>
            new OrderDto
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

        private static Order ToEntity(OrderDto dto) =>
            new Order
            {
                OrderId = dto.OrderId,
                CustomerId = dto.CustomerId,
                EmployeeId = dto.EmployeeId,
                OrderDate = dto.OrderDate,
                RequiredDate = dto.RequiredDate,
                ShippedDate = dto.ShippedDate,
                ShipVia = dto.ShipVia,
                Freight = dto.Freight,
                ShipName = dto.ShipName,
                ShipAddress = dto.ShipAddress,
                ShipCity = dto.ShipCity,
                ShipRegion = dto.ShipRegion,
                ShipPostalCode = dto.ShipPostalCode,
                ShipCountry = dto.ShipCountry
            };

        private static Order ToEntity(CreateOrderDto dto) =>
            new Order
            {
                CustomerId = dto.CustomerId,
                EmployeeId = dto.EmployeeId,
                OrderDate = dto.OrderDate,
                RequiredDate = dto.RequiredDate,
                ShippedDate = dto.ShippedDate,
                ShipVia = dto.ShipVia,
                Freight = dto.Freight,
                ShipName = dto.ShipName,
                ShipAddress = dto.ShipAddress,
                ShipCity = dto.ShipCity,
                ShipRegion = dto.ShipRegion,
                ShipPostalCode = dto.ShipPostalCode,
                ShipCountry = dto.ShipCountry
            };
    }
}
