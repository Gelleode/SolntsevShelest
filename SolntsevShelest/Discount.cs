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
        public int discountAmount
        {
            get
            {
                if (totalMoney == 0)
                    return 0;
                return 5;
            }
        } 
    }
}
