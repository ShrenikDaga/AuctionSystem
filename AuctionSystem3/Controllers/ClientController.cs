using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionSystem3.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using AuctionSystem3.Models;
using System.Net.Http;
using System.Net.Http.Headers;


namespace AuctionSystem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment_;
        private string webRootPath = null;
        private string filePath = null;
        private readonly ApplicationDbContext context;

        public ClientController(IHostingEnvironment hostingEnvironment, ApplicationDbContext dbContext)
        {
            hostingEnvironment_ = hostingEnvironment;
            webRootPath = hostingEnvironment_.WebRootPath;
            filePath = Path.Combine(webRootPath, "FileStorage");
            context = dbContext;
        }

        [HttpGet]
        public string Send()
        {
            return "Shrenik";
        }

        public class ProductItem
        {
            public string image;
            public Product product;
        }

        [HttpGet("{id}/{token}")]
        public IEnumerable<ProductItem> SendProductListToSeller(int id, int token)
        {
            IQueryable<Product> products;
            if (token == 1)
            {
                products = context.Products.Where(selection => selection.SellerID == id);
            }
            else
            {
                products = context.Products;
            }
            
            List<ProductItem> productItems = new List<ProductItem>();
            foreach (var item in products)
            {
                string file = Path.Combine(filePath,item.Image);
                byte[] iTempData = System.IO.File.ReadAllBytes(file);
                string imBase64Data = Convert.ToBase64String(iTempData);
                string imgDataFinal = string.Format("data:image/jpeg;base64,{0}",imBase64Data);
                productItems.Add(new ProductItem() { image = imgDataFinal,product = item});
            }
            return productItems;
        }

        [HttpGet("{id}/{ida}/{idb}")]
        public IEnumerable<Bids> SendProductBidDetaisl(int id, int ida, int idb)
        {
            var bids = context.Bids.Where(selection => selection.ProductID == id);
            return bids;
        }

        [HttpGet("{id}")]
        public IEnumerable<string> VerifyID(int id)
        {
            List<string> UserDetails = new List<string>();
            int userBitType = id % 10;
            id /= 10;

            if (userBitType == 0)
            {
                var available = context.Selles.Find(id);
                if (available != null)
                {
                    UserDetails.Add(available.SellerID.ToString());
                    UserDetails.Add(available.Name);
                    UserDetails.Add("Seller");
                    return UserDetails;
                }
                return null;
            }
            else if (userBitType == 1)
            {
                var available = context.Bidders.Find(id);
                if (available != null)
                {
                    UserDetails.Add(available.BidderID.ToString());
                    UserDetails.Add(available.Name);
                    UserDetails.Add("Bidder");
                    return UserDetails;
                }
                return null;
            }
            else
                return null;

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostBid([FromBody]dynamic requestObject)
        {
            var bidderID = requestObject.BidderID;
            var bidderName = requestObject.BidderName;
            var bidAmount = requestObject.BidAmount;
            var bidDate = requestObject.BidDate;
            var productId = requestObject.ProductID;

            var bid = new Bids{ BidderID = bidderID, BidderName = bidderName, BidAmount = bidAmount, BidDate = bidDate, ProductID = productId};
            context.Bids.Add(bid);
            context.SaveChanges();

            Product product = context.Products.Find(bid.ProductID);
            if (product.BidAmount < bid.BidAmount)
            {
                product.BidAmount = bid.BidAmount;
                product.BidderID = bid.BidderID;
                context.Products.Update(product);
                context.SaveChanges();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var request = HttpContext.Request;
            var imgFile = request.Form.Files[0];
            var productName = request.Form["Name"];
            var productDesc = request.Form["Description"];
            var productPrice = request.Form["Price"];
            var productDate = request.Form["Date"];
            var sellerId = request.Form["SellerID"];

            if (imgFile.Length > 0)
            {
                var path = Path.Combine(filePath, imgFile.FileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imgFile.CopyToAsync(fileStream);
                }

                var product = new Product { Name = productName, Description = productDesc, InitialPrice = Int32.Parse(productPrice), Image = imgFile.FileName, SellerID = Int32.Parse(sellerId), IsActive = true, EndBid = DateTime.Parse(productDate) };
                context.Products.Add(product);
                context.SaveChanges();
                
            }
            else
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}