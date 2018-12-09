using EbayCrawlerWPF.model;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace EbayCrawlerWPF.Controllers.Grimms
{
    public abstract class AGrimm
    {
        protected SearchRequestHandler _handler;
        protected Goblin _goblin;
        //ebayitems / keywords
        protected Dictionary<String, List<EbayItem>> _itemsList;
        //links / keywords
        protected Dictionary<String, List<String>> _urls;
        public abstract List<EbayItem> CreateItems(String filename);
        //er is iets speciaal aan de volgende x items opvragen van ebay ,wnr je de skc param wijzigt krijg je de volgende 50 items
        protected abstract string NextPage(string url, int current);

        //create docs using string
        public void CreateDocument(String htmlcontent, String filename)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlcontent);

            string grimmBaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.GetType().Name);
            Directory.CreateDirectory(grimmBaseDirectory); // Creates the directory if it does not yet exists

            using (StreamWriter file = new StreamWriter(Path.Combine(grimmBaseDirectory, filename)))
            {
                file.WriteLine(doc.Text);
            }
        }

        public Dictionary<String, List<String>> FetchUrls()
        {
            Dictionary<String, List<String>> map = new Dictionary<String, List<String>>();
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EbayCrawler");
            String s = File.ReadAllText(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EbayCrawler"),"Links"), "sites.json"));
            dynamic stuff = JsonConvert.DeserializeObject(s);
            foreach (dynamic request in stuff[this.GetType().Name])
            {
                String kw = request.keywords;
                List<String> list = new List<string>();
                foreach (String l in request.links)
                {
                    list.Add(l);
                }
                map.Add(kw, list);
            }
            return map;
        }

        protected DateTime GetEnddate(string timems)
        {
            DateTime enddate = new DateTime();
            if (timems != null)
            {
                double ticks = double.Parse(timems) + 7200000;
                TimeSpan time = TimeSpan.FromMilliseconds(ticks);
                enddate = new DateTime(1970, 1, 1) + time;
            }
            return enddate;
        }

        public Dictionary<String, List<EbayItem>> GetItemsList()
        {
            return this._itemsList;
        }

        //iterate over ebay from page 1 till lastpage ,if -1 till lastpage
        public void Loop(int lastpage)
        {
            int checkingPageIndex = 1;
            try
            {
                foreach (KeyValuePair<String, List<String>> entry in _urls)
                {
                    String key = entry.Key;
                    // Do something here
                    List<EbayItem> list = new List<EbayItem>();
                    foreach (String s in entry.Value)
                    {
                        String starturl = s;
                        while (starturl != null && checkingPageIndex != lastpage + 1 && checkingPageIndex<200)
                        {
                            Console.WriteLine(this.GetType().Name + " checking " + starturl);

                            string filename = string.Format("ebay{0}.html", checkingPageIndex);
                            string content = _goblin.FetchHtml(starturl); //blocking call(?)

                            CreateDocument(content, filename);                            
                            list.AddRange(CreateItems(filename));
                            _handler.ProcessItemsAgainstAllSearchRequestsFromJsonurl(list, key);
                            Console.WriteLine(this.GetType().Name + " finished reading page " + checkingPageIndex);
                            starturl = NextPage(starturl, checkingPageIndex);
                            checkingPageIndex++;
                        }
                    }
                    checkingPageIndex = 1;
                    _itemsList.Add(key, list);
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(this.GetType().Name + " Error : Loop");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

    }
}
