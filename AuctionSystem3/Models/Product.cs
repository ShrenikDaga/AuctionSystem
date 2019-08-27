using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionSystem3.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Image { get; set; }
        public int InitialPrice { get; set; }
        public int BidAmount { get; set; }
        public int? BidderID { get; set; }
        public int SellerID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime EndBid { get; set; }
    }
}
