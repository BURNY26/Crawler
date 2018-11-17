using EbayCrawlerWPF.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbayCrawlerWPF.Model
{
    public class Helper
    {
        public static void WriteMap(Dictionary<String ,List<String>> map,String tag)
        {
            Console.WriteLine("==================<"+tag+">====================");
            foreach (KeyValuePair<String, List<String>> entry in map)
            {
                String key = entry.Key;
                Console.WriteLine("key: "+key+" has "+entry.Value.Count+" values");
                foreach (String s in entry.Value)
                {
                    Console.WriteLine("\t" + s);                   
                }
            }
            Console.WriteLine("==================>" + tag + "<====================");
        }
        public static void WriteMapEbayItems(Dictionary<String, List<EbayItem>> map, String tag)
        {
            Console.WriteLine("==================<" + tag + ">====================");
            foreach (KeyValuePair<String, List<EbayItem>> entry in map)
            {
                String key = entry.Key;
                Console.WriteLine("keywords : " + key + " has " + entry.Value.Count + " values");
                foreach (EbayItem s in entry.Value)
                {
                    Console.WriteLine("\t" + s.ToString());
                }
            }
            Console.WriteLine("==================>" + tag + "<====================");
        }
    }

   
}

