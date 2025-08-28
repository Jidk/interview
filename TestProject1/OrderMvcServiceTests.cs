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
                .UseInMemoryDatabase(databaseName: dbName) // 每個測試用不同 DB 避免資料污染
                .Options;

            var context = new NorthwindContext(options);

            // 初始化測試必要資料
            if (!context.Customers.Any())
            {
                context.Orders.Add(new Order { OrderId = 1, EmployeeId = 1, ShipVia = 1, CustomerId = "ALFKI" });
                context.Customers.Add(new Customer { CustomerId = "ALFKI", CompanyName = "Alfreds Futterkiste" });
                context.Employees.Add(new Employee { EmployeeId = 1, FirstName = "Janet", LastName = "Fuller" });
                context.Shippers.Add(new Shipper { ShipperId = 1, CompanyName = "Speedy Express" });
                context.SaveChanges();
            }

            return context;
        }

            [Fact]
        public async Task GetAllAsync_Should_Return_All_Orders()
        {
            var context = GetInMemoryDbContext(nameof(GetAllAsync_Should_Return_All_Orders));
            context.Orders.AddRange(
                new Order { OrderId = 1, CustomerId = "ALFKI", EmployeeId = 1, OrderDate = DateTime.UtcNow },
                new Order { OrderId = 2, CustomerId = "BLAUS", EmployeeId = 2, OrderDate = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();

            var service = new OrderMvcService(context);

            var orders = await service.GetAllAsync();

            Assert.Equal(2, orders.Count());
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
        public async Task GetForEditAsync_Should_Return_Order_With_Dropdowns()
        {
            var context = GetInMemoryDbContext(nameof(GetForEditAsync_Should_Return_Order_With_Dropdowns));

            var service = new OrderMvcService(context);

            var vm = await service.GetForEditAsync(1);

            Assert.NotNull(vm);
            Assert.Equal("ALFKI", vm.CustomerId);
            Assert.NotNull(vm.Customers);
            Assert.NotNull(vm.Employees);
            Assert.NotNull(vm.Shippers);
        }

        [Fact]
        public async Task BuildCreateModelAsync_Should_Return_Empty_Model_With_Dropdowns()
        {
            var context = GetInMemoryDbContext(nameof(BuildCreateModelAsync_Should_Return_Empty_Model_With_Dropdowns));

            var service = new OrderMvcService(context);

            var vm = await service.BuildCreateModelAsync();

            Assert.NotNull(vm);
            Assert.Null(vm.CustomerId); // 尚未填值
            Assert.NotNull(vm.Customers);
            Assert.NotNull(vm.Employees);
            Assert.NotNull(vm.Shippers);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Order()
        {
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

            var result = await service.CreateAsync(vm);

            Assert.True(result);
            Assert.Equal(1, context.Orders.Count());
            var order = context.Orders.First();
            Assert.Equal("ALFKI", order.CustomerId);
            Assert.Equal("Berlin", order.ShipCity);
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