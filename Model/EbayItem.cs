using System;

namespace EbayCrawlerWPF.model
{
    public class EbayItem
    {
        public double Startprice
        {
            get;
        }

        public double Endprice
        {
            get;
        }

        public string Title
        {
            set;
            get;
        }

        public string Url
        {
            set;
            get;
        }

        public string ItemID
        {
            get;
        }

        public DateTime Date
        {
            get;
        }

        public int Followers
        {
            get;
        }

        public double Shipping
        {
            get;
        }

        public string Location
        {
            get;
        }

        public string PictureUrl
        {
            get;
        }

        public EbayItem()
        {
        }

        public EbayItem(string title, DateTime date, double endPrice)
        {
            Title = title;
            Date = date;
            Endprice = endPrice;
        }

        public EbayItem(double startprice, double endprice, string title, string url, string itemID, DateTime date, int followers, double shipping, string location, string picurl)
        {
            Startprice = startprice;
            Endprice = endprice;
            Title = title;
            Url = url;
            ItemID = itemID;
            Date = date;
            Followers = followers;
            Shipping = shipping;
            Location = location;
            PictureUrl = picurl;
        }
             
        public override string ToString()
        {
            return "Debug EbayItem : " +
            "itemid : " + ItemID + "\n" +
            "title : " + Title + "\n" +
            "startprice : " + Startprice + "\n" +
            "endprice : " + Endprice + "\n" +
            "shipping : " + Shipping + "\n" +
            "location : " + Location+ "\n" +
            "date : " + Date + "\n" +
            "followers : " + Followers + "\n" +
            "url : " + Url + "\n"+
            "picurl : " + PictureUrl + "\n";
        }
    }
}
