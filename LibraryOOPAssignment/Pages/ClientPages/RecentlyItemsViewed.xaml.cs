using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryOOPAssignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecentlyItemsViewed : Page
    {
        List<AbstractItem> itemsViewed = new List<AbstractItem>();
        List<Button> buttons = new List<Button>();

        public RecentlyItemsViewed()
        {
            this.InitializeComponent();
            Customer customer = LibrarySystem._userManager.GetLoggedUser() as Customer;
            List<AbstractItem> viewedItems = LibrarySystem._library.GetUsersViewedItems();
            for (int i = 0; i < viewedItems.Count; i++)
            {
                screen.Items.Remove(DefaultTXTBlock);
                bool isAlreadyExist = false;
                for (int j = 0; j < itemsViewed.Count; j++)
                {
                    if (viewedItems[i].ISBN == itemsViewed[j].ISBN)
                    {
                        isAlreadyExist = true;
                        break;
                    }
                }
                if (!isAlreadyExist && LibrarySystem._library.GetItem(viewedItems[i].ISBN) != null)
                {
                    itemsViewed.Add(LibrarySystem._library.GetItem(viewedItems[i].ISBN));
                    CreateButton(LibrarySystem._library.GetItem(viewedItems[i].ISBN), screen);
                }

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
                    itemViewing = itemsViewed[i];
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
    }
}