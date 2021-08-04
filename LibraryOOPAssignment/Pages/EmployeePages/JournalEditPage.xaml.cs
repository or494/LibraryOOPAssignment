using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class JournalEditPage : Page
    {
        Journal item;
        public JournalEditPage()
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

            item = e.Parameter as Journal;

            JournalNameInput.Text = item.Name;
            PublisherInput.Text = item.Publisher;
            if (item.Edition == -1)
                JournalEditionInput.Text = "";
            else
                JournalEditionInput.Text = item.Edition.ToString();
            PublishingDate.Date = item.PrintDate;
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
        private async void JournalEditBtn_Click(object sender, RoutedEventArgs e)
        {
            JournalNameError.Visibility = Visibility.Collapsed;
            JournalPublisherError.Visibility = Visibility.Collapsed;
            JournalYearError.Visibility = Visibility.Collapsed;
            JournalPriceError.Visibility = Visibility.Collapsed;

            if (JournalNameInput.Text.Length > 2 && PublisherInput.Text.Length > 2 && PublishingDate.Date != null &&
                PriceInput.Text.Length >= 1)
            {
                try
                {
                    LibrarySystem._library.EditJournal(item.ISBN, JournalNameInput.Text, (CategoryName)CategoryComboBox.SelectedIndex, PublisherInput.Text, float.Parse(PriceInput.Text),
                        JournalEditionInput.Text != "" ? int.Parse(JournalEditionInput.Text) : -1, PublishingDate.Date.DateTime, PublishingDate.Date.DateTime, item.ImgName);
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
                if (JournalNameInput.Text.Length <= 2)
                    JournalNameError.Visibility = Visibility.Visible;

                if (PublisherInput.Text.Length <= 2)
                    JournalPublisherError.Visibility = Visibility.Visible;

                if (PublishingDate.Date == null)
                    JournalYearError.Visibility = Visibility.Visible;

                if (PriceInput.Text.Length < 1)
                    JournalPriceError.Visibility = Visibility.Visible;
            }
        }
        private void Backbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(NavigationHelpClass.PreviousPage);
        }
    }
}
