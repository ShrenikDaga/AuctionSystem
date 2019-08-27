using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionSystem3.Models
{
    public class Bids
    {
        public int BidsID { get; set; }
        public int BidderID { get; set; }
        public int ProductID { get; set; }
        public string BidderName { get; set; }
        public int BidAmount { get; set; }
        public DateTime BidDate { get; set; }
    }
}
