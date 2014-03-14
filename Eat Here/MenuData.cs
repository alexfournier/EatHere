using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Eat_Here
{

    public class MenuData
    {
        public static MenuResponse menuJsonData = null;
        public CategoryItem selectedItem { get; set; }

        public MenuData()
        {
            LoadMenuDataLocal();
        }

        [DataContract]
        public class MenuResponse
        {

            [DataMember]
            public string Menu { get; set; }

            [DataMember]
            public string Website { get; set; }

            [DataMember]
            public string Timestamp { get; set; }

            [DataMember]
            public ObservableCollection<CategoryItem> Categories { get; set; }
        }

        public class CategoryItem
        {
            [DataMember]
            public string Title { get; set; }

            [DataMember]
            public string img { get; set; }

            [DataMember]
            public string Desc { get; set; }

            [DataMember]
            public ObservableCollection<SubMenuItem> Items { get; set; }
        }

        public class SubMenuItem
        {
            [DataMember]
            public string Title { get; set; }

            [DataMember]
            public string img { get; set; }

            [DataMember]
            public string Desc { get; set; }

            [DataMember]
            public string Price { get; set; }
        }

        public void LoadMenuDataLocal()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                // network is NOT ok
                MessageBox.Show("Netowrk error. Displaying default menu");

                /*  LOCAL JSON LOADING */
                using (StreamReader r = new StreamReader("Assets/resources.json"))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json))
                    {
                        menuJsonData = JsonConvert.DeserializeObject<MenuResponse>(json);
                    }
                }
            }
            else
            {
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
                webClient.DownloadStringAsync(new Uri("http://alexfournier.netne.net/resources.json"));
                return;
            }
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var frame = App.Current.RootVisual as PhoneApplicationFrame;
            var container = frame.Content as welcome;

            if (e.Error == null) // ensure we have data
            {
                string json = e.Result;
                if (!string.IsNullOrEmpty(json))
                {
                    menuJsonData = JsonConvert.DeserializeObject<MenuResponse>(json);
                    frame = App.Current.RootVisual as PhoneApplicationFrame;
                    container = frame.Content as welcome;
                    container.longList.ItemsSource = menuJsonData.Categories;
                }
            }
            else
            {
                MessageBox.Show("Server error. Could not load menu (JSON ERROR). Try again");
            }
        }
    }
}

