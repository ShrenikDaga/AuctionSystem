using System;
using System.Collections.Generic;

namespace DbManipulator
{
    public partial class Bids
    {
        public int BidsId { get; set; }
        public int BidderId { get; set; }
        public string BidderName { get; set; }
        public int BidAmount { get; set; }
        public DateTime BidDate { get; set; }
        public int ProductId { get; set; }
    }
}
