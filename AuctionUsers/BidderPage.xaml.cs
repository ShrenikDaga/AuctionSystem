using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AuctionUsers
{
    /// <summary>
    /// Interaction logic for BidderPage.xaml
    /// </summary>
    /// 

    public class Bids
    {
        public int BidderID;
        public int ProductID;
        public string BidderName;
        public int BidAmount;
        public DateTime BidDate;
    }
    
    public partial class BidderPage : Page
    {
        public HttpClient client { get; set; }
        private string baseUrl = "https://localhost:44304/api/Client";
        public ObservableCollection<ProductItem> Products { get; set; }
        public ObservableCollection<Bids> Bid { get; set; }
        public List<ProductItem> ProductsList;
        ProductItem tempProduct;

        int BidderId;
        string BidderName;
        public BidderPage(int id, string name)
        {
            InitializeComponent();
            client = new HttpClient();
            BidderId = id;
            BidderName = name;
            GetHistory();
        }

        private async Task GetHistory()
        {
            HttpResponseMessage resp = await client.GetAsync(baseUrl + "/0/0");
            if (resp.IsSuccessStatusCode)
            {
                var json = await resp.Content.ReadAsStringAsync();
                JArray jArr = (JArray)JsonConvert.DeserializeObject(json);
                Products = new ObservableCollection<ProductItem>();
                ProductsList = new List<ProductItem>();
                foreach (var item in jArr)
                {
                    if (item["product"]["isActive"].ToString() == false.ToString())
                    {
                        continue;
                    }
                    var encoding64b = item["image"].ToString().Split(',');
                    byte[] b = Convert.FromBase64String(encoding64b[1]);
                    var source = new BitmapImage();
                    source.BeginInit();
                    source.StreamSource = new MemoryStream(b);
                    source.EndInit();
                    ProductsList.Add(new ProductItem() { id = Int32.Parse(item["product"]["productID"].ToString()), name = item["product"]["name"].ToString(), image = source,endDate = DateTime.Parse(item["product"]["endBid"].ToString())  });
                    Products.Add(new ProductItem() { id = Int32.Parse(item["product"]["productID"].ToString()), name = item["product"]["name"].ToString(), image = source });
                    //Products.Add(new ProductItem() { id = Int32.Parse(item["product"]["productID"].ToString()), name = item["product"]["name"].ToString(), image = item["image"].ToString()});
                }
                FieldsListBox.ItemsSource = Products;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                btn.Tag.ToString();
            }
            //bidFor.Content += FieldsListBox.SelectedItem.id;
            tempProduct = ProductsList.Find(selection => selection.id == Int32.Parse(btn.Tag.ToString()));
            bidFor.Content = "Bid for: ";
            bidFor.Content += tempProduct.name;
            bidExpiry.Content = "Bid expries on: ";
            bidExpiry.Content += tempProduct.endDate.ToString()+"EST";
            postBidButton.IsHitTestVisible = true;
        }

        private async void PostBidButton_Click(object sender, RoutedEventArgs e)
        {
            var temp = bidFor.Content;
            var prdId = temp.ToString().Split(':');
            var newBid = new Bids() { BidAmount = Int32.Parse(bidAmount.Text), BidDate=DateTime.Parse(DateTime.Now.ToString("G")), BidderID=BidderId,BidderName=BidderName,ProductID=tempProduct.id};

            var myContent = JsonConvert.SerializeObject(newBid);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var result = client.PostAsync(baseUrl+ "/PostBid", byteContent);
        }
    }
}
