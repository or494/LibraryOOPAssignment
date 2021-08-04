using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
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
    public sealed partial class DiscountsPage : Page
    {
        List<string> AuthorsList;
        List<string> PublishersList;
        List<string> YearsList;
        int btnCnt = 0;

        public DiscountsPage()
        {
            this.InitializeComponent();
            AuthorsList = LibrarySystem._library.GetAllAuthors();
            PublishersList = LibrarySystem._library.GetAllPublishers();
            YearsList = LibrarySystem._library.GetAllPublishingYears();
            foreach (string item in AuthorsList)
            {
                Authors.Items.Add(item);
            }
            foreach (string item in PublishersList)
            {
                Publishers.Items.Add(item);
            }
            foreach (string item in YearsList)
            {
                Years.Items.Add(item);
            }
            Subject.Items.Add("Authors");
            Subject.Items.Add("Publishers");
            Subject.Items.Add("Print year");
            Subject.SelectedIndex = 0;
            Authors.SelectedIndex = 0;
            Publishers.SelectedIndex = 0;
            Years.SelectedIndex = 0;
            foreach (Discount item in LibrarySystem._discountManager.GetDiscounts())
            {
                CreateTemplate(item);
            }
        }

        private void CreateTemplate(Discount item)
        {
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Colors.Azure);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinition colDef = new ColumnDefinition();
            colDef.Width = new GridLength(300);
            grid.ColumnDefinitions.Add(colDef);

            TextBlock txt = new TextBlock();
            if (item.Category == DiscountCategories.Author)
            {
                txt.Text = $"Discount of {item.DiscountPercent}% on all items of author {item.DiscountReasonName}";
            }
            else if (item.Category == DiscountCategories.Publisher)
            {
                txt.Text = $"Discount of {item.DiscountPercent}% on all items of publisher {item.DiscountReasonName}";
            }
            else if (item.Category == DiscountCategories.PublishingYear)
            {
                txt.Text = $"Discount of {item.DiscountPercent}% on all items published on {item.DiscountReasonName}";
            }
            txt.VerticalAlignment = VerticalAlignment.Center;
            txt.FontSize = 40;
            txt.Margin = new Thickness(30);
            grid.Children.Add(txt);

            Button btn = new Button();
            btn.Content = "End discount";
            Grid.SetColumn(btn, 1);
            btn.FontSize = 30;
            btn.Margin = new Thickness(50);
            btn.Click += EndDiscount_Click;
            btn.Tag = btnCnt;
            btnCnt++;
            grid.Children.Add(btn);

            Grid space = new Grid();
            space.Height = 20;

            DiscountsStack.Children.Add(grid);
            DiscountsStack.Children.Add(space);
        }

        private async void DiscountBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DiscountNumber.Text != "" && DiscountNumber.Text != "0") {
                if(Subject.SelectedIndex == 0)
                    LibrarySystem._discountManager.SetDiscount(DiscountCategories.Author, float.Parse(DiscountNumber.Text),Authors.SelectedItem.ToString());
                else if (Subject.SelectedIndex == 1)
                    LibrarySystem._discountManager.SetDiscount(DiscountCategories.Publisher, float.Parse(DiscountNumber.Text), Publishers.SelectedItem.ToString());
                else if(Subject.SelectedIndex == 2)
                    LibrarySystem._discountManager.SetDiscount(DiscountCategories.PublishingYear, float.Parse(DiscountNumber.Text), Years.SelectedItem.ToString());
                Frame.Navigate(typeof(DiscountsPage));
            }
            else
            {
                MessageDialog msg = new MessageDialog("Discount number is invalid...");
                await msg.ShowAsync();
            }
        }

        private void Subject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Authors.Visibility = Visibility.Collapsed;
            Publishers.Visibility = Visibility.Collapsed;
            Years.Visibility = Visibility.Collapsed;

            ComboBox tmp = sender as ComboBox;
            if (tmp.SelectedIndex == 0)
            {
                SubjectType.Text = "Author Name:";
                Authors.Visibility = Visibility.Visible;
            }
            else if (tmp.SelectedIndex == 1)
            {
                SubjectType.Text = "Publisher Name:";
                Publishers.Visibility = Visibility.Visible;
            }
            else if (tmp.SelectedIndex == 2)
            {
                SubjectType.Text = "Publishing year:";
                Years.Visibility = Visibility.Visible;
            }
        }

        private void DiscountNumber_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void EndDiscount_Click(object sender, RoutedEventArgs e)
        {
            Button tmp = sender as Button;
            LibrarySystem._discountManager.RemoveDiscount(LibrarySystem._discountManager.GetDiscounts()[(int)tmp.Tag]);
            Frame.Navigate(typeof(DiscountsPage));
        }
    }
}
