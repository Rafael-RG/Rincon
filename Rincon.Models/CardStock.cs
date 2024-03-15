using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rincon.Models
{
    public class CardStock
    {
        public int Id { get; set; }
        public int StockAvailable { get; set; }
        public string Description { get; set; }
        public int StockReserved { get; set; }
        public string Icon { get; set; }

    }
}
