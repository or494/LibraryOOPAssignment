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
    public sealed partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            this.InitializeComponent();
            ResetPage();
            for (int i = 18; i < 100; i++)
            {
                AgeComboBox.Items.Add(i);
            }
            AgeComboBox.SelectedIndex = 0;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void ResetPage()
        {
            FirstNameInvalidBlock.Visibility = Visibility.Collapsed;
            LastNameInvalidBlock.Visibility = Visibility.Collapsed;
            IDInvalidBlock.Visibility = Visibility.Collapsed;
            PasswardInvalidBlock.Visibility = Visibility.Collapsed;
            VerifyPasswardInvalidBlock.Visibility = Visibility.Collapsed;
            AgeInvalidBlock.Visibility = Visibility.Collapsed;
            FirstNameTxtBox.Text = "";
            LastNameTxtBox.Text = "";
            IDTxtBox.Text = "";
            PasswardTxtBox.Password = "";
            VerifyPasswardTxtBox.Password = "";
            AgeComboBox.SelectedItem = null;
        }

        private void NameTxtBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsLetter(c));
        }

        private void IDTxtBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private async void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            FirstNameInvalidBlock.Visibility = Visibility.Collapsed;
            LastNameInvalidBlock.Visibility = Visibility.Collapsed;
            IDInvalidBlock.Visibility = Visibility.Collapsed;
            PasswardInvalidBlock.Visibility = Visibility.Collapsed;
            VerifyPasswardInvalidBlock.Visibility = Visibility.Collapsed;
            AgeInvalidBlock.Visibility = Visibility.Collapsed;

            if (FirstNameTxtBox.Text.Length > 1 && LastNameTxtBox.Text.Length > 1 && IDTxtBox.Text.Length == 9 && PasswardTxtBox.Password.Length >= 8 &&
                VerifyPasswardTxtBox.Password == PasswardTxtBox.Password && AgeComboBox.SelectedItem != null)
            {
                Person registering = new Customer(FirstNameTxtBox.Text, LastNameTxtBox.Text, int.Parse(AgeComboBox.SelectedValue.ToString()), PasswardTxtBox.Password, IDTxtBox.Text);

                bool check = LibrarySystem._userManager.Register(registering);
                MessageDialog msg;
                if (check)
                {
                    msg = new MessageDialog("You registered succesfully");
                    await msg.ShowAsync();
                    LibrarySystem._userManager.SetLoggedUser(registering);
                    Frame.Navigate(typeof(ClientNavigationPage));
                }
                else
                {
                    msg = new MessageDialog("It was unable to register");
                    await msg.ShowAsync();
                }
            }

            if (FirstNameTxtBox.Text.Length <= 1)
                FirstNameInvalidBlock.Visibility = Visibility.Visible;

            if (LastNameTxtBox.Text.Length <= 1)
                LastNameInvalidBlock.Visibility = Visibility.Visible;

            if (IDTxtBox.Text.Length != 9)
                IDInvalidBlock.Visibility = Visibility.Visible;

            if (PasswardTxtBox.Password.Length < 8)
                PasswardInvalidBlock.Visibility = Visibility.Visible;

            if (PasswardTxtBox.Password != VerifyPasswardTxtBox.Password)
                VerifyPasswardInvalidBlock.Visibility = Visibility.Visible;

            if (AgeComboBox.SelectedIndex == null)
                AgeInvalidBlock.Visibility = Visibility.Visible;
        }
    }
}
