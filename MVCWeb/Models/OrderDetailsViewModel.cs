using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWeb.Models
{
    public class OrderDetailsViewModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Size { get; set; }
        public string AgentName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public decimal UnitAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int AgentID { get; set; }
    }
}