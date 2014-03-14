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
    public partial class aboutPage : PhoneApplicationPage
    {
        public aboutPage()
        {
            InitializeComponent();
        }

        private void Email_Button_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "Request for support";
            emailComposeTask.To = "four0126@algonquinlive.com";
            emailComposeTask.Show();
        }

        private void Url_Button_Tap(object sender, RoutedEventArgs e)
        {
            WebBrowserTask wbt = new Microsoft.Phone.Tasks.WebBrowserTask();
            wbt.Uri = new Uri("http://alexfournier.netne.net/");
            wbt.Show();
        }
        private void phoneTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhoneCallTask phoneTask = new PhoneCallTask();
            phoneTask.DisplayName = "Art Is In";
            phoneTask.PhoneNumber = "555-5555";
            phoneTask.Show();
        }
    }
}