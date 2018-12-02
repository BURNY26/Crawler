using EbayCrawlerWPF.Controllers;
using EbayCrawlerWPF.Controllers.Grimms;
using EbayCrawlerWPF.Controllers.TextExtractors;
using EbayCrawlerWPF.model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace EbayCrawlerWPF.Model
{
    public class EbayCoUkGrimm : AGrimm
    {

        private static EbayCoUkGrimm _grim;

        public EbayCoUkGrimm(SearchRequestHandler handler)
        {
            _handler = handler;
            _goblin = new Goblin();
            _itemsList = new Dictionary<String,List<EbayItem>>();
            _urls   = FetchUrls();
        }

        //create ebayitems
        public override List<EbayItem> CreateItems(String filename)
        {
            HtmlDocument doc = new HtmlDocument();
            var path = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.GetType().Name), filename);
            doc.Load(path, Encoding.UTF8);
            EbayItem ei;
            List<EbayItem> list = new List<EbayItem>();
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//li[@class='sresult lvresult clearfix li' or @class='sresult lvresult clearfix li shic']"))
            {
                try
                {
                    //Console.WriteLine("#########");
                    string itemid = node.Attributes["id"].Value;
                    string itemlink = node.SelectSingleNode(".//h3[@class='lvtitle']").SelectSingleNode("a").Attributes["href"].Value;
                    string piclink = node.SelectSingleNode(".//div[@class='lvpicinner full-width picW']").SelectSingleNode(".//img").Attributes["src"].Value;
                    
                    //check once if there exists a better picurl , 2nd condition makes sure there even is an imageurl
                    if (piclink.Contains(".gif") && node.SelectSingleNode(".//div[@class='lvpicinner full-width picW']").SelectSingleNode(".//img").Attributes["imgurl"] != null)
                    {
                        piclink = node.SelectSingleNode(".//div[@class='lvpicinner full-width picW']").SelectSingleNode(".//img").Attributes["imgurl"].Value;
                    }
                    //Console.WriteLine(itemid+"\n"+itemlink+"\n"+piclink);
                    string title = TextExtractorEbayCoUk.ExtractTitle(node.SelectSingleNode(".//h3[@class='lvtitle']").SelectSingleNode("a").Attributes["title"].Value);
                    int followers = int.Parse(TextExtractorEbayCoUk.ExtractFollowers(node.SelectSingleNode(".//li[@class='lvextras']").InnerText.TrimEnd().TrimStart()));
                    string location = "";
                    if (null != node.SelectSingleNode(".//ul[@class='lvdetails left space-zero full-width']"))
                    {
                        location = TextExtractorEbayCoUk.ExtractLocation(node.SelectSingleNode(".//ul[@class='lvdetails left space-zero full-width']").InnerHtml);
                    }

                    else
                    {
                        location = null;
                    }
                    //Console.WriteLine(title + "\n" + followers + "\n" + location);
                    string timeleft = null;
                    if (null != node.SelectSingleNode(".//span[@class='tme']"))
                    {

                        timeleft = TextExtractorEbayCom.ExtractTime(node.SelectSingleNode(".//span[@class='tme']").InnerHtml);
                    }
                    DateTime clock = GetEnddate(timeleft);

                    double exchange = Double.Parse(Properties.Resources.europerpound);

                    string price = node.SelectSingleNode(".//li[@class='lvprice prc']").InnerText.TrimStart().TrimEnd();
                    //Console.WriteLine("====="+price);
                    double startPrice = exchange * Double.Parse(TextExtractorEbayCoUk.ExtractEbayCoUkStartPrice(price));

                    double endPrice = exchange * Double.Parse(TextExtractorEbayCoUk.ExtractEbayCoUkEndPrice(price));
                    //Console.WriteLine(clock+"\n"+ startPrice + "\n" + endPrice + "\n");
                    double shipping;
                    if (null != node.SelectSingleNode("//span[@class='ship']"))
                    {
                        shipping = exchange * Double.Parse(TextExtractorEbayCoUk.ExtractShippingEbayCoUk(node.SelectSingleNode(".//span[@class='ship']").InnerText));
                    }
                    else
                    {
                        shipping = 0;
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
                        Console.Write("+");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Problem with creating Ebayitem");
                    Console.WriteLine(e.Message);
                }                   
            }
            return list;
        }

        public static EbayCoUkGrimm GetInstance(SearchRequestHandler handler)
        {
            _grim = new EbayCoUkGrimm(handler);
            return _grim;
        }

        //create url of the next page, using the current one
        protected override string NextPage(string url, int current)
        {
            string pgn = "pgn=" + current;
            string vervanging = "pgn=" + (current + 1);
            //Console.WriteLine("=======CreatedURL======="+ url.Replace(pgn, vervanging));
            return url.Replace(pgn, vervanging);
        }
    }
}
