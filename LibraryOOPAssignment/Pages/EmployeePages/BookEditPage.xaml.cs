using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class BookEditPage : Page
    {
        Book item;

        public BookEditPage()
        {
            this.InitializeComponent();
            for (int i = 0; i < Enum.GetNames(typeof(CategoryName)).Length; i++)
            {
                CategoryComboBox.Items.Add(Enum.GetNames(typeof(CategoryName))[i]);
            }
            CategoryComboBox.SelectedIndex = 0;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            item = e.Parameter as Book;

            BookNameInput.Text = item.Name;
            AuthorInput.Text = item.Author;
            PublisherInput.Text = item.Publisher;
            if (item.Edition == -1)
                BookEditionInput.Text = "";
            else
                BookEditionInput.Text = item.Edition.ToString();
            SummaryInput.Text = item.Summary;
            PublishingYearInput.Text = item.PrintYear.Year.ToString();
            CategoryComboBox.SelectedIndex = (int)item.Category;
            PriceInput.Text = item.Price.ToString();
        }
        private void Letters_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsLetter(c) && !char.IsPunctuation(c));
        }
        private void Numbers_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
        private async void BookEditBtn_Click(object sender, RoutedEventArgs e)
        {
            BookNameError.Visibility = Visibility.Collapsed;
            BookAuthorError.Visibility = Visibility.Collapsed;
            BookPublisherError.Visibility = Visibility.Collapsed;
            BookYearError.Visibility = Visibility.Collapsed;
            BookPriceError.Visibility = Visibility.Collapsed;

            if (BookNameInput.Text.Length > 2 && AuthorInput.Text.Length > 2 && PublisherInput.Text.Length > 2 && PublishingYearInput.Text.Length == 4 &&
                PriceInput.Text.Length >= 1)
            {
                try
                {
                    LibrarySystem._library.EditBook(item.ISBN, BookNameInput.Text, (CategoryName)CategoryComboBox.SelectedIndex, PublisherInput.Text, float.Parse(PriceInput.Text),
                        BookEditionInput.Text != "" ? int.Parse(BookEditionInput.Text) : -1, new DateTime(int.Parse(PublishingYearInput.Text), 1, 1), AuthorInput.Text,
                        SummaryInput.Text != "" ? SummaryInput.Text : "", item.ImgName);
                    MessageDialog msg = new MessageDialog("Changes have been saved succesfully...");
                    await msg.ShowAsync();
                }
                catch (Exception)
                {
                    MessageDialog msg = new MessageDialog("Something went wrong...");
                    await msg.ShowAsync();
                }
            }
            else
            {
                if (BookNameInput.Text.Length <= 2)
                    BookNameError.Visibility = Visibility.Visible;

                if (AuthorInput.Text.Length <= 2)
                    BookAuthorError.Visibility = Visibility.Visible;

                if (PublisherInput.Text.Length <= 2)
                    BookPublisherError.Visibility = Visibility.Visible;

                if (PublishingYearInput.Text.Length != 4)
                    BookYearError.Visibility = Visibility.Visible;

                if (PriceInput.Text.Length < 1)
                    BookPriceError.Visibility = Visibility.Visible;
            }
        }
        private void Backbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(NavigationHelpClass.PreviousPage);
        }
    }
}
