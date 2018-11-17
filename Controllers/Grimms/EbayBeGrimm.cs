using System;
using System.IO;
using System.Text;
using HtmlAgilityPack;

using System.Collections.Generic;
using EbayCrawlerWPF.Controllers.Grimms;
using Newtonsoft.Json;
using EbayCrawlerWPF.Model;

namespace EbayCrawlerWPF.model
{
    public class EbayBeGrimm : AGrimm
    {

        private static EbayBeGrimm _grim;        

        private EbayBeGrimm()
        {
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
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//li[@class='sresult lvresult clearfix li' or @class='sresult lvresult clearfix li shic']"))
            {
                string id = node.Id.TrimEnd();
                string title = TextExtractorEbayBe.ExtractName(node.SelectSingleNode("h3").SelectSingleNode("a").InnerText);
                string url = node.SelectSingleNode("h3").SelectSingleNode("a").Attributes["href"].Value.TrimEnd();
                string price = node.SelectSingleNode("ul[@class='lvprices left space-zero' or @class='lvprices left space-zero conprices']").SelectSingleNode("li[@class='lvprice prc']").SelectSingleNode("span[@class='bold']").InnerText.TrimStart().TrimEnd();
                double startPrice = Double.Parse(TextExtractorEbayBe.ExtractEbayBeStartPrice(price));
                double endPrice = Double.Parse(TextExtractorEbayBe.ExtractEbayBeEndPrice(price));
                string timems = TextExtractorEbayBe.ExtractTime(node.InnerHtml);
                string location = TextExtractorEbayBe.ExtractEbayBeLocation(node.SelectSingleNode("ul[@class='lvdetails left space-zero full-width']").InnerHtml);
                Double shipping = Double.Parse(TextExtractorEbayBe.ExtractEbayBeShipping(node.SelectSingleNode("ul[@class='lvprices left space-zero' or @class='lvprices left space-zero conprices']").SelectSingleNode("li[@class='lvshipping']").OuterHtml));
                DateTime enddate = GetEnddate(timems);
                string picurl = node.SelectSingleNode(".//div[@class='lvpicinner full-width picW']").SelectSingleNode(".//img").Attributes["src"].Value;
                int followers = Int32.Parse(TextExtractorEbayBe.ExtractFollowersEbayBe(node.InnerHtml));
                if (startPrice > 0)
                {
                    ei = new EbayItem(startPrice, endPrice, title, url, id, enddate, followers, shipping, location, picurl);
                    list.Add(ei);
                    Console.Write("+");
                }
            }
            Console.WriteLine();
            return list;            
        }

        public static EbayBeGrimm GetInstance()
        {
            _grim = new EbayBeGrimm();
            return _grim;
        }

        //er is iets speciaal aan de volgende x items opvragen van ebay ,wnr je de skc param wijzigt krijg je de volgende 50 items
        protected override string NextPage(string url, int current)
        {
            string pgn = "pgn=" + current;
            string vervanging = "pgn=" + (current + 1);
            //Console.WriteLine("=======CreatedURL======="+ url.Replace(pgn, vervanging));
            return url.Replace(pgn, vervanging);
        }
    }
}
