using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryOOPAssignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllItemsPage : Page
    {
        List<AbstractItem> items = new List<AbstractItem>();
        List<Button> buttons = new List<Button>();
        IComparer<AbstractItem> comparer = new IcompareItemByName();
        bool iSFirstLoad = true;

        public AllItemsPage()
        {
            this.InitializeComponent();
            category.Items.Add("All");
            for (int i = 0; i < Enum.GetNames(typeof(CategoryName)).Length; i++)
            {
                category.Items.Add(Enum.GetNames(typeof(CategoryName))[i]);
            }
            SortType.Items.Add("Name");
            SortType.Items.Add("Publisher");
            SortType.Items.Add("Price");
            SortType.Items.Add("Newest");
            category.SelectedIndex = 0;
        }

        private void OrganizeLists()
        {
            items = new List<AbstractItem>();
            if(category.SelectedIndex == 0)
            {
                for(int i = 0; i < LibrarySystem._library.GetUnborowedItems().Count; i++)
                {
                    items.Add(LibrarySystem._library.GetUnborowedItems()[i]);
                }
                for(int i = 0; i < LibrarySystem._library.GetCurrentBorrowedItems().Count; i++)
                {
                    items.Add(LibrarySystem._library.GetCurrentBorrowedItems()[i]);
                }
            }
            else
            {
                for (int i = 0; i < LibrarySystem._library.GetUnborowedItems().Count; i++)
                {
                    if((int)LibrarySystem._library.GetUnborowedItems()[i].Category == category.SelectedIndex - 1)
                        items.Add(LibrarySystem._library.GetUnborowedItems()[i]);
                }
                for (int i = 0; i < LibrarySystem._library.GetCurrentBorrowedItems().Count; i++)
                {
                    if ((int)LibrarySystem._library.GetCurrentBorrowedItems()[i].Category == category.SelectedIndex)
                        items.Add(LibrarySystem._library.GetCurrentBorrowedItems()[i]);
                }
            }
        }

        private void CreatePage()
        {
            for(int i = 0; i < items.Count; i++)
            {
                CreateButton(items[i], Container);
            }
        }

        private async void CreateButton(AbstractItem item, GridView gridView)
        {
            int height = 200;
            int width = 300;

            Button btn = new Button();
            btn.Height = height + 30;
            btn.Width = width + 30;
            btn.Padding = new Thickness(0, 0, 0, 0);
            btn.Margin = new Thickness(15);
            btn.Click += ItemClick;
            buttons.Add(btn);
            gridView.Items.Add(btn);

            Grid btnGrid = new Grid();
            btnGrid.Height = height; ;
            btnGrid.Width = width;
            btnGrid.ColumnDefinitions.Add(new ColumnDefinition());
            btnGrid.ColumnDefinitions.Add(new ColumnDefinition());

            Image img = new Image();
            if (item.ImgName == "")
                img.Source = new BitmapImage(new Uri("ms-appx:/Assets/EmptyBook.jpg"));
            else
            {
                StorageFolder local = ApplicationData.Current.LocalFolder;
                StorageFolder folder;
                try
                {
                    folder = await local.GetFolderAsync("Images");
                }
                catch (Exception e)
                {
                    folder = await local.CreateFolderAsync("Images");
                }
                Uri uri = new Uri($@"{folder.Path}\{item.ImgName}");
                BitmapImage bmi = new BitmapImage(uri);
                img.Source = bmi;
            }
            img.Height = height;
            img.Width = width / 2;
            Grid.SetColumn(img, 0);
            btnGrid.Children.Add(img);

            TextBlock txt = new TextBlock();
            txt.FontFamily = new FontFamily("Times New Roman");
            txt.FontSize = 18;
            txt.Height = height;
            txt.Width = width / 2 - 15;
            txt.Text = $"מוצר:{item.Name}\n\nהוצאה לאור: {item.Publisher}\n\nמחיר:{item.Price}$";
            txt.TextAlignment = TextAlignment.Right;
            txt.Foreground = new SolidColorBrush(Colors.White);
            txt.Margin = new Thickness(10, 20, 5, 20);
            txt.FontWeight = FontWeights.Bold;
            txt.TextWrapping = TextWrapping.Wrap;
            Grid.SetColumn(txt, 1);
            btnGrid.Children.Add(txt);

            btn.Content = btnGrid;

        }

        private void ItemClick(object sender, RoutedEventArgs e)
        {
            Button tmp = sender as Button;
            AbstractItem itemViewing = null;
            for (int i = 0; i < buttons.Count; i++)
            {
                if (tmp.Equals(buttons[i]))
                {
                    itemViewing = items[i];
                    break;
                }
            }
            Customer customer = LibrarySystem._userManager.GetLoggedUser() as Customer;
            if (customer != null)
                LibrarySystem._library.UserViewedItem(itemViewing);
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("forwardConnectedAnimation", tmp);
            NavigationHelpClass.PreviousPage = this.GetType();
            if (itemViewing as Book != null)
                Frame.Navigate(typeof(BookItemViewPage), itemViewing, new SuppressNavigationTransitionInfo());
            else if (itemViewing as Journal != null)
                Frame.Navigate(typeof(JournaltemViewPage), itemViewing, new SuppressNavigationTransitionInfo());
        }

        private async void SearchTool_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestBox box = sender as AutoSuggestBox;
            List<Grid> list = new List<Grid>();
            List<AbstractItem> SearchItems;
            sender.ItemsSource = null;
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                SearchItems = LibrarySystem._library.SearchFor(box.Text);
                
                for(int i = 0;i<5 && i < SearchItems.Count; i++)
                {
                    CreateSearchToolItem(SearchItems[i], list, i);
                }
                sender.ItemsSource = list;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Grid chosenGrid = args.SelectedItem as Grid;
            TextBlock chosenBookName = chosenGrid.Children[1] as TextBlock;
            AbstractItem chosenItem = null;
            for(int i = 0; i < items.Count; i++)
            {
                if (chosenBookName.Text.ToString() == items[i].Name)
                {
                    chosenItem = items[i];
                    break;
                }
            }
            if (chosenItem != null) {
                Customer customer = LibrarySystem._userManager.GetLoggedUser() as Customer;
                if (customer != null) LibrarySystem._library.UserViewedItem(chosenItem);
                NavigationHelpClass.PreviousPage = this.GetType();
                if (chosenItem as Book != null)
                    Frame.Navigate(typeof(BookItemViewPage),chosenItem);
                else if (chosenItem as Journal != null)
                    Frame.Navigate(typeof(JournaltemViewPage),chosenItem);
            }
        }

        private async void CreateSearchToolItem(AbstractItem item,List<Grid> list,int index)
        {
            AbstractItem matchItem = item;
            Grid SearchItemGrid = new Grid();
            list.Add(SearchItemGrid);
            SearchItemGrid.Height = 100;
            SearchItemGrid.Width = 10000;
            SearchItemGrid.Background = new SolidColorBrush(Colors.Black);
            ColumnDefinition columnDef = new ColumnDefinition();
            columnDef.Width = new GridLength(100);
            SearchItemGrid.ColumnDefinitions.Add(columnDef);
            SearchItemGrid.ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = new GridLength(65);
            SearchItemGrid.RowDefinitions.Add(rowDef);
            SearchItemGrid.RowDefinitions.Add(new RowDefinition());
            Image img = new Image();
            if (matchItem.ImgName == "")
                img.Source = new BitmapImage(new Uri("ms-appx:/Assets/EmptyBook.jpg"));
            else
            {
                StorageFolder local = ApplicationData.Current.LocalFolder;
                StorageFolder folder;
                try
                {
                    folder = await local.GetFolderAsync("Images");
                }
                catch (Exception e)
                {
                    folder = await local.CreateFolderAsync("Images");
                }
                Uri uri = new Uri($@"{folder.Path}\{matchItem.ImgName}");
                BitmapImage bmi = new BitmapImage(uri);
                img.Source = bmi;
            }
            img.Height = 100;
            img.Width = 80;
            Grid.SetColumn(img, 0);
            Grid.SetRowSpan(img, 2);
            TextBlock itemName = new TextBlock();
            itemName.Height = 30;
            itemName.Text = matchItem.Name;
            itemName.FontSize = 20;
            itemName.Foreground = new SolidColorBrush(Colors.White);
            itemName.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(itemName, 0);
            Grid.SetColumn(itemName, 1);

            TextBlock publisherName = new TextBlock();
            publisherName.Height = 30;
            publisherName.Text = item.GetType() == typeof(Book)?(item as Book).Author : "";
            publisherName.FontSize = 12;
            publisherName.Foreground = new SolidColorBrush(Colors.White);
            publisherName.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(publisherName, 1);
            Grid.SetColumn(publisherName, 1);

            SearchItemGrid.Children.Add(img);
            SearchItemGrid.Children.Add(itemName);
            SearchItemGrid.Children.Add(publisherName);


        }

        private void category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            items = new List<AbstractItem>();
            buttons = new List<Button>();
            OrganizeLists();
            if (iSFirstLoad)
            {
                iSFirstLoad = false;
                SortType.SelectedIndex = 0;
            }
            else
            {
                StackScreen.Children.Remove(Container);
                Container = new GridView();
                StackScreen.Children.Add(Container);
                items.Sort(comparer);
                CreatePage();
            }
        }

        private void SortType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttons = new List<Button>();
            ComboBox tmp = sender as ComboBox;
            if (tmp.SelectedItem.ToString() == "Name")
                comparer = new IcompareItemByName();
            else if (tmp.SelectedItem.ToString() == "Publisher")
                comparer = new IcompareItemByPublisher();
            else if (tmp.SelectedItem.ToString() == "Price")
                comparer = new IcompareItemLowerByPrice();
            else if (tmp.SelectedItem.ToString() == "Newest")
                comparer = new IcompareItemByAddingDate();

            items.Sort(comparer);
            StackScreen.Children.Remove(Container);
            Container = new GridView();
            StackScreen.Children.Add(Container);
            CreatePage();
        }
    }
}
