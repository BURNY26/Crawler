using System;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using EbayCrawlerWPF.model;
using EbayCrawlerWPF.ViewModel;
using System.Collections.Generic;
using EbayCrawlerWPF.Controllers.Grimms;
using EbayCrawlerWPF.Controllers;

namespace EbayCrawlerWPF.Model
{
    public class EbayComGrimm : AGrimm
    {
        private static EbayComGrimm _grim;

        public EbayComGrimm(SearchRequestHandler handler)
        {
            _handler = handler;
            _goblin = new Goblin();
            _itemsList = new Dictionary<String, List<EbayItem>>();
            _urls = FetchUrls();
        }

        //create ebayitems and returns url to next page
        public override List<EbayItem> CreateItems(String filename)
        {
            HtmlDocument doc = new HtmlDocument();
            var path = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.GetType().Name), filename);
            doc.Load(path, Encoding.UTF8);
            EbayItem ei;
            List<EbayItem> list = new List<EbayItem>();
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//li[@class='s-item  ']"))
            {
                try
                {
                    
                    String itemlink = GetItemLink(node);
                    String itemid = TextExtractorEbayCom.ExtractItemIdFromUrl(itemlink);
                    String piclink = GetImageLink(node);
                    String title = GetTitle(node);
                    
                    int followers = GetFollowers(node);
                    
                    String location ="";
                    if (node.InnerHtml.Contains("From"))
                    {
                       location = GetLocation(node); 
                    }
                    
                    DateTime clock = DateTime.Now;
                    if (node.InnerHtml.Contains("Time left"))
                    {
                        foreach(HtmlNode n in node.SelectNodes("descendant::span[contains(@class,'s-item__time-left')]"))
                        {
                            clock = TextExtractorEbayCom.ExtractEbayComTimeLeft(n.OuterHtml,DateTime.Now);
                        }
                    }
                    double exchange = Double.Parse(Properties.Resources.europerdollar);
                    
                    string price = node.SelectSingleNode("descendant::span[contains(@class,'s-item__price')]").InnerText;
                    double startPrice = exchange * Double.Parse(TextExtractorEbayCom.ExtractEbayComStartPrice(price));
                    double endPrice = exchange * Double.Parse(TextExtractorEbayCom.ExtractEbayComEndPrice(price));
                    double shipping = 0;
                    if (node.InnerHtml.Contains("shipping"))
                    {
                        shipping = exchange * TextExtractorEbayCom.ExtractShipping(node.SelectSingleNode("descendant::span[contains(@class,'s-item__shipping s-item__logisticsCost')]").InnerText);
                    }
                    
                    if (startPrice > 0)
                    {
                        ei = new EbayItem(startPrice, endPrice, title, itemlink, itemid, clock, followers, shipping, location, piclink);
                        list.Add(ei);
                        Console.Write("+");
                    }
                    else
                    {
                        ei = new EbayItem(startPrice, endPrice, title, itemlink, itemid, clock, followers, shipping, location, piclink);
                        list.Add(ei);
                        Console.WriteLine("==================================================\n" + ei);
                    }
                    
                }
                catch (Exception e)
                {
                    CsvController.CreateDocument(list, filename + ".csv");
                    Console.WriteLine("Problem with creating Ebayitem, check "+ filename + ".csv");
                    Console.WriteLine(e.Message);
                }
            }
            
            return list;
        }

        public DateTime GetTime(HtmlNode node)
        {
            return TextExtractorEbayCom.ExtractEbayComTimeLeft(node.OuterHtml,DateTime.Now);
        }

        public String GetLocation(HtmlNode node)
        {
            return TextExtractorEbayCom.ExtractEbayComLocation(node.SelectSingleNode("descendant::span[contains(@class,'s-item__location s-item__itemLocation')]").InnerText);
        }
        public int GetFollowers(HtmlNode node)
        {
            return TextExtractorEbayCom.ExtractFollowersEbayCom(node.InnerHtml);
        }
        public String GetTitle(HtmlNode node)
        {
            return (node.SelectSingleNode("div/div[1]/div/a/div/img")).Attributes["alt"].DeEntitizeValue;
        }
        public String GetImageLink(HtmlNode node)
        {            
            return TextExtractorEbayCom.ExtractPicUrl(node.InnerHtml);
        }
        public String GetItemLink(HtmlNode node)
        {   
            return (node.SelectSingleNode("div/div[2]/a")).Attributes["href"].DeEntitizeValue;
        }

        public static EbayComGrimm GetInstance(SearchRequestHandler handler)
        {
            _grim = new EbayComGrimm(handler);
            return _grim;
        }

        protected override string NextPage(string url, int current)
        {
            string pgn = "pgn=" + current;
            string vervanging = "pgn=" + (current + 1);
            //Console.WriteLine("=======CreatedURL======="+ url.Replace(pgn, vervanging));
            return url.Replace(pgn, vervanging);
        }
    }
}
