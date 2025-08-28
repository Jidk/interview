using interview.ViewModels;

namespace interview.Services.Interfaces
{
    public interface IOrderMvcService
    {
        Task<IEnumerable<OrderViewModel>> GetAllAsync();
        Task<OrderViewModel?> GetByIdAsync(int id);
        Task<OrderViewModel?> GetForEditAsync(int id);
        Task<bool> CreateAsync(OrderViewModel vm);
        Task<bool> UpdateAsync(OrderViewModel vm);
        Task<bool> DeleteAsync(int id);
        Task<OrderViewModel> BuildCreateModelAsync();
    }
}
