using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EbayCrawlerWPF.Controllers.TextExtractors
{
    public class TextExtractorEbayCoUk
    {
        public static string ExtractTitle(string txt)
        {
            string lineabbrev = Regex.Replace(txt, @"\t|\n|\r|;", "");
            lineabbrev = Regex.Replace(lineabbrev, @"New listing", "");
            string pattern = "Click\\sthis\\slink\\sto\\saccess\\s(.*)";
            Regex rgx = new Regex(pattern, RegexOptions.Multiline);
            MatchCollection matches = rgx.Matches(lineabbrev.TrimStart());
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].ToString();
            }
            return null;
        }

        public static string ExtractFollowers(string txt)
        {
            string lineabbrev = Regex.Replace(txt, @"\t|\n|\r|;", "");
            lineabbrev = Regex.Replace(lineabbrev, @"\+", "");
            string pattern = "(\\d*)\\sWatching";
            Regex rgx = new Regex(pattern, RegexOptions.Multiline);
            MatchCollection matches = rgx.Matches(lineabbrev.TrimStart());
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].ToString();
            } else
            {
                return "0";
            }
        }

        public static string ExtractLocation(string txt)
        {
            string pattern = "From\\s(.*?)<";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(txt);
            if (matches.Count == 1)
            {
                //Console.WriteLine("==========ExtractedEbayBeLocation============" + matches[0].Groups[1].ToString());
                return matches[0].Groups[1].ToString();
            }
            else
            {
                return null;
            }
        }

        public static DateTime ExtractTime(string txt, DateTime now)
        {
            string pattern = "((\\d*)w)?(\\s)?((\\d*)d)?(\\s)?((\\d*)h)?(\\s)?((\\d*)m)?(\\s)?((\\d*)s)?(\\s)?left";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(txt);
            if (matches.Count == 1)
            {
                DateTime today = now;
                /*
                Console.WriteLine("==w==" + (matches[0].Groups[2].ToString() == "" ? "0" : matches[0].Groups[2].ToString()));
                Console.WriteLine("==w==" + (matches[0].Groups[5].ToString() == "" ? "0" : matches[0].Groups[5].ToString()));
                Console.WriteLine("==w==" + (matches[0].Groups[8].ToString() == "" ? "0" : matches[0].Groups[8].ToString()));
                Console.WriteLine("==w==" + (matches[0].Groups[11].ToString() == "" ? "0" : matches[0].Groups[11].ToString()));
                Console.WriteLine("==w==" + (matches[0].Groups[14].ToString() == "" ? "0" : matches[0].Groups[14].ToString()));
                */
                today = today.AddDays(7 * int.Parse((matches[0].Groups[2].ToString() == "" ? "0" : matches[0].Groups[2].ToString())));
                today = today.AddDays(int.Parse((matches[0].Groups[5].ToString() == "" ? "0" : matches[0].Groups[5].ToString())));
                today = today.AddHours(int.Parse((matches[0].Groups[8].ToString() == "" ? "0" : matches[0].Groups[8].ToString())));
                today = today.AddMinutes(int.Parse((matches[0].Groups[11].ToString() == "" ? "0" : matches[0].Groups[11].ToString())));
                today = today.AddSeconds(int.Parse((matches[0].Groups[14].ToString() == "" ? "0" : matches[0].Groups[14].ToString())));

                return today;
            }
            return now;
        }

        public static string ExtractEbayCoUkStartPrice(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r", "");
            lineabbrev = Regex.Replace(lineabbrev, @",", "");
            lineabbrev = Regex.Replace(lineabbrev, @"\.", ",");
            string pattern = "^\\£(\\d+,\\d+)";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = rgx.Matches(lineabbrev.TrimStart());
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].ToString();
            }
            else
            {
                return "0";
            }
        }

        public static string ExtractEbayCoUkEndPrice(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r", "");
            lineabbrev = Regex.Replace(lineabbrev, @",", "");
            lineabbrev = Regex.Replace(lineabbrev, @"\.", ",");
            string pattern = "to \\£(\\d+,\\d+)";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = rgx.Matches(lineabbrev.TrimStart());
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].ToString();
            }
            else
            {
                return "0";
            }
        }

        public static string ExtractShippingEbayCoUk(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r", "");
            lineabbrev = Regex.Replace(lineabbrev, @"\.", ",");
            string pattern = "\\£(\\d+,\\d+)\\spostage";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = rgx.Matches(lineabbrev.TrimStart());
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].ToString();
            }
            else
            {
                return "0";
            }
        }
    }
}
