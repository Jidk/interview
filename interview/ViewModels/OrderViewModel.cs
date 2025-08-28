using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace interview.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        [Display(Name = "客戶代號")]
        public string? CustomerId { get; set; }

        [Display(Name = "員工編號")]
        public int? EmployeeId { get; set; }

        [Display(Name = "訂單日期")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "需求日期")]
        public DateTime? RequiredDate { get; set; }

        [Display(Name = "出貨日期")]
        public DateTime? ShippedDate { get; set; }

        [Display(Name = "出貨方式")]
        public int? ShipVia { get; set; }

        [Display(Name = "運費")]
        public decimal? Freight { get; set; }

        [Display(Name = "收貨人姓名")]
        public string? ShipName { get; set; }

        [Display(Name = "收貨地址")]
        public string? ShipAddress { get; set; }

        [Display(Name = "收貨城市")]
        public string? ShipCity { get; set; }

        [Display(Name = "收貨地區")]
        public string? ShipRegion { get; set; }

        [Display(Name = "郵遞區號")]
        public string? ShipPostalCode { get; set; }

        [Display(Name = "收貨國家")]
        public string? ShipCountry { get; set; }

        // 下拉選單資料
        public SelectList? Customers { get; set; }
        public SelectList? Employees { get; set; }
        public SelectList? Shippers { get; set; }
    }
}
