using System;
using System.IO;
using EbayCrawlerWPF.model;
using System.ComponentModel;
using EbayCrawlerWPF.Controllers;
using System.Collections.Generic;
using EbayCrawlerWPF.Controllers.Grimms;

namespace EbayCrawlerWPF.Model
{
    public class Director
    {
        private static SearchRequestHandler _searchRequestHandler;
        public static SearchRequestHandler SearchRequestHandler
        {
            get
            {
                return _searchRequestHandler;
            }
            set
            {
                if (_searchRequestHandler == value)
                {
                    return;
                }

                _searchRequestHandler = value;
                OnPropertyChanged(nameof(SearchRequestHandler));
                Console.WriteLine("fire");
            }
        }

        private static List<AGrimm> _grimms;
        private static int _numberOfPagesToFetch = -1;
        private static Director _instance;

        public Director()
        {
            _instance = this;
        }

        public static void Boot()
        {
            _numberOfPagesToFetch = int.Parse(EbayCrawlerWPF.Properties.Resources.amountofpages);
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EbayCrawler");
            string filepath = Path.Combine(directoryPath, EbayCrawlerWPF.Properties.Resources.requestsfile);
            _grimms = new List<AGrimm>();
            SearchRequestHandler = new SearchRequestHandler(filepath);
            _grimms.Add(EbayBeGrimm.GetInstance(SearchRequestHandler));
            _grimms.Add(EbayCoUkGrimm.GetInstance(SearchRequestHandler));
            _grimms.Add(EbayComGrimm.GetInstance(SearchRequestHandler));

            /*foreach (AGrimm g in _grimms)
            {
                try
                {
                    g.Loop(_numberOfPagesToFetch);

                    foreach (KeyValuePair<String, List<EbayItem>> entry in g.GetItemsList())
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(AsyncSendEbayItemsToDb);

                        //verschrikkelijk lelijke manier om 3 args te kunnen meegeven aan de workermethode
                        EbayItem ebayItem = new EbayItem
                        {
                            Title = g.GetType().Name,
                            Url = entry.Key
                        };

                        Tuple<EbayItem, List<EbayItem>> t = new Tuple<EbayItem, List<EbayItem>>(ebayItem, entry.Value);
                        worker.RunWorkerAsync(argument: t);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error during crawling, writing items to csv");
                    foreach (KeyValuePair<String, List<EbayItem>> entry in g.GetItemsList())
                    {
                        DateTime now = DateTime.Now;
                        CsvController.CreateDocument(entry.Value, now + ".csv");
                        Console.WriteLine(now + ".csv created");
                    }
                }
            }*/
        }

        private static void AsyncSendEbayItemsToDb(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Sending items async");
            Tuple<EbayItem, List<EbayItem>> l = (Tuple<EbayItem, List<EbayItem>>)e.Argument;
            int numberofItemsAdded = DbController.AddEbayItemsToDB(l.Item2);
            DbController.AddMetadata(l.Item1.Url, null, l.Item1.Title, numberofItemsAdded, null);
            Console.WriteLine("Stop sending items async");
        }

        public static event PropertyChangedEventHandler PropertyChanged;
        protected static void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(_instance, new PropertyChangedEventArgs(propertyName));
        }
    }
}
