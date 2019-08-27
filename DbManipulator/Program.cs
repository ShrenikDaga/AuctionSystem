using System;
using System.Collections.Generic;
using System.Linq;

namespace DbManipulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var context = new aspnetAuctionSystem3BD4A4B35380A4C089BCE37126FD1CF6FContext())
            {
                while (true)
                {
                    var allProducts = context.Products.Where(selection => selection.IsActive == true).ToList();

                    foreach (var item in allProducts)
                    {
                        if (item.EndBid < DateTime.Now)
                        {
                            var itemUpdate = item;
                            itemUpdate.IsActive = false;
                            context.Products.Update(itemUpdate);
                            context.SaveChanges();
                            Console.WriteLine("Item: {0} removed from the market as its bidding time is up.", item.Name);
                        }
                    }
                    //Console.WriteLine("Before sleep");
                    System.Threading.Thread.Sleep(2000);
                    //Console.WriteLine("After sleep");
                }   
            }
        }
    }
}
