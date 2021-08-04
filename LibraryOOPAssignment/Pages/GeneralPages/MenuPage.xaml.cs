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
    public sealed partial class MenuPage : Page
    {
        List<List<AbstractItem>> categoryLists;
        List<AbstractItem> items = new List<AbstractItem>();
        List<Button> buttons = new List<Button>();
        IComparer<AbstractItem> comparer = new IcompareItemByName();
        static int colorsCnt = 0;
        Color[] colors = new Color[7];

        public MenuPage()
        {
            this.InitializeComponent();
            colors[0] = Color.FromArgb(70, 102, 138, 255);
            colors[1] = Color.FromArgb(70, 255, 212, 128);
            colors[2] = Color.FromArgb(70, 0, 153, 153);
            colors[3] = Color.FromArgb(70, 204, 136, 0);
            colors[4] = Color.FromArgb(70, 255, 51, 204);
            colors[5] = Color.FromArgb(70, 255, 77, 77);
            colors[6] = Color.FromArgb(70, 82, 122, 122);
            CreateCategoryList();
            SortComboBox.SelectedIndex = 0;
        }

        private void CreateCategoryList()
        {
            categoryLists = new List<List<AbstractItem>>();
            for (int i = 0; i < Enum.GetNames(typeof(CategoryName)).Length; i++)
            {
                List<AbstractItem> categoryItemsList = new List<AbstractItem>();
                for (int j = 0; j < LibrarySystem._library.GetUnborowedItems().Count; j++)
                {
                    AbstractItem item = LibrarySystem._library.GetUnborowedItems()[j];
                    CategoryName name = item.Category;
                    if (name.ToString() == Enum.GetNames(typeof(CategoryName))[i].ToString())
                        categoryItemsList.Add(item);
                }
                for (int j = 0; j < LibrarySystem._library.GetCurrentBorrowedItems().Count; j++)
                {
                    AbstractItem item = LibrarySystem._library.GetCurrentBorrowedItems()[j];
                    CategoryName name = item.Category;
                    if (name.ToString() == Enum.GetNames(typeof(CategoryName))[i].ToString())
                        categoryItemsList.Add(item);
                }
                categoryLists.Add(categoryItemsList);
            }
            for (int i = 0; i < categoryLists.Count; i++)
            {
                categoryLists[i].Sort(comparer);
            }
        }

        private void CreatePage()
        {
            for (int i = 0; i < Enum.GetNames(typeof(CategoryName)).Length; i++)
            {
                if (categoryLists[i].Count > 0)
                {
                    AddCategoryTemplate(Enum.GetNames(typeof(CategoryName))[i].ToString(), out GridView gridView, stackPanelAll);
                    for (int j = 0; j < categoryLists[i].Count && j < 4; j++)
                    {
                        CreateButton(categoryLists[i][j], gridView);
                    }
                }
            }
            for (int i = 0; i < Enum.GetNames(typeof(CategoryName)).Length; i++)
            {
                GridView gridView = null;
                bool isTemplateAlreadyExist = false;
                if (categoryLists[i].Count > 0)
                {
                    for (int j = 0; j < categoryLists[i].Count && j < 4; j++)
                    {
                        if (categoryLists[i][j] as Book != null)
                        {
                            if (!isTemplateAlreadyExist)
                            AddCategoryTemplate(Enum.GetNames(typeof(CategoryName))[i].ToString(), out gridView, stackPanelBooks);
                            CreateButton(categoryLists[i][j], gridView);
                            isTemplateAlreadyExist = true;
                        }
                    }
                }
            }
            for (int i = 0; i < Enum.GetNames(typeof(CategoryName)).Length; i++)
            {
                GridView gridView = null;
                bool isTemplateAlreadyExist = false;
                if (categoryLists[i].Count > 0)
                {
                    for (int j = 0; j < categoryLists[i].Count && j < 4; j++)
                    {
                        if (categoryLists[i][j] as Journal != null)
                        {
                            if (!isTemplateAlreadyExist)
                                AddCategoryTemplate(Enum.GetNames(typeof(CategoryName))[i].ToString(), out gridView, stackPanelJournals);
                            CreateButton(categoryLists[i][j], gridView);
                            isTemplateAlreadyExist = true;
                        }
                    }
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
            items.Add(item);
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

        private void AddCategoryTemplate(string categoryHeader, out GridView gridview, StackPanel panel)
        {
            Grid headerContainer = new Grid();
            headerContainer.Height = 70;
            headerContainer.Width = 1500;
            headerContainer.Background = new SolidColorBrush(colors[colorsCnt]);
            headerContainer.ColumnDefinitions.Add(new ColumnDefinition());
            headerContainer.ColumnDefinitions.Add(new ColumnDefinition());

            TextBlock header = new TextBlock();
            header.Margin = new Thickness(0, 0, 0, 10);
            header.FontSize = 50;
            header.Foreground = new SolidColorBrush(Colors.White);
            header.Text = categoryHeader;
            headerContainer.Children.Add(header);

            HyperlinkButton hyperlink = new HyperlinkButton();
            hyperlink.Content = "Click for more";
            Grid.SetColumn(hyperlink, 1);
            hyperlink.VerticalAlignment = VerticalAlignment.Center;
            hyperlink.HorizontalAlignment = HorizontalAlignment.Right;
            hyperlink.Margin = new Thickness(0, 0, 50, 0);
            hyperlink.Click += Hyperlink_Click;
            headerContainer.Children.Add(hyperlink);

            gridview = new GridView();
            gridview.Background = new SolidColorBrush(colors[colorsCnt]);
            gridview.IsItemClickEnabled = false;

            panel.Children.Add(headerContainer);
            panel.Children.Add(gridview);
            colorsCnt = (colorsCnt + 1) % colors.Length;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AllItemsPage));
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetPage();
            ComboBox tmp = sender as ComboBox;
            if (tmp.SelectedIndex == 0)
                comparer = new IcompareItemByName();
            else if (tmp.SelectedIndex == 1)
                comparer = new IcompareItemLowerByPrice();
            else if (tmp.SelectedIndex == 2)
                comparer = new IcompareItemByAddingDate();
            for (int i = 0; i < categoryLists.Count; i++)
            {
                categoryLists[i].Sort(comparer);
            }
            CreatePage();
        }

        private void ResetPage()
        {
            int cnt = stackPanelAll.Children.Count;
            for (int i = 0; i < cnt; i++)
            {
                stackPanelAll.Children.RemoveAt(0);
            }
            cnt = stackPanelBooks.Children.Count;
            for (int i = 0; i < cnt; i++)
            {
                stackPanelBooks.Children.RemoveAt(0);
            }
            cnt = stackPanelJournals.Children.Count;
            for (int i = 0; i < cnt; i++)
            {
                stackPanelJournals.Children.RemoveAt(0);
            }
        }
    }
}
