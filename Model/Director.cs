using System;
using EbayCrawlerWPF.model;
using System.Configuration;
using EbayCrawlerWPF.AppConfig;
using System.Diagnostics;
using static EbayCrawlerWPF.AppConfig.SitesSection;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;
using EbayCrawlerWPF.Controllers.Grimms;

namespace EbayCrawlerWPF.Model
{
    public class Director
    { 
        private List<AGrimm> _grimms;
        private int _numberOfPagesToFetch = -1;

        public Director()
        {
            _grimms = new List<AGrimm>();
            _grimms.Add(EbayBeGrimm.GetInstance());
            _grimms.Add(EbayCoUkGrimm.GetInstance());
            _grimms.Add(EbayComGrimm.GetInstance());
            _numberOfPagesToFetch = int.Parse(EbayCrawlerWPF.Properties.Resources.amountofpages);
        }

        public void Boot()
        {
            foreach(AGrimm g in _grimms)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                try{
                    g.Loop(_numberOfPagesToFetch);
                    sw.Stop();
                    
                    foreach (KeyValuePair<String, List<EbayItem>> entry in g.GetItemsList())
                    {
                        int newitems = DbController.AddEbayItemsToDB(entry.Value);
                        DbController.AddMetadata(entry.Key, null, g.GetType().Name, newitems, null);
                    }
                    
                } catch(Exception e)
                {
                    Console.WriteLine("wegschrijving tabel bij fout ");
                    sw.Stop();
                    foreach (KeyValuePair<String, List<EbayItem>> entry in g.GetItemsList())
                    {
                        int newitems = DbController.AddEbayItemsToDB(entry.Value);
                        DbController.AddMetadata(entry.Key, null, g.GetType().Name, newitems, null);
                    }
                }
            }      
        }

        private String DetermineDuration(TimeSpan ts, String text)
        {
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + text + " => " + elapsedTime);
            return elapsedTime;
        }

        public void ExportHtmlToCsv(string htmlfile, string csvfilename)
        {

        }
    }
}
