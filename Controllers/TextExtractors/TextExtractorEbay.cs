using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EbayCrawlerWPF.Controllers.TextExtractors
{
    public class TextExtractorEbay
    {
        //er is iets speciaal aan de volgende x items opvragen van ebay ,wnr je de skc param wijzigt krijg je de volgende 50 items
        public static string CreateUrl(string url, int current)
        {
            string pgn = "skc=" + current;
            string vervanging = "skc=" + (current+50);
            //Console.WriteLine("=======CreatedURL======="+ url.Replace(pgn, vervanging));
            return url.Replace(pgn, vervanging);
        }

        public static String FindNextPage(String content)
        {
            string pattern = "\"pg \" href=\"(.*)\"";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(content);
            Console.WriteLine(matches.Count);
            foreach(Match s in matches)
            {
                Console.WriteLine(s.Value);
            }
            
            
                return matches[0].Groups[1].ToString();
            
            
        }
    }
}
