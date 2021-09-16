using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DataAccessLibrary;
using System.Data;
using System.Collections.ObjectModel;

namespace AddressBook
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //On startup, show initialized database
            var collection = new ObservableCollection<Entry>();
            foreach (DataRow row in DataAccess.GetData().Rows)
            {
                var obj = new Entry()
                {
                    Name = (string)row.ItemArray[0],
                    Date = (string)row.ItemArray[1],
                    PhoneNumber = (string)row.ItemArray[2],
                    Address = (string)row.ItemArray[3]
                };
                collection.Add(obj);
            }
            dataGrid.DataContext = collection;
        }

        //On search button click check textboxes for text to determine which search queries to run
        //then update and show query results
        private void Search(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            if (NameSearch.Text.Length > 0)
            {
                if (lowDateSearch.Text.Length > 0 && highDateSearch.Text.Length > 0)
                {
                    dt = DataAccess.GetDateAndNameSearch(lowDateSearch.Text, highDateSearch.Text, NameSearch.Text);
                }
                else
                {
                    dt = DataAccess.GetNameSearch(NameSearch.Text);
                }
            }
            else if(lowDateSearch.Text.Length > 0 && highDateSearch.Text.Length > 0)
            {
                dt = DataAccess.GetDateSearch(lowDateSearch.Text, highDateSearch.Text);
            }
            else
            {
                dt = DataAccess.GetData();
            }

            var collection = new ObservableCollection<Entry>();
            foreach(DataRow row in dt.Rows)
            {
                var obj = new Entry()
                {
                    Name = (string)row.ItemArray[0],
                    Date = (string)row.ItemArray[1],
                    PhoneNumber = (string)row.ItemArray[2],
                    Address = (string)row.ItemArray[3]
                };
                collection.Add(obj);
            }

            dataGrid.DataContext = collection;
        }

        //invoke insert query if all boxes contain text
        private void Add(object sender, RoutedEventArgs e)
        {
            if (Name.Text.Length > 0 && BirthDate.Text.Length > 0 && PhoneNumber.Text.Length > 0 && Address.Text.Length > 0)
            {
                DataAccess.AddData(Name.Text, BirthDate.Text, PhoneNumber.Text, Address.Text);
            }
        }

        //invoke delete query by primary key name
        private void Delete(object sender, RoutedEventArgs e)
        {
            if (Name.Text.Length > 0)
            {
                DataAccess.DeleteData(Name.Text);
            }
        }
    }

    //Class to store address book data
    public class Entry
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        
    }
}
