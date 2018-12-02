using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EbayCrawlerWPF.Model
{
    [Serializable()]
    [XmlRoot("ArrayOfJsonurl")]
    public class ArrayOfJsonurl
    {

        [XmlArray("Jsonurls"), XmlArrayItem(typeof(Jsonurl), ElementName = "Jsonurl")]
        public List<Jsonurl> Jsonurls
        {
            get;
            set;
        }

        public void PrintContent()
        {
            Console.WriteLine("<ArrayOfJsonurl>\n<Jsonurls> ");
            foreach(Jsonurl url in Jsonurls)
            {
                Console.WriteLine("<Jsonurl name=\""+url.Name+"\"> ");
                url.PrintContent();
                Console.WriteLine("</Jsonurl> ");
            }
            Console.WriteLine("</Jsonurls>\n</ArrayOfJsonurl> ");
        }
    }
}
