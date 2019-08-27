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
using System.IO;
using System.Reflection;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AuctionUsers
{
    /// <summary>
    /// Interaction logic for SellerPage.xaml
    /// </summary>
    public partial class SellerPage : Page
    {
        public HttpClient client { get; set; }
        private string baseUrl = "https://localhost:44304/api/Client";
        string path = "";
        List<string> PicturesList = new List<string>();
        int SellerId;
        string SellerName;

        public SellerPage(int id, string name)
        {
            path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = Path.Combine(path,"Pictures");
            client = new HttpClient();
            InitializeComponent();
            SellerId = id;
            SellerName = name;
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PicturesList = Directory.GetFiles(path).ToList();

                cmbImageList.ItemsSource = PicturesList;
                cmbImageList.SelectedIndex = 0;
                //cmbImageList.Text = "Choose";
            }
            catch(Exception ex)
            {

            }
        }

        private void CmbImageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private async void UpdProduct_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxDesc.Text == "" || txtBoxName.Text == "" || txtBoxPrice.Text == "" || cmbImageList.Text == "" || datePicker.Text == "" || txtBoxTime.Text == "")
            {
                return;
            }
            else
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                byte[] prName = Encoding.ASCII.GetBytes(txtBoxName.Text);
                ByteArrayContent names = new ByteArrayContent(prName);

                byte[] prDesc = Encoding.ASCII.GetBytes(txtBoxDesc.Text);
                ByteArrayContent desc = new ByteArrayContent(prDesc);

                byte[] prPrice = Encoding.ASCII.GetBytes(txtBoxPrice.Text);
                ByteArrayContent prices = new ByteArrayContent(prPrice);

                DateTime endBit = DateTime.Parse(datePicker.Text +" "+ txtBoxTime.Text); //validate this
                byte[] prDate = Encoding.ASCII.GetBytes(endBit.ToString());
                ByteArrayContent date = new ByteArrayContent(prDate);

                byte[] seID = Encoding.ASCII.GetBytes(SellerId.ToString());
                ByteArrayContent sellerid = new ByteArrayContent(seID);
                
                byte[] prImage = File.ReadAllBytes(cmbImageList.Text);
                ByteArrayContent imageContent = new ByteArrayContent(prImage);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                string[] splits = cmbImageList.Text.Split('\\');

                multiContent.Add(imageContent, "Image", splits[splits.Count() - 1]);
                multiContent.Add(names,"Name");
                multiContent.Add(desc,"Description");
                multiContent.Add(prices,"Price");
                multiContent.Add(date, "Date");
                multiContent.Add(sellerid,"SellerID");

                await client.PostAsync(baseUrl,multiContent);

                CleanUp();
            }

            
        }

        public void CleanUp()
        {
            txtBoxDesc.Text = "";
            txtBoxName.Text = "";
            txtBoxPrice.Text = "";
            cmbImageList.Text = "";
            datePicker.Text = "";
            txtBoxTime.Text = "";
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            SellerHistory sellerHistory = new SellerHistory(SellerId);
            this.NavigationService.Navigate(sellerHistory);
        }
    }
}
