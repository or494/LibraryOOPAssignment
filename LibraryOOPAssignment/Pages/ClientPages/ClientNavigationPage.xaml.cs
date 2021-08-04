using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LibraryOOPAssignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClientNavigationPage : Page
    {
        public ClientNavigationPage()
        {
            this.InitializeComponent();
            UserName.Text = LibrarySystem._userManager.GetLoggedUser().FirstName + " " + LibrarySystem._userManager.GetLoggedUser().LastName;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
         {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
            }
         }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            switch (((NavigationViewItem)args.SelectedItem).Name.ToString())
            {
                case "menu":
                    Page.Navigate(typeof(MenuPage));
                    break;

                case "library":
                    Page.Navigate(typeof(AllItemsPage));
                    break;

                case "myItems":
                    Page.Navigate(typeof(ClientItemsPage));
                    break;

                case "RecentlyViewed":
                    Page.Navigate(typeof(RecentlyItemsViewed));
                    break;

                case "logOut":
                    LibrarySystem._userManager.LogOut();
                    Frame.Navigate(typeof(LoginPage));
                    break;
            }
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationView nav = sender as NavigationView;
            nav.IsPaneOpen = false;
            menu.IsSelected = true;
            Page.Navigate(typeof(MenuPage));
        }
    }
}
