using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EbayCrawlerWPF.Model
{
    [Serializable()]
    public class Jsonurl
    {
        [XmlAttribute]
        public string Name {
            get;
            set;
        }
        [XmlArray("SearchRequests"), XmlArrayItem(typeof(SearchRequest), ElementName = "SearchRequest")]
        public List<SearchRequest> SearchRequests
        {
            get;
            set;
        }

        public void PrintContent()
        {
            Console.WriteLine("<SearchRequests>");
            foreach (SearchRequest sr in SearchRequests)
            {
                Console.WriteLine("<SearchRequest> ");
                sr.PrintContent();
                Console.WriteLine("</SearchRequest> ");
            }
            Console.WriteLine("</SearchRequests>");
        }
    }
}
