using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            LibrarySystem.Load();
            ResetPage();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegistrationPage));
        }

        private async void LibraryName_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(10);
            LibraryName.Text = LibrarySystem._library.GetLibraryName();
            IDTxtBox.Text = "";
            PasswardTxtBox.Password = "";
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            IDInvalidTxtBox.Visibility = Visibility.Collapsed;
            PasswardInvalidTxtBox.Visibility = Visibility.Collapsed;
            LoginFailedTxtBlock.Visibility = Visibility.Collapsed;
            if (IDTxtBox.Text.Length == 9)
            {
                if (PasswardTxtBox.Password.Length >= 8)
                {
                    bool check = LibrarySystem._userManager.LogIn(IDTxtBox.Text, PasswardTxtBox.Password);
                    if (check)
                    {
                        if ((LibrarySystem._userManager.GetLoggedUser() as Librarian) != null)
                            Frame.Navigate(typeof(EmployeeNavigationPage));
                        else if ((LibrarySystem._userManager.GetLoggedUser() as Customer) != null)
                            Frame.Navigate(typeof(ClientNavigationPage));
                        return;
                    }
                    else
                    {
                        LoginFailedTxtBlock.Visibility = Visibility.Visible;
                    }
                }
                else
                    PasswardInvalidTxtBox.Visibility = Visibility.Visible;
            }
            else
            {
                IDInvalidTxtBox.Visibility = Visibility.Visible;
            }
            if (PasswardTxtBox.Password.Length < 8)
                PasswardInvalidTxtBox.Visibility = Visibility.Visible;
        }

        private void IDTxtBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void ResetPage()
        {
            IDInvalidTxtBox.Visibility = Visibility.Collapsed;
            PasswardInvalidTxtBox.Visibility = Visibility.Collapsed;
            LoginFailedTxtBlock.Visibility = Visibility.Collapsed;
            IDTxtBox.Text = "";
            PasswardTxtBox.Password = "";
        }
    }
}
