using interview.Data;
using interview.Models;
using interview.Services.Implementations;
using interview.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace interview.Tests.Services
{
    public class OrderMvcServiceTests
    {
        private NorthwindContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: dbName) // �C�Ӵ��եΤ��P DB �קK��ƦìV
                .Options;

            var context = new NorthwindContext(options);

            // ��l�ƴ��ե��n���
            if (!context.Customers.Any())
            {
                context.Customers.Add(new Customer { CustomerId = "ALFKI", CompanyName = "Alfreds Futterkiste" });
                context.Employees.Add(new Employee { EmployeeId = 1, FirstName = "Janet", LastName = "Fuller" });
                context.Shippers.Add(new Shipper { ShipperId = 1, CompanyName = "Speedy Express" });
                context.SaveChanges();
            }

            return context;
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Order()
        {
            // Arrange
            var context = GetInMemoryDbContext(nameof(CreateAsync_Should_Add_Order));
            var service = new OrderMvcService(context);

            var vm = new OrderViewModel
            {
                CustomerId = "ALFKI",
                EmployeeId = 1,
                OrderDate = DateTime.UtcNow,
                ShipCity = "Berlin",
                ShipCountry = "Germany",
                ShipVia = 1
            };

            // Act
            var result = await service.CreateAsync(vm);

            // Assert
            Assert.True(result);
            Assert.Equal(1, context.Orders.Count());
            var order = context.Orders.First();
            Assert.Equal("ALFKI", order.CustomerId);
            Assert.Equal("Berlin", order.ShipCity);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Order()
        {
            var context = GetInMemoryDbContext(nameof(GetByIdAsync_Should_Return_Order));
            context.Orders.Add(new Order
            {
                OrderId = 1,
                CustomerId = "ALFKI",
                EmployeeId = 1,
                OrderDate = DateTime.UtcNow,
                ShipVia = 1
            });
            await context.SaveChangesAsync();

            var service = new OrderMvcService(context);

            var orderVm = await service.GetByIdAsync(1);

            Assert.NotNull(orderVm);
            Assert.Equal("ALFKI", orderVm.CustomerId);
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Order()
        {
            var context = GetInMemoryDbContext(nameof(UpdateAsync_Should_Modify_Order));
            context.Orders.Add(new Order
            {
                OrderId = 1,
                CustomerId = "ALFKI",
                EmployeeId = 1,
                OrderDate = DateTime.UtcNow,
                ShipVia = 1
            });
            await context.SaveChangesAsync();

            var service = new OrderMvcService(context);

            var vm = new OrderViewModel
            {
                OrderId = 1,
                CustomerId = "BLAUS",
                EmployeeId = 2,
                OrderDate = DateTime.UtcNow,
                ShipCity = "Paris",
                ShipCountry = "France",
                ShipVia = 1
            };

            var result = await service.UpdateAsync(vm);

            Assert.True(result);
            var order = context.Orders.First();
            Assert.Equal("BLAUS", order.CustomerId);
            Assert.Equal("Paris", order.ShipCity);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Order()
        {
            var context = GetInMemoryDbContext(nameof(DeleteAsync_Should_Remove_Order));
            context.Orders.Add(new Order
            {
                OrderId = 1,
                CustomerId = "ALFKI",
                EmployeeId = 1,
                OrderDate = DateTime.UtcNow,
                ShipVia = 1
            });
            await context.SaveChangesAsync();

            var service = new OrderMvcService(context);

            var result = await service.DeleteAsync(1);

            Assert.True(result);
            Assert.Empty(context.Orders);
        }
    }
}