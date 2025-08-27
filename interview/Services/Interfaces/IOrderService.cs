using interview.Models;

namespace interview.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Order order);
        Task<Order> AddAsync(Order order);
        Task<bool> DeleteAsync(int id);
    }
}
