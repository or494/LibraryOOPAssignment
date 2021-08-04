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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryOOPAssignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmployeeNavigationPage : Page
    {
        static List<AbstractItem> items = LibrarySystem._library.GetCurrentBorrowedItems().Concat(LibrarySystem._library.GetUnborowedItems()).ToList();
        public EmployeeNavigationPage()
        {
            this.InitializeComponent();
            items.Sort(new IcompareItemByName());
            UserName.Text = LibrarySystem._userManager.GetLoggedUser().FirstName + " " + LibrarySystem._userManager.GetLoggedUser().LastName;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestBox box = sender as AutoSuggestBox;
            List<string> list = new List<string>();
            if(args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (box.Text == "") sender.ItemsSource = null;
                else
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].Name.Contains(box.Text))
                            list.Add(items[i].Name);
                    }
                    sender.ItemsSource = list;
                }
            }
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            switch (((NavigationViewItem)args.SelectedItem).Name.ToString())
            {
                case "menu":
                    Page.Navigate(typeof(MenuPage));
                    break;

                case "addItems":
                    Page.Navigate(typeof(AddItemPage));
                    break;

                case "library":
                    Page.Navigate(typeof(AllItemsPage));
                    break;

                case "Reports":
                    Page.Navigate(typeof(ReportsPage));
                    break;

                case "Discounts":
                    Page.Navigate(typeof(DiscountsPage));
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
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            for(int i = 0; i < items.Count; i++)
            {
                if(args.SelectedItem.ToString() == items[i].Name)
                {
                    NavigationHelpClass.PreviousPage = this.GetType();
                    if (items[i].GetType() == typeof(Book))
                        Page.Navigate(typeof(BookItemViewPage), items[i]);
                    else if (items[i].GetType() == typeof(Journal))
                        Page.Navigate(typeof(JournaltemViewPage), items[i]);
                }
            }
        }
    }
}
