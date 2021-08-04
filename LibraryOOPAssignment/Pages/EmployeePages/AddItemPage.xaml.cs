using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class AddItemPage : Page
    {
        static StorageFile file;
        static string pictureName = "";
        public AddItemPage()
        {
            this.InitializeComponent();
            for(int i = 0; i < Enum.GetNames(typeof(CategoryName)).Length; i++)
            {
                CategoryComboBox.Items.Add(Enum.GetNames(typeof(CategoryName))[i]);
            }
            CategoryComboBox.SelectedIndex = 0;
            for (int i = 0; i < Enum.GetNames(typeof(CategoryName)).Length; i++)
            {
                JournalCategoryComboBox.Items.Add(Enum.GetNames(typeof(CategoryName))[i]);
            }
            JournalCategoryComboBox.SelectedIndex = 0;

        }

        private async void BookAddBtn_Click(object sender, RoutedEventArgs e)
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
                    if (ImagePathInput.Text != "")
                    {
                        CopyPictureToLocalStorageAsync();
                    }
                    else
                        pictureName = "";

                    LibrarySystem._library.AddBook(BookNameInput.Text, AuthorInput.Text, (CategoryName)CategoryComboBox.SelectedIndex, PublisherInput.Text,
                        float.Parse(PriceInput.Text), SummaryInput.Text != "" ? SummaryInput.Text : "", BookEditionInput.Text != "" ? int.Parse(BookEditionInput.Text) : -1,
                        new DateTime(int.Parse(PublishingYearInput.Text), 1, 1),
                        pictureName);
                    MessageDialog msg = new MessageDialog("Your book Has been saved succesfully...");
                    await msg.ShowAsync();
                }
                catch (Exception exception) 
                {
                    Trace.WriteLine(exception.Message);
                    MessageDialog msg = new MessageDialog("Something went wrong...");
                    await msg.ShowAsync();
                    ImagePathInput.BeforeTextChanging -= ImageName_BeforeTextChanging;
                    ImagePathInput.Text = "";
                    pictureName = file.Name;
                }
            }
            else
            {
                if(BookNameInput.Text.Length <= 2)
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
        private async void JournalAddBtn_Click(object sender, RoutedEventArgs e)
        {
            JournalNameError.Visibility = Visibility.Collapsed;
            JournalPublisherError.Visibility = Visibility.Collapsed;
            JournalDateError.Visibility = Visibility.Collapsed;
            JournalPriceError.Visibility = Visibility.Collapsed;

            if (JournalNameInput.Text.Length > 2 && JournalPublisherInput.Text.Length > 2 && JournalPubishingDate.Date.Year > 1602 &&
                JournalPriceInput.Text.Length >= 1)
            {

                try
                {
                    if (JournalImagePathInput.Text != "")
                    {
                        CopyPictureToLocalStorageAsync();
                    }
                    else
                        pictureName = "";

                    LibrarySystem._library.AddJournal(JournalNameInput.Text, (CategoryName)JournalCategoryComboBox.SelectedIndex, JournalPublisherInput.Text,
                        float.Parse(JournalPriceInput.Text), JournalEditionInput.Text != null && JournalEditionInput.Text != "" ? int.Parse(JournalEditionInput.Text) : 0,
                        new DateTime(JournalPubishingDate.Date.Year, 1, 1), new DateTime(JournalPubishingDate.Date.Year, JournalPubishingDate.Date.Month,
                        JournalPubishingDate.Date.Day), pictureName);
                    MessageDialog msg = new MessageDialog("Your Journal Has been saved succesfully...");
                    await msg.ShowAsync();
                }
                catch
                {
                    MessageDialog msg = new MessageDialog("Something went wrong...");
                    await msg.ShowAsync();
                }
            }

            else
            {
                if (JournalNameInput.Text.Length <= 2)
                    JournalNameError.Visibility = Visibility.Visible;

                if (JournalPublisherInput.Text.Length <= 2)
                    JournalPublisherError.Visibility = Visibility.Visible;

                if (JournalPubishingDate.Date.Year < 1602)
                    JournalDateError.Visibility = Visibility.Visible;

                if (JournalPriceInput.Text.Length <= 1)
                    JournalPriceError.Visibility = Visibility.Visible;
            }
        }
        private void Letters_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsLetter(c) && !char.IsSeparator(c) && !char.IsPunctuation(c));
        }
        private void ImageName_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => char.IsLetterOrDigit(c));
        }
        private void Numbers_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
        private async void BrowseJournalPicture_Click(object sender, RoutedEventArgs e)
        {
            ImagePathInput.Text = "";

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                JournalImagePathInput.Text = file.Path;
                pictureName = file.Name;
            }
            else
            {
                JournalImagePathInput.Text = "";
                pictureName = "";
            }
        }
        private async void BrowseBookPicture_Click(object sender, RoutedEventArgs e)
        {
            ImagePathInput.Text = "";

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                ImagePathInput.BeforeTextChanging -= ImageName_BeforeTextChanging;
                ImagePathInput.Text = file.Path;
                pictureName = file.Name;
                ImagePathInput.BeforeTextChanging += ImageName_BeforeTextChanging;
            }
            else
            {
                ImagePathInput.Text = "";
                pictureName = "";
            }
        }
        private async void CopyPictureToLocalStorageAsync()
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;
            StorageFolder folder;
            try
            {
                folder = await local.GetFolderAsync("Images");
            }
            catch (Exception)
            {
                folder = await local.CreateFolderAsync("Images");
            }
            try
            {
                await file.CopyAsync(folder);
            }
            catch
            {
                file = null;
            }
            file = null;
        }
    }
}
