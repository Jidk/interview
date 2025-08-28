using Microsoft.AspNetCore.Mvc.Rendering;

namespace interview.Data.Repositories
{
    public interface ILookupRepository
    {
        Task<SelectList> GetCustomersAsync(string? id = null);
        Task<SelectList> GetEmployeesAsync(int? id = null);
        Task<SelectList> GetShippersAsync(int? id = null);
    }
}
