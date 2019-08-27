using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AuctionUsers
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public HttpClient client { get; set; }
        private string baseUrl = "https://localhost:44304/api/Client";
        public UserPage()
        {
            InitializeComponent();
            client = new HttpClient();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage resp = await client.GetAsync(baseUrl + "/" + IdVal.Text);

            if (resp.IsSuccessStatusCode)
            {
                var json = await resp.Content.ReadAsStringAsync();
                JArray jArr = (JArray)JsonConvert.DeserializeObject(json);
                if (json != "")
                {

                    if (jArr[2].ToString() == "Seller")
                    {
                        SellerPage sellerPage = new SellerPage(Int32.Parse(jArr[0].ToString()),jArr[1].ToString());
                        this.NavigationService.Navigate(sellerPage);
                    }
                    if (jArr[2].ToString() == "Bidder")
                    {
                        BidderPage bidderPage = new BidderPage(Int32.Parse(jArr[0].ToString()), jArr[1].ToString());
                        this.NavigationService.Navigate(bidderPage);
                    }
                }
            }
        }
    }
}
