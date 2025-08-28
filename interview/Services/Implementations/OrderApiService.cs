using interview.Data;
using interview.Data.Repositories;
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

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, Order order)
        {
            if (id != order.OrderId) return false;
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            return await _orderRepository.CreateAsync(order);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }
    }
}
