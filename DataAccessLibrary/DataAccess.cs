using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        //Initialize empty database with proper address book columns
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("addressBook.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "addressBook.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String dropTableCommand = "DROP TABLE IF EXISTS AddressBook;";
                String tableCommand = "CREATE TABLE " +
                    "AddressBook (Name TEXT PRIMARY KEY, BirthDate DATE, PhoneNumber TEXT, Address TEXT);";

                SqliteCommand dropTable = new SqliteCommand(dropTableCommand, db);
                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                
                dropTable.ExecuteReader();
                createTable.ExecuteReader();

                db.Close();
            }
            
        }

        //Handle insert query
        public static void AddData(string nameText, string dateText, string phoneText, string addressText)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "addressBook.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO AddressBook VALUES (@Name, @Date, @Phone, @Address);";
                insertCommand.Parameters.AddWithValue("Name", nameText);
                insertCommand.Parameters.AddWithValue("@Date", dateText);
                insertCommand.Parameters.AddWithValue("@Phone", phoneText);
                insertCommand.Parameters.AddWithValue("@Address", addressText);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        //Handle delete query
        public static void DeleteData(string nameText)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "addressBook.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand deleteCommand = new SqliteCommand();
                deleteCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                deleteCommand.CommandText = "DELETE FROM AddressBook WHERE Name = @Name";
                deleteCommand.Parameters.AddWithValue("@Name", nameText);
                deleteCommand.ExecuteReader();

                db.Close();
            }
        }

        //Handle select statement for the entire database
        public static DataTable GetData()
        {
            DataTable addressBook = new DataTable();
            
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "addressBook.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * FROM AddressBook", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                addressBook.Load(query);

                

                db.Close();
            }

            return addressBook;
        }

        //Handle select query for specific name
        public static DataTable GetNameSearch(string name)
        {
            DataTable addressBook = new DataTable();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "addressBook.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * FROM AddressBook WHERE Name = @name", db);
                selectCommand.Parameters.AddWithValue("@name", name);

                SqliteDataReader query = selectCommand.ExecuteReader();

                addressBook.Load(query);

                db.Close();
            }

            return addressBook;
        }

        //Handle select query for between two dates
        public static DataTable GetDateSearch(string lowerBound, string upperBound)
        {
            DataTable addressBook = new DataTable();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "addressBook.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * FROM AddressBook WHERE BirthDate BETWEEN @lower and @upper", db);
                selectCommand.Parameters.AddWithValue("@lower", lowerBound);
                selectCommand.Parameters.AddWithValue("@upper", upperBound);

                SqliteDataReader query = selectCommand.ExecuteReader();

                addressBook.Load(query);

                db.Close();
            }

            return addressBook;
        }

        //Handle select query for date and name search combined
        public static DataTable GetDateAndNameSearch(string lowerBound, string upperBound, string name)
        {
            DataTable addressBook = new DataTable();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "addressBook.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * FROM AddressBook WHERE (BirthDate BETWEEN @lower and @upper) AND (Name = @name)", db);
                selectCommand.Parameters.AddWithValue("@lower", lowerBound);
                selectCommand.Parameters.AddWithValue("@upper", upperBound);
                selectCommand.Parameters.AddWithValue("@name", name);

                SqliteDataReader query = selectCommand.ExecuteReader();

                addressBook.Load(query);

                db.Close();
            }

            return addressBook;
        }
    }
}
