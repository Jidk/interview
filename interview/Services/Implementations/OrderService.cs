using interview.Data;
using interview.Models;
using interview.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace interview.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly NorthwindContext _context;

        public OrderService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, Order order)
        {
            if (id != order.OrderId) return false;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Orders.AnyAsync(o => o.OrderId == id))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<Order> AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
