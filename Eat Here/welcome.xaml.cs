using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Eat_Here
{
    public partial class welcome : PhoneApplicationPage
    {

        public static MenuData menuData;
        public static MenuDetail menuItemList;

        public welcome()
        {
            InitializeComponent();
            menuData = new MenuData();
            DataContext = MenuData.menuJsonData;
        }


        private void longListTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MenuData.CategoryItem item = ((FrameworkElement)e.OriginalSource).DataContext as MenuData.CategoryItem;
            menuData.selectedItem = item; // save the category item selected here
            if (item != null) // if fast-clicking, it is possible to get here with nothing selected.  Ignore
                NavigationService.Navigate(new Uri("/menuDetail.xaml", UriKind.Relative));
        }
        
         private void locationTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/map.xaml", UriKind.Relative));
        }

        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void externalLink(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.artisinbakery.com");
            webBrowserTask.Show();
        }

        private void phoneTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhoneCallTask phoneTask = new PhoneCallTask();
            phoneTask.DisplayName = "Art Is In";
            phoneTask.PhoneNumber = "555-5555";
            phoneTask.Show();
        }

        private void emailTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Send Email?", "Message to support", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                // user respondid "ok"
                EmailComposeTask emailComposeTask = new EmailComposeTask();
                emailComposeTask.Subject = "Request for info";
                emailComposeTask.To = "artisin@bakery.com";
                emailComposeTask.Show();
            }
        }

        private void resTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SaveAppointmentTask SATask = new SaveAppointmentTask();

            SATask.Subject = "Book a Reservation";
            SATask.Location = "Art Is In - 250 City Centre Ave";
            SATask.Details = "extra details";
            SATask.Reminder = Reminder.OneHour;
            SATask.AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.Busy;
            DateTime today = DateTime.Now;
            DateTime soon = today.AddHours(2);
            SATask.Show();

            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault();
            if (TileToFind != null)
            {
                TileToFind.Update(cycleTile);
            }
        }

        CycleTileData cycleTile = new CycleTileData()
        {
            Title = "Art Is In - NEW RESERVATION MADE",
            Count = 0,
            SmallBackgroundImage = new Uri("/images/logoArt.png", UriKind.Relative),
            CycleImages = new Uri[]
               {
                  new Uri("/images/reservation.png", UriKind.Relative),
                 
               }
        };

        private void aboutPageClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/aboutPage.xaml", UriKind.Relative));
        }
    }
}