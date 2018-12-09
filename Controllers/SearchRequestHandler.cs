using EbayCrawlerWPF.model;
using EbayCrawlerWPF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbayCrawlerWPF.Controllers
{
    public class SearchRequestHandler
    {
        private int _idCounter = -1;
        private double _hitThreshold = 0.5;
        private List<EbayItem> dbcache;
        
        private Dictionary<int, Dictionary<EbayItem, double>> _hitpercentages;
        public ArrayOfJsonurl _jsonurls;

        public SearchRequestHandler(string path)
        {
            _hitpercentages = new Dictionary<int, Dictionary<EbayItem, double>>();
            FetchSearchRequests(path);
        }

        private void FetchSearchRequests(string path)
        {
             _jsonurls = (ArrayOfJsonurl) DataAccess.XmlDataAccess.ReadXmlFile<ArrayOfJsonurl>(path);
        }

        public SearchRequest GetSearchRequest(int id)
        {
            foreach(Jsonurl jurl in _jsonurls.Jsonurls)
            {
                foreach (SearchRequest rs in jurl.SearchRequests)
                {
                    if (rs.Id == id)
                    {
                        return rs;
                    }
                }
            }
            return null;
        }

        public List<SearchRequest> GetAllSearchRequestsOfJsonurl(string jsonurl)
        {
            
            foreach (Jsonurl jurl in _jsonurls.Jsonurls)
            {
                if (jurl.Name.Equals(jsonurl))
                {
                    return jurl.SearchRequests;
                }
            }
            return null;
        }

        public List<SearchRequest> GetAllSearchRequests()
        {
            List<SearchRequest> list = new List<SearchRequest>();
            foreach(Jsonurl jurl in _jsonurls.Jsonurls)
            {
                list.AddRange(jurl.SearchRequests);
                foreach(SearchRequest sr in jurl.SearchRequests)
                {
                    if (sr.Id > _idCounter)
                    {
                        _idCounter = sr.Id;
                    }
                }
            }
            return list;
        }

        public void UpdateSearchRequest(SearchRequest s)
        {
            DbController.DeleteSearchRequestTable(s.Id);
            DbController.CreateSearchRequestTable(s.Id);
            SearchRequest sr = GetSearchRequest(s.Id);
            sr.PrimaryKeywords = s.PrimaryKeywords;
            sr.SecondaryKeywords = s.SecondaryKeywords;
            sr.ExclusiveKeywords = s.ExclusiveKeywords;            
        }

        public void DeleteSearchRequest(int id)
        {
            DbController.DeleteSearchRequestTable(id);
            Jsonurl foundjurl=null;
            SearchRequest foundrs=null;
            foreach (Jsonurl jurl in _jsonurls.Jsonurls)
            {
                foreach (SearchRequest rs in jurl.SearchRequests)
                {
                    if (rs.Id == id)
                    {
                        foundjurl = jurl;
                        foundrs = rs;
                    }
                }
            }
            foundjurl.SearchRequests.Remove(foundrs);
        }

        public void AddSearchRequest(SearchRequest s,string jsonurl_name)
        {
            s.Id = _idCounter + 1;
            DbController.CreateSearchRequestTable(s.Id);
            Jsonurl jurl = (_jsonurls.Jsonurls.Find(x => x.Name.Equals(jsonurl_name)));
            if (jurl != null)
            {
                jurl.SearchRequests.Add(s);
            }
            else
            {
                jurl = new Jsonurl();
                jurl.SearchRequests.Add(s);
                _jsonurls.Jsonurls.Add(jurl);
            }
        }

        public void ProcessItemsAgainstAllSearchRequestsFromJsonurl(List<EbayItem> list, string jsonurl)
        {
            foreach(EbayItem ei in list)
            {
                foreach(SearchRequest sr in (_jsonurls.Jsonurls.Find( x => x.Name.Equals(jsonurl))).SearchRequests)
                {
                    if (!_hitpercentages.ContainsKey(sr.Id))
                    {
                        _hitpercentages.Add(sr.Id, new Dictionary<EbayItem, double>());
                    }
                    if (!_hitpercentages[sr.Id].ContainsKey(ei))
                    {
                        _hitpercentages[sr.Id].Add(ei, CalculateScore(sr, ei));
                    }
                    
                }
            }
        }

        public void ProcessDbItemsAgainstAllSearchRequestsFromJsonUrl(string jsonurl)
        {            
            if (dbcache == null)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(AsyncFetchDb);
                worker.DoWork += new DoWorkEventHandler(AsyncProcessItemsAgainstSearchRequests);

                worker.RunWorkerAsync(argument : jsonurl);
            }
            else
            {
                ProcessItemsAgainstAllSearchRequestsFromJsonurl(dbcache, jsonurl);
            }
        }

        private void AsyncProcessItemsAgainstSearchRequests(object sender ,DoWorkEventArgs e)
        {
            string jsonurl = (string)e.Argument;
            ProcessDbItemsAgainstAllSearchRequestsFromJsonUrl(jsonurl);
        }

        public void ShowHits()
        {
            Console.WriteLine("#####################Showhits######################");
            foreach (KeyValuePair<int, Dictionary<EbayItem, Double>> entry in _hitpercentages)
            {
                foreach (KeyValuePair<EbayItem, Double> entry2 in entry.Value)
                {
                    if (entry2.Value>=_hitThreshold)
                    {
                        Console.WriteLine("Hit on request #"+entry.Key);
                        Console.WriteLine(entry2.Key.Title);
                        Console.WriteLine(entry2.Value);
                    }
                }
            }
            Console.WriteLine("#####################END-Showhits######################");
        }

        public void ShowcaseScore()
        {
            Console.WriteLine("EbayItems score per SR ");
            foreach (KeyValuePair<int, Dictionary<EbayItem, Double>> entry in _hitpercentages)
            {
                Console.WriteLine("SR " + entry.Key + " : ");
                foreach (KeyValuePair<EbayItem, Double> entry2 in entry.Value)
                {
                    Console.WriteLine(entry2.Key.Title);
                    Console.WriteLine(entry2.Value);
                }
            }
        }

        public Double CalculateScore(SearchRequest sr , EbayItem ei)
        {
            double score = 0;
            double numberofpk = sr.PrimaryKeywords.Count;
            double numberofsk = sr.SecondaryKeywords.Count;
            foreach(string excl in sr.ExclusiveKeywords)
            {
                List<string> excluded = excl.Split('|').ToList<string>();
                foreach(string s in excluded)
                {
                    if (ei.Title.ToLower().Split(' ', ',').ToList().Contains(s.ToLower()))
                    {
                        return -1;
                    }
                }

            }
            foreach(string pk in sr.PrimaryKeywords)
            {
                List<string> primary = pk.Split('|').ToList<string>();
                foreach(string s in primary)
                {
                    if (ei.Title.ToLower().Split(' ', ',').ToList().Contains(s.ToLower()))
                    {
                        score += 1 / numberofpk;
                    }
                }

            }
            foreach (string sk in sr.SecondaryKeywords)
            {
                List<string> secundary = sk.Split('|').ToList<string>();
                foreach (string s in secundary)
                {
                    if (ei.Title.ToLower().Split(' ', ',', '.', ':').ToList().Contains(s.ToLower()))
                    {
                        score += 1 / (10 * numberofsk);
                    }
                }

            }
            return score;
        }
        
        private void AsyncFetchDb(object sender,DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            dbcache = DbController.ImportFromDB();
        }

    }
}
