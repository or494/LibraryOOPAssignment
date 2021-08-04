using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class ReportsPage : Page
    {
        Report report;
        public ReportsPage()
        {
            this.InitializeComponent();
            RestartPage(true);
        }

        private void RestartPage(bool isInitialize)
        {
            if (!isInitialize) ItemTableStack.Children.Clear();
            else
            {
                ReportDate.Items.Clear();
                for (int i = 0; i < LibrarySystem._reportsManager.GetReports().Count; i++)
                {
                    ReportDate.Items.Add(LibrarySystem._reportsManager.GetReports()[i].Date);
                }
                if (ReportDate.Items.Count > 0)
                    ReportDate.SelectedIndex = ReportDate.Items.Count - 1;
            }
            if (LibrarySystem._reportsManager.GetReports().Count == 0)
            {
                DataGrid.Visibility = Visibility.Collapsed;
                DefaultMessage.Visibility = Visibility.Visible;
            }
            else
            {
                DataGrid.Visibility = Visibility.Visible;
                DefaultMessage.Visibility = Visibility.Collapsed;
                journalItems.Text = $"{report.ExistJournals}";
                bookItems.Text = $"{report.ExistBooks}";
                TotalItems.Text = $"{report.ExistBooks + report.ExistJournals}";
                BorrowedItems.Text = $"{report.BorrowingHistories.Count}";
            }
        }

        private void ResetPage()
        {
        }

        private void CreateTableRow(string itemName, string customerName, DateTime borrow, DateTime expected, StackPanel table)
        {
            Grid row = new Grid();
            row.Height = 70;
            row.ColumnDefinitions.Add(new ColumnDefinition());
            row.ColumnDefinitions.Add(new ColumnDefinition());
            row.ColumnDefinitions.Add(new ColumnDefinition());
            row.ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(50);
            row.RowDefinitions.Add(rowDefinition);
            table.Children.Add(row);

            TextBlock itemNametxt = new TextBlock();
            itemNametxt.Text = itemName;
            Grid.SetColumn(itemNametxt, 0);
            itemNametxt.FontSize = 30;
            itemNametxt.HorizontalAlignment = HorizontalAlignment.Stretch;
            itemNametxt.VerticalAlignment = VerticalAlignment.Center;
            itemNametxt.TextAlignment = TextAlignment.Center;
            itemNametxt.Foreground = new SolidColorBrush(Colors.White);

            TextBlock customerNametxt = new TextBlock();
            customerNametxt.Text = customerName;
            Grid.SetColumn(customerNametxt, 1);
            customerNametxt.FontSize = 30;
            customerNametxt.HorizontalAlignment = HorizontalAlignment.Stretch;
            customerNametxt.VerticalAlignment = VerticalAlignment.Center;
            customerNametxt.TextAlignment = TextAlignment.Center;
            customerNametxt.Foreground = new SolidColorBrush(Colors.White);


            TextBlock borrowingDatetxt = new TextBlock();
            borrowingDatetxt.Text = borrow.ToString();
            Grid.SetColumn(borrowingDatetxt, 2);
            borrowingDatetxt.FontSize = 30;
            borrowingDatetxt.HorizontalAlignment = HorizontalAlignment.Stretch;
            borrowingDatetxt.VerticalAlignment = VerticalAlignment.Center;
            borrowingDatetxt.TextAlignment = TextAlignment.Center;
            borrowingDatetxt.Foreground = new SolidColorBrush(Colors.White);

            TextBlock expectedReturnDatetxt = new TextBlock();
            expectedReturnDatetxt.Text = expected.ToString();
            Grid.SetColumn(expectedReturnDatetxt, 3);
            expectedReturnDatetxt.FontSize = 30;
            expectedReturnDatetxt.HorizontalAlignment = HorizontalAlignment.Stretch;
            expectedReturnDatetxt.VerticalAlignment = VerticalAlignment.Center;
            expectedReturnDatetxt.TextAlignment = TextAlignment.Center;
            expectedReturnDatetxt.Foreground = new SolidColorBrush(Colors.White);

            row.Children.Add(itemNametxt);
            row.Children.Add(customerNametxt);
            row.Children.Add(borrowingDatetxt);
            row.Children.Add(expectedReturnDatetxt);

        }

        private void ReportGenerator_Click(object sender, RoutedEventArgs e)
        {
            Report report = LibrarySystem._reportsManager.GenerateReport();
            ReportDate.Items.Add(report.Date);
            RestartPage(true);
        }
        private void ReportDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox tmp = sender as ComboBox;
            if (tmp.SelectedItem != null)
            {
                DateTime date = DateTime.Parse(tmp.SelectedItem.ToString());
                report = LibrarySystem._reportsManager.GetReport(date);
            }
            RestartPage(false);
            if (report != null)
            {
                for (int i = 0; i < report.BorrowingHistories.Count; i++)
                {
                    if(report.BorrowingHistories[i].BorrowingHistory.ReturningDate.Year > 2000)
                    {
                        AbstractItem item = LibrarySystem._library.GetItem(report.BorrowingHistories[i].BorrowingHistory.Item.ISBN);
                        Customer person = LibrarySystem._userManager.GetPerson(report.BorrowingHistories[i].BorrowingHistory.Borrower.Id) as Customer;
                        DateTime borrowingDate = report.BorrowingHistories[i].BorrowingHistory.BorrowingDate;
                        DateTime expectedReturn = borrowingDate.AddDays(14);
                        if (item != null && person != null)
                        {
                            CreateTableRow(item.Name, person.FirstName + " " + person.LastName, borrowingDate, expectedReturn, ItemTableStack);
                        }
                    }
                }
            }
        }
    }
}
