using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolntsevShelest
{
    class AgentWithDiscount
    {
        public Agent Agent { get; set; }
        public int ProductCount { get; set; }
        public decimal TotalMoney { get; set; }
        public int Discount
        {
            get
            {
                if (TotalMoney == 0)
                    return 0;
                if (TotalMoney >= 0 && TotalMoney < 10000)
                    return 5;
                if (TotalMoney >= 10000 && TotalMoney < 50000)
                    return 10;
                if (TotalMoney >= 50000 && TotalMoney < 150000)
                    return 20;
                return 25;
            }
        }
    }
}