using System;
using System.IO;
using System.Text;
using EbayCrawlerWPF.model;
using System.Collections.Generic;

namespace EbayCrawlerWPF.Model
{
    public class CsvController
    {
        public static void CreateDocument(List<EbayItem> list, String filename)
        {
            var csv = new StringBuilder();
            foreach (EbayItem ei in list)
            {
                var first = ei.ItemID;
                var second = ei.Title;
                var third = ei.Startprice;
                var fourth = ei.Endprice;
                var fifth = ei.Shipping;
                var sixth = ei.Followers;
                var seventh = ei.Url;
                var eight = ei.PictureUrl;
                var ninth = ei.Location;
                var tenth = ei.Date;
                var newLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth);
                csv.AppendLine(newLine);
            }

            File.WriteAllText(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Csv"), filename), csv.ToString());
        }
    }
}
