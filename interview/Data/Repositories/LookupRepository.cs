using interview.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace interview.Data.Repositories
{
    public class LookupRepository : ILookupRepository
    {
        private readonly NorthwindContext _context;

        public LookupRepository(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<SelectList> GetCustomersAsync(string? id = null)
        {
            return new SelectList(await _context.Customers.Select(c => new { c.CustomerId }).ToListAsync(), "CustomerId", "CustomerId", id);
        }

        public async Task<SelectList> GetEmployeesAsync(int? id = null)
        {
            return new SelectList(await _context.Employees.Select(c => new { c.EmployeeId }).ToListAsync(), "EmployeeId", "EmployeeId", id);
        }

        public async Task<SelectList> GetShippersAsync(int? id = null)
        {
            return new SelectList(await _context.Shippers.Select(c => new { c.ShipperId }).ToListAsync(), "ShipperId", "ShipperId", id);
        }
    }
}
