using interview.Data;
using interview.Models;
using interview.Services.Interfaces;
using interview.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace interview.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderMvcService _orderService;

        public OrdersController(IOrderMvcService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        public async Task<IActionResult> Create()
        {
            var vm = await _orderService.BuildCreateModelAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await _orderService.CreateAsync(vm);
                return RedirectToAction(nameof(Index));
            }
            vm = await _orderService.BuildCreateModelAsync();
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _orderService.GetForEditAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderViewModel vm)
        {
            if (id != vm.OrderId) return NotFound();

            if (ModelState.IsValid)
            {
                var success = await _orderService.UpdateAsync(vm);
                if (success) return RedirectToAction(nameof(Index));
                return NotFound();
            }
            vm = await _orderService.GetForEditAsync(id) ?? new OrderViewModel();
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
