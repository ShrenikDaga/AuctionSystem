using System;
using System.Collections.Generic;

namespace DbManipulator
{
    public partial class Selles
    {
        public Selles()
        {
            Products = new HashSet<Products>();
        }

        public int SellerId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
