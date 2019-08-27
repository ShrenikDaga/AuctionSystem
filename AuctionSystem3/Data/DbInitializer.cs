using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionSystem3.Models;

namespace AuctionSystem3.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Selles.Any())
            {
                var sellers = new Seller[] {
                    new Seller { Name="Waldomart"},
                    new Seller { Name="Costca"},
                };

                foreach (Seller s in sellers)
                {
                    context.Selles.Add(s);
                }
                context.SaveChanges();
            }

            if (!context.Bidders.Any())
            {
                var bidders = new Bidder[] {
                    new Bidder{ Name="NewYorkBidders"},
                    new Bidder{ Name="ArizonaBidders"},
                };

                foreach (Bidder b in bidders)
                {
                    context.Bidders.Add(b);
                }
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var products = new Product[] {
                    new Product{ Name="Diamond",Description="1920 diamond necklace", InitialPrice=1000,Image="Diamond.jpg",BidAmount=0,IsActive=true,EndBid=DateTime.Now.AddMinutes(3),SellerID=1},
                    new Product{ Name="Xbox",Description="Matte finish xbox",InitialPrice=300,Image="Xbox.jpg",BidAmount=0,IsActive=true,EndBid=DateTime.Now.AddMinutes(2),SellerID=1 },
                    new Product{ Name="Shoes",Description="Kanye's shoes",InitialPrice=500,Image="Shoes.jpg",BidAmount=0,IsActive=true,EndBid=DateTime.Now.AddMinutes(2).AddSeconds(30),SellerID=2},
                };

                foreach (Product p in products)
                {
                    context.Products.Add(p);
                }
                context.SaveChanges();
            }

            if (!context.Bids.Any())
            {
                var bids = new Bids[] {
                    new Bids{ BidderName="NewYorkBidders",BidderID=1,BidAmount=1150,ProductID=1,BidDate=DateTime.Now},
                    new Bids{ BidderName="ArizonaBidders",BidderID=2,BidAmount=1300,ProductID=1,BidDate=DateTime.Now},
                    new Bids{ BidderName="ArizonaBidders",BidderID=2,BidAmount=350,ProductID=2,BidDate=DateTime.Now},
                };
                foreach (Bids b in bids)
                {
                    context.Bids.Add(b);
                }
                context.SaveChanges();
            }

            //context.SaveChanges();
        }
    }
}
