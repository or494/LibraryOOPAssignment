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
    public sealed partial class ClientItemsPage : Page
    {
        List<AbstractItem> items = new List<AbstractItem>();
        List<Button> buttons = new List<Button>();

        public ClientItemsPage()
        {
            this.InitializeComponent();
            Customer customer = LibrarySystem._userManager.GetLoggedUser() as Customer;
            List<AbstractItem> customerCurrentBorrowedItems = LibrarySystem._library.GetUserBorrowedItems(customer);
            List<AbstractItem> customerPastBorrowedItems = LibrarySystem._library.GetUserPastBorrowedItems(customer);

            for (int i = 0; i < customerCurrentBorrowedItems.Count; i++)
            {
                AbstractItem tmp = LibrarySystem._library.GetItem(customerCurrentBorrowedItems[i].ISBN);
                items.Add(tmp);
                if(items[i]!=null)
                    CreateButton(items[i], BorrowedItemsContainer);
            }
            for (int i = 0; i < customerPastBorrowedItems.Count; i++)
            {
                bool isAlreadyExist = false;
                for (int j = 0; j < items.Count; j++)
                {
                    if (items[j] != null && customerPastBorrowedItems[i].ISBN == items[j].ISBN)
                    {
                        isAlreadyExist = true;
                        break;
                    }
                }
                if (!isAlreadyExist&& LibrarySystem._library.GetItem(customerPastBorrowedItems[i].ISBN)!=null)
                {
                    items.Add(LibrarySystem._library.GetItem(customerPastBorrowedItems[i].ISBN));
                    CreateButton(LibrarySystem._library.GetItem(customerPastBorrowedItems[i].ISBN), PastBorrowedItemsContainer);
                }
            }
            bool isEmpty1 = false;
            if(BorrowedItemsContainer.Items.Count == 0)
            {
                BorrowedItemsStack.Visibility = Visibility.Collapsed;
                isEmpty1 = true;
                GeneralStack.Background = PastBorrowedItemsStack.Background;
            }
            bool isEmpty2 = false;
            if (PastBorrowedItemsContainer.Items.Count == 0)
            {
                PastBorrowedItemsStack.Visibility = Visibility.Collapsed;
                isEmpty2 = true;
                GeneralStack.Background = BorrowedItemsStack.Background;
            }
            if (isEmpty1 && isEmpty2) DefaultMessage.Visibility = Visibility.Visible;
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
            //if (customer != null)
            //    customer.ViewItem(itemViewing);
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("forwardConnectedAnimation", tmp);
            NavigationHelpClass.PreviousPage = this.GetType();
            if (itemViewing as Book != null)
                Frame.Navigate(typeof(BookItemViewPage), itemViewing, new SuppressNavigationTransitionInfo());
            else if (itemViewing as Journal != null)
                Frame.Navigate(typeof(JournaltemViewPage), itemViewing, new SuppressNavigationTransitionInfo());
        }
    }
}
