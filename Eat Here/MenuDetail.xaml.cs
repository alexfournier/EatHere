using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Eat_Here
{
    public partial class MenuDetail : PhoneApplicationPage
    {
        public MenuDetail()
        {
            InitializeComponent();
            menuItemList.ItemsSource = welcome.menuData.selectedItem.Items;
            detailsTitle.Text = welcome.menuData.selectedItem.Title;
        }
    }
}