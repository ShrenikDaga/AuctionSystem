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
    /// Interaction logic for SellerHistory.xaml
    /// </summary>
    /// 

    public class ProductItem
    {
        public string name { get; set; }
        public BitmapImage image { get; set; }
        public int id { get; set; }
        public DateTime? endDate {get;set;}
    }

    public class ProductDetails
    {
        public string sellerId { get; set; }
        public string bidderName { get; set; }
        public int bidAmount { get; set; }
        public bool isActive { get; set; }
    }

    public partial class SellerHistory : Page
    {
        public HttpClient client { get; set; }
        private string baseUrl = "https://localhost:44304/api/Client";
        int SellerId;
        public ObservableCollection<ProductItem> Products { get; set; }
        public ObservableCollection<ProductDetails> ProductDetails { get; set; }

        public SellerHistory(int id)
        {
            InitializeComponent();
            client = new HttpClient();
            SellerId = id;
            GetHistory();
        }

        private async Task GetHistory()
        {
            var urlExt = SellerId.ToString() + "/1";
            HttpResponseMessage resp = await client.GetAsync(baseUrl + "/" + urlExt);
            if (resp.IsSuccessStatusCode)
            {
                var json = await resp.Content.ReadAsStringAsync();
                JArray jArr = (JArray)JsonConvert.DeserializeObject(json);
                Products = new ObservableCollection<ProductItem>();

                foreach (var item in jArr)
                {
                    var encoding64b = item["image"].ToString().Split(',');
                    byte[] b = Convert.FromBase64String(encoding64b[1]);
                    var source = new BitmapImage();
                    source.BeginInit();
                    source.StreamSource = new MemoryStream(b);
                    source.EndInit();

                    Products.Add(new ProductItem() { id = Int32.Parse(item["product"]["productID"].ToString()), name = item["product"]["name"].ToString(), image = source});
                    
                }
                FieldsListBox.ItemsSource = Products;
                
            }
            

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                btn.Tag.ToString();
            }

            HttpResponseMessage resp = await client.GetAsync(baseUrl + "/" + btn.Tag.ToString() + "/0/0");
            if (resp.IsSuccessStatusCode)
            {
                var json = await resp.Content.ReadAsStringAsync();
                JArray jArr = (JArray)JsonConvert.DeserializeObject(json);

                ProductDetails = new ObservableCollection<ProductDetails>();

                foreach (var item in jArr)
                {
                    ProductDetails.Add(new ProductDetails() { sellerId = item["bidderID"].ToString(),bidderName=item["bidderName"].ToString(),bidAmount = Int32.Parse(item["bidAmount"].ToString())});
                }
                var ProductDetails2 = ProductDetails.OrderByDescending(selection => selection.bidAmount);
                DetailsListBox.ItemsSource = ProductDetails2;
            }
        }
    }
}
