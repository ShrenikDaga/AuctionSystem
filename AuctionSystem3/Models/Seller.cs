using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem3.Models
{
    public class Seller
    {
        public int SellerID { get; set; }
        public String Name { get; set; }

        public ICollection<Product> ProductList { get; set; }
    }
}
