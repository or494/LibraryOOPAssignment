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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryOOPAssignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookItemViewPage : Page
    {
        Book item;
        public BookItemViewPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("forwardConnectedAnimation");
            if (anim != null)
            {
                anim.TryStart(screen);
            }

            item = e.Parameter as Book;
            if (item.ImgName == "" || item.ImgName == null)
                Picture.Source = new BitmapImage(new Uri("ms-appx:/Assets/EmptyBook.jpg"));
            else
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
                Uri uri = new Uri($@"{folder.Path}\{item.ImgName}");
                BitmapImage bmi = new BitmapImage(uri);
                Picture.Source = bmi;
            }

            Customer tmp =  LibrarySystem._userManager.GetLoggedUser() as Customer;
            if(tmp != null)
            {
                bool check = false;
                Borrow.Visibility = Visibility.Visible;
                var currentBorrowedItems = LibrarySystem._library.GetUserBorrowedItems(tmp);
                for (int i = 0; i < currentBorrowedItems.Count; i++)
                {
                    if (currentBorrowedItems[i].ISBN == item.ISBN)
                    {
                        Return.Visibility = Visibility.Visible;
                        Borrow.Visibility = Visibility.Collapsed;
                        check = true;
                        break;
                    }
                }
                if (!check && item.IsBorrowed)
                {
                    Borrow.Content = "This book already borrowed";
                    Borrow.IsEnabled = false;
                }
            }
            else
            {
                Librarian lib = LibrarySystem._userManager.GetLoggedUser() as Librarian;
                if (lib != null)
                {
                    Edit.Visibility = Visibility.Visible;
                    Remove.Visibility = Visibility.Visible;
                    if (item.IsBorrowed)
                    {
                        Edit.Content = "Book is currently borrowed";
                        Grid.SetColumn(Edit, 2);
                        Grid.SetColumnSpan(Edit, 2);
                        Edit.Width = 500;
                        Edit.IsEnabled = false;
                        Remove.Visibility = Visibility.Collapsed;
                    }
                }
            }
            Price.Text = item.Price + "$";
            Name.Text = item.Name;
            Author.Text = item.Author;
            Publisher.Text = item.Publisher;
            if (item.Edition == -1)
            {
                panel.Children.Remove(EditionGrid);
                panel.Children.Remove(Editionspace);
            }
            else
                Edition.Text = item.Edition.ToString();

            PublishingYear.Text = item.PrintYear.Year.ToString();
            Category.Text = Enum.GetName(typeof(CategoryName), item.Category);
            ISBN.Text = item.ISBN.ToString();
        }

        private async void Borrow_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = LibrarySystem._userManager.GetLoggedUser() as Customer;
            try
            {
                LibrarySystem._library.BorrowItem(item);
                Borrow.Visibility = Visibility.Collapsed;
                Return.Visibility = Visibility.Visible;
            }
            catch(Exception exception)
            {
                MessageDialog msg = new MessageDialog(exception.Message);
                await msg.ShowAsync();
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = LibrarySystem._userManager.GetLoggedUser() as Customer;
            LibrarySystem._library.ReturnItem(item);
            Return.Visibility = Visibility.Collapsed;
            Borrow.Visibility = Visibility.Visible;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BookEditPage),item);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            LibrarySystem._library.RemoveItem(item.ISBN);
            Frame.Navigate(NavigationHelpClass.PreviousPage);
        }

        private void Backbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(NavigationHelpClass.PreviousPage);
        }
    }
}
