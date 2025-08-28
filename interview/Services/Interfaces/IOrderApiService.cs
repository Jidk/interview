using interview.DTOs;
using interview.Models;

namespace interview.Services.Interfaces
{
    public interface IOrderApiService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, OrderDto order);
        Task<OrderDto> CreateAsync(CreateOrderDto order);
        Task<bool> DeleteAsync(int id);
    }
}
