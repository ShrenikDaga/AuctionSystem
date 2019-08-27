using System;
using System.Collections.Generic;

namespace DbManipulator
{
    public partial class Products
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int InitialPrice { get; set; }
        public int BidAmount { get; set; }
        public int? BidderId { get; set; }
        public int SellerId { get; set; }
        public bool IsActive { get; set; }
        public DateTime EndBid { get; set; }

        public virtual Selles Seller { get; set; }
    }
}
