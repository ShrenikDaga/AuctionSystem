using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem3.Models
{
    public class Bidder
    {
        public int BidderID { get; set; }
        public String Name { get; set; }
        
        ICollection<Product> AquiredProductList { get; set; }
    }
}
