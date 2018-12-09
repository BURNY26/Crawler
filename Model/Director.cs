using System;
using EbayCrawlerWPF.model;
using System.Configuration;
using System.Diagnostics;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;
using EbayCrawlerWPF.Controllers.Grimms;
using System.IO;
using EbayCrawlerWPF.Controllers;
using System.ComponentModel;

namespace EbayCrawlerWPF.Model
{
    public class Director
    { 
        private List<AGrimm> _grimms;
        private int _numberOfPagesToFetch = -1;
        public SearchRequestHandler sr;
        public Director()
        {
            _numberOfPagesToFetch = int.Parse(EbayCrawlerWPF.Properties.Resources.amountofpages);
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EbayCrawler");
            string filepath = Path.Combine(directoryPath, EbayCrawlerWPF.Properties.Resources.requestsfile);
            _grimms = new List<AGrimm>();
             sr = new SearchRequestHandler(filepath);
            _grimms.Add(EbayBeGrimm.GetInstance(sr));
            _grimms.Add(EbayCoUkGrimm.GetInstance(sr));
            _grimms.Add(EbayComGrimm.GetInstance(sr));            
        }

        public void Boot()
        {
            foreach(AGrimm g in _grimms)
            {
                try{
                    g.Loop(_numberOfPagesToFetch);
                    
                    foreach (KeyValuePair<String, List<EbayItem>> entry in g.GetItemsList())
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(AsyncSendEbayItemsToDb);
                        //verschrikkelijk lelijke manier om 3 args te kunnen meegeven aan de workermethode
                        EbayItem ei = new EbayItem();
                        ei.Title = g.GetType().Name;
                        ei.Url = entry.Key;
                        Tuple<EbayItem, List<EbayItem>> t = new Tuple<EbayItem, List<EbayItem>>(ei, entry.Value);
                        worker.RunWorkerAsync(argument: t );
                   }
                    
                } catch(Exception e)
                {
                    Console.WriteLine("Error during crawling, writing items to csv");
                    foreach (KeyValuePair<String, List<EbayItem>> entry in g.GetItemsList())
                    {
                        DateTime now = DateTime.Now;
                        CsvController.CreateDocument(entry.Value, now+".csv");
                        Console.WriteLine(now+".csv created"); 
                    }
                }
            }      
        }

        private void AsyncSendEbayItemsToDb(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Sending items async" );
            Tuple<EbayItem, List<EbayItem>> l = (Tuple<EbayItem, List<EbayItem>>) e.Argument;
            int numberofItemsAdded = DbController.AddEbayItemsToDB(l.Item2);
            DbController.AddMetadata(l.Item1.Url, null, l.Item1.Title, numberofItemsAdded, null);
            Console.WriteLine("Stop sending items async");
        }

    }
}
