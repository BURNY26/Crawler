using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EbayCrawlerWPF.Model
{
    [Serializable()]
    public class SearchRequest
    {
        [XmlAttribute]
        public int Id
        {
            get; set;
        }

        public List<string> PrimaryKeywords
        {
            get; set;
        }

        public List<string> SecondaryKeywords
        {
            get; set;
        }

        public List<string> ExclusiveKeywords
        {
            get; set;
        }
        public void PrintContent()
        {
            Console.WriteLine("<PrimaryKeywords>");
            foreach (string str in PrimaryKeywords)
            {
                Console.Write("<string> ");
                Console.Write(str);
                Console.WriteLine("</string> ");
            }
            Console.WriteLine("</PrimaryKeywords>");
            Console.WriteLine("<SecondaryKeywords>");
            foreach (string str in SecondaryKeywords)
            {
                Console.Write("<string> ");
                Console.Write(str);
                Console.WriteLine("</string> ");
            }
            Console.WriteLine("</SecondaryKeywords>");
            Console.WriteLine("<ExclusiveKeywords>");
            foreach (string str in ExclusiveKeywords)
            {
                Console.Write("<string> ");
                Console.Write(str);
                Console.WriteLine("</string> ");
            }
            Console.WriteLine("</ExclusiveKeywords>");
        }
    }
}
