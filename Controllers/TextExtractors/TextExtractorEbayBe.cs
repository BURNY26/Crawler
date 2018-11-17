using System;
using System.Text.RegularExpressions;

namespace EbayCrawlerWPF.model
{
    public class TextExtractorEbayBe
    {
        public static string ExtractItemIdFromUrl(string url)
        {
            url = url + ":";
            string pattern = "hash=(\\S*?):";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = rgx.Matches(url.TrimStart());
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].ToString();
            }
            else
            {
                throw new Exception("ERROR : TextExtractor ExtractItemIdFromUrl while analysing : " + url);
            }
        }

        public static bool DetermineIfLinkIsJpg(string txt)
        {
            string pattern = "\\.jpg";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(txt);
            if (matches.Count == 1)
            {
                return true ;
            }
            return false;
        }

        public static string ExtractEbayBeShipping(string txt)
        {
            string lineabbrev = Regex.Replace(txt, @"\t|\n|\r", "");
            lineabbrev = Regex.Replace(lineabbrev, @"\.", "");
            lineabbrev = Regex.Replace(lineabbrev, @"Nieuwe aanbieding", "");
            string pattern = "(\\d+,\\d+)\\sverzendkosten";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(lineabbrev);
            if (matches.Count == 1)
            {
                //Console.WriteLine("==========ExtractedEbayBeShipping============" + matches[0].Groups[1].ToString());
                return matches[0].Groups[1].ToString();
            }
            return "0";
        }

        public static string extractUrl(string v)
        {
            throw new NotImplementedException();
        }

        public static string ExtractEbayBeLocation(string txt)
        {
            string pattern = "Uit\\s(.*?)<";
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

        public static string ExtractNextPage(string line)
        {
            string pattern = "Volgende pagina met resultaten.*href=\"(\\S+.?)\"";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(line);
            if (matches.Count==1)
            {
                //return ReplaceAmountOfItems(matches[0].Groups[1].ToString());
                return matches[0].Groups[1].ToString();
            }
            else
            {
                throw new Exception("Meer of minder dan 1 mogelijke volgende pagina!");
            }           
        }

        private static string ReplaceAmountOfItems(string line)
        {
            string pattern = "skc=(\\d+)";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(line);

            int oldAmount = int.Parse(matches[0].Groups[1].ToString());
            int newAmount = 150+oldAmount;
            return line.Replace("skc=" + oldAmount, "skc=" + newAmount);
            
        }

        public static string ExtractName(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r|;", " ");
            string pattern = "(.*?)EUR";
            Regex rgx = new Regex(pattern, RegexOptions.Multiline);
            MatchCollection matches = rgx.Matches(lineabbrev.TrimStart());
            return lineabbrev;           
        }

        public static string ExtractFollowersEbayBe(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r", "");
            string pattern = "(\\d+) keer gevolgd";
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

        public static string ExtractEbayBeStartPrice(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r", "");
            lineabbrev = Regex.Replace(lineabbrev, @"\.", "");
            string pattern = "EUR (\\d+,\\d+)";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = rgx.Matches(lineabbrev.TrimStart());
            if (matches.Count >0)
            {
                return matches[0].Groups[1].ToString();
            }
            else
            {
                return "0";
            }
        }

        public static string ExtractEbayBeEndPrice(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r", "");
            lineabbrev = Regex.Replace(lineabbrev, @"\.", "");
            string pattern = "tot EUR (\\d+,\\d+)";
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

        public static string ExtractTime(string line)
        {
            string pattern = "timems=\"(\\d+)\"";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(line);
            if (matches.Count == 1)
            {
                return matches[0].Groups[1].ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
