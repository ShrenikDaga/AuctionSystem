using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuctionSystem3.Models;
using Microsoft.EntityFrameworkCore.Extensions;
using AuctionSystem3.Models;
using AuctionSystem3.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace AuctionSystem3.Controllers
{
    public class ProductBid
    {
        public Product product;
        public  List<Bids> bid; 
    }

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ViewBids()
        {
            List<ProductBid> productBids = new List<ProductBid>();
            var products = context.Products;

            foreach (var item in products)
            {
                var bids = context.Bids.Where(selection => selection.ProductID == item.ProductID);
                productBids.Add(new ProductBid { product=item,bid=bids.ToList()});
            }
            return View(productBids);
        }

        [HttpGet]
        public IActionResult RegisterAsSeller()
        {
            var model = new Seller();
            return View(model);
        }

        [HttpPost]
        public IActionResult RegisterAsSeller(Seller seller)
        {
            var isDup = context.Selles.Where(selection => selection.Name == seller.Name);
            var dupCount = isDup.Count();
            if (dupCount==0)
            {
                context.Selles.Add(seller);
                context.SaveChanges();
            }
            var isDup2 = context.Selles.Where(selection => selection.Name == seller.Name).First();
            return View(isDup2);
        }

        [HttpGet]
        public IActionResult RegisterAsBidder()
        {
            var model = new Bidder();
            return View(model);
        }

        [HttpPost]
        public IActionResult RegisterAsBidder(Bidder bidder)
        {
            var isDup = context.Bidders.Where(selection => selection.Name == bidder.Name);
            var dupCount = isDup.Count();
            if (dupCount == 0)
            {
                context.Bidders.Add(bidder);
                context.SaveChanges();
            }
            var isDup2 = context.Bidders.Where(selection => selection.Name == bidder.Name).First();
            return View(isDup2);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBid(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var bid = context.Bids.Find(id);
                if (bid != null)
                {
                    context.Bids.Remove(bid);
                    context.SaveChanges();
                }
            }
            catch(Exception ex){ }
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
