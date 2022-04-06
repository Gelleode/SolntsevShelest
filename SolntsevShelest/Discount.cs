using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolntsevShelest
{
    class Discount
    {
        public int agentId { get; set; }
        public int productAmount { get; set; }
        public decimal totalMoney { get; set; }
        public int discount
        {
            get
            {
                if (totalMoney == 0)
                    return 0;
                if (totalMoney >= 0 && totalMoney < 10000)
                    return 5;
                if (totalMoney >= 10000 && totalMoney < 50000)
                    return 10;
                if (totalMoney >= 50000 && totalMoney < 150000)
                    return 20;
                return 25;
            }
        }
    }
}