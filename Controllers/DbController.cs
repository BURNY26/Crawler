using System;
using System.Linq;
using EbayCrawlerWPF.model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using EbayCrawlerWPF.Controllers.Settings;

namespace EbayCrawlerWPF.Model
{
    public class DbController
    {

        public static List<EbayItem> ImportFromDB()
        {
            MySqlConnection conn = EstablishConn();
            List<EbayItem> list = new List<EbayItem>();

            string query = "select * from lootcrate.ebay;";
            Console.WriteLine(query);
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.CommandTimeout = 0;
            MySqlDataReader dataReader = comm.ExecuteReader();
            Boolean hasrows = dataReader.HasRows;

            if (hasrows == false)
            {
                dataReader.Close();
                conn.Close();
                return null;
            }
            else
            {
                while (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        string itemid = dataReader.GetString(0);
                        string title = dataReader.GetString(1);
                        int followers = dataReader.GetInt32(2);
                        double startprice = dataReader.GetDouble(3);
                        double endprice = dataReader.GetDouble(4);
                        string itemurl = dataReader.GetString(5);
                        double shipping = dataReader[6] as double? ?? default(double);
                        string location = dataReader[7] as string;
                        DateTime clock = dataReader[8] as DateTime? ?? default(DateTime);
                        DateTime created_at = dataReader[9] as DateTime? ?? default(DateTime);
                        DateTime updated_at = dataReader[10] as DateTime? ?? default(DateTime);
                        string imageurl = dataReader[11] as string;
                        list.Add(new EbayItem(startprice, endprice, title, itemurl, itemid, clock, followers, shipping, location, imageurl));
                    }
                    dataReader.NextResult();
                }
            }
            dataReader.Close();
            conn.Close();
            return list;
        }

        public static string BuildQueryLikeKeywords(bool substr, List<string> keywords)
        {
            string query = "select * from lootcrate.ebay where (title ";

            int counter = keywords.Count;
            if (substr == true)
            {
                foreach (string s in keywords)
                {
                    query += "like '%" + s + "%' ";
                    counter--;
                    if (counter > 0)
                    {
                        query += ") and (title ";
                    }
                }
                query += ");";
                return query;
            }
            else
            {
                foreach (string s in keywords)
                {
                    query += "like '% " + s + " %' ";
                    query += "or title like '" + s + " %' ";
                    query += "or title like '% " + s + "' ";
                    counter--;
                    if (counter > 0)
                    {
                        query += ") and (title ";
                    }
                }
                query += ");";
                return query;
            }

        }

        public static List<EbayItem> FindItemsByKeywords(bool substr, string textboxcontent)
        {
            List<string> keywords = SplitInKeyWords(textboxcontent);
            if (keywords.Count == 0)
            {
                return null;
            }
            MySqlConnection conn = EstablishConn();
            List<EbayItem> list = new List<EbayItem>();

            string query = BuildQueryLikeKeywords(substr, keywords);
            Console.WriteLine(query);
            MySqlCommand comm = new MySqlCommand(query, conn);
            MySqlDataReader dataReader = comm.ExecuteReader();
            Boolean hasrows = dataReader.HasRows;

            if (hasrows == false)
            {
                dataReader.Close();
                conn.Close();
                return null;
            }
            else
            {
                while (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        string itemid = dataReader.GetString(0);
                        string title = dataReader.GetString(1);
                        int followers = dataReader.GetInt32(2);
                        double startprice = dataReader.GetDouble(3);
                        double endprice = dataReader.GetDouble(4);
                        string itemurl = dataReader.GetString(5);
                        double shipping = dataReader[6] as double? ?? default(double);
                        string location = dataReader[7] as string;
                        DateTime clock = dataReader[8] as DateTime? ?? default(DateTime);
                        DateTime created_at = dataReader[9] as DateTime? ?? default(DateTime);
                        DateTime updated_at = dataReader[10] as DateTime? ?? default(DateTime);
                        string imageurl = dataReader[11] as string;
                        list.Add(new EbayItem(startprice, endprice, title, itemurl, itemid, clock, followers, shipping, location, imageurl));
                    }
                    dataReader.NextResult();
                }
            }
            dataReader.Close();
            conn.Close();
            return list;
        }

        private static List<string> SplitInKeyWords(string mainSearchBoxText)
        {
            return mainSearchBoxText.Split(' ').ToList();
        }

        public static Boolean ContainsItem(EbayItem ei)
        {
            //geval 1 ,check matching id's
            //geval 2 ,zowel titel ,startprice als imageurl zijn gelijk
            //

            string query = "select * from lootcrate.ebay where itemid='" + ei.ItemID + "' ;";
            MySqlConnection conn = EstablishConn();
            MySqlCommand comm = new MySqlCommand(query, conn);

            MySqlDataReader dataReader = comm.ExecuteReader();
            Boolean hasrows = dataReader.HasRows;
            dataReader.Close();
            if (hasrows == true)
            {
                conn.Close();
                return true;
            }
            else
            {
                string query2 = "select * from lootcrate.ebay where imageurl='" + ei.PictureUrl +
                                                                "' and title='" + ei.Title +
                                                                "' and startprice='" + ei.Startprice + "';";
                comm = new MySqlCommand(query, conn);
                dataReader = comm.ExecuteReader();
                hasrows = dataReader.HasRows;
            }
            conn.Close();
            return hasrows;
        }

        public static MySqlConnection EstablishConn()
        {
            /*
            string server = "localhost";
            string database = "ebaycrawler";
            string uid = "lienert";
            string password = "wachtwoord";
            */

            /*
            string server = "185.182.56.105";
            string database = "lienesv171_crawler";
            string uid = "lienesv171_crawler";
            string password = "123456789";
            */

            string connectionString;
            connectionString = "SERVER=" + SettingsController.Settings.DatabaseUrl + ";" + "DATABASE=" +
            SettingsController.Settings.DatabaseName + ";" + "UID=" + SettingsController.Settings.DatabaseUsername + ";" + "PASSWORD=" + SettingsController.Settings.DatabasePassword + ";";

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            //Console.WriteLine("Connection with Db established");
            return conn;
        }



        public static void AddEbayItemToDB(EbayItem ei)
        {
            MySqlConnection conn = EstablishConn();
            try
            {
                String query = "insert into lootcrate.ebay(itemid,title,url,startprice,endprice,clock,followers,imageurl) " +
                    "values ('" + ei.ItemID + "','" + ei.Title + "','" + ei.Url + "','" + ei.Startprice + "','" + ei.Endprice + "','" + ei.Date + "','" + ei.Followers + ei.PictureUrl + "');";
                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Error : trying to write to DB.");
                Console.WriteLine("EbayItemdetails: " + ei);
            }
        }

        //return amount of newly added items
        public static int AddEbayItemsToDB(List<EbayItem> list)
        {
            String query;

            Console.WriteLine("Writing items to DB");
            int teller = 0;
            //Console.WriteLine("Length list is : "+list.Count);
            foreach (EbayItem ei in list)
            {
                try
                {
                    if (!ContainsItem(ei))
                    {
                        query = "insert into lootcrate.ebay(itemid,title,url,startprice,endprice,clock,followers,created_at,shippingcost,location,imageurl,updated_at) " +
                                 "values ('" + ei.ItemID + "','" + ei.Title + "','" + ei.Url + "','"
                                             + (Int32)ei.Startprice + "','" + (Int32)ei.Endprice + "','"
                                             + ei.Date.ToString("yyyy-MM-dd HH:mm") + "','" + ei.Followers + "','"
                                             + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "','" + (Int32)ei.Shipping + "','" + ei.Location + "','" + ei.PictureUrl + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "');";
                        //Console.WriteLine(query);
                        MySqlConnection conn = EstablishConn();
                        MySqlCommand comm = new MySqlCommand(query, conn);

                        comm.ExecuteNonQuery();
                        Console.Write('*');
                        teller++;
                        if (teller % 50 == 0)
                        {
                            Console.Write('\n');
                        }
                        conn.Close();
                    }
                    else
                    {
                        query = "update lootcrate.ebay set startprice='" + (Int32)ei.Startprice
                                                    + "',followers='" + ei.Followers
                                                    + "', updated_at='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                                                    + "',imageurl='" + ei.PictureUrl
                                                    + "' where itemid='" + ei.ItemID + "';";
                        MySqlConnection conn = EstablishConn();
                        MySqlCommand comm = new MySqlCommand(query, conn);

                        comm.ExecuteNonQuery();
                        conn.Close();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine("Error : trying to write to DB.");
                    Console.WriteLine("EbayItemdetails: " + ei);
                }
            }
            Console.WriteLine("Finished adding new EbayItems to DB, " + teller + " were added.");
            return teller;
        }

        public static void AddMetadata(String keywords, String execution, String site, int newitems, String sortorder)
        {
            String query;
            MySqlCommand comm;
            MySqlConnection conn = EstablishConn();
            Console.WriteLine("Writing metadata to DB ");
            try
            {
                query = "insert into lootcrate.metadata(DateTime,Keywords,Executiontime,Site,AmountOfNewItems,SortOrder) " +
                        "values ('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "','" + keywords + "','" + execution + "','"
                                    + site + "','" + newitems + "','" + sortorder + "');";
                comm = new MySqlCommand(query, conn);
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Error : trying to write metadata to DB.");
            }
            Console.WriteLine("Finished writing metadata to DB. ");
            conn.Close();
        }
    }
}
