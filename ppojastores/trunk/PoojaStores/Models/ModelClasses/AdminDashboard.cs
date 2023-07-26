using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    public class AdminDashboard
    {
        public int NoOfCustomers { get; set; }
        public int NoOfOrdersToBeDisptched { get; set; }
        public int NoOfOrdersToGetReturn { get; set; }
        public decimal? ThisMonthSales { get; set; }
    }
}
