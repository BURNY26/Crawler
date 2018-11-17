using EbayCrawlerWPF.Model;
using System;
using System.Text.RegularExpressions;

namespace EbayCrawlerWPF.model
{
    public class TextExtractorEbayCom
    {
        public static DateTime ExtractEbayComTimeLeft(string txt, DateTime now)
        {

            string pattern = "((\\d*)w)?(\\s)?((\\d*)d)?(\\s)?((\\d*)h)?(\\s)?((\\d*)m)?(\\s)?((\\d*)s)?(\\s)?left";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(txt);

            if (matches.Count >0)
            {
                DateTime today = now;
                /*
                Console.WriteLine("==w==" + (matches[1].Groups[2].ToString() == "" ? "0" : matches[1].Groups[2].ToString()));
                Console.WriteLine("==w==" + (matches[1].Groups[5].ToString() == "" ? "0" : matches[1].Groups[5].ToString()));
                Console.WriteLine("==w==" + (matches[1].Groups[8].ToString() == "" ? "0" : matches[1].Groups[8].ToString()));
                Console.WriteLine("==w==" + (matches[1].Groups[11].ToString() == "" ? "0" : matches[1].Groups[11].ToString()));
                Console.WriteLine("==w==" + (matches[1].Groups[14].ToString() == "" ? "0" : matches[1].Groups[14].ToString()));
                */
                today = today.AddDays(7 * int.Parse((matches[1].Groups[2].ToString() == "" ? "0" : matches[1].Groups[2].ToString())));
                today = today.AddDays(int.Parse((matches[1].Groups[5].ToString() == "" ? "0" : matches[1].Groups[5].ToString())));
                today = today.AddHours(int.Parse((matches[1].Groups[8].ToString() == "" ? "0" : matches[1].Groups[8].ToString())));
                today = today.AddMinutes(int.Parse((matches[1].Groups[11].ToString() == "" ? "0" : matches[1].Groups[11].ToString())));
                today = today.AddSeconds(int.Parse((matches[1].Groups[14].ToString() == "" ? "0" : matches[1].Groups[14].ToString())));

                return today;
            }
            return now;
        }

        public static string ExtractEbayComLocation(string txt)
        {
            String location = txt.Replace("From ", "");
            if (txt.Contains("From"))
            {
                return location;
            } else
            {                
                return null;
            }
        }

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

        public static String ExtractPicUrl(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r", "");
            string pattern = "=\"([^\"]*jpg)";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = rgx.Matches(lineabbrev.TrimStart());
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].ToString();
            }
            else
            {
                return "";
            }
        }

        public static double ExtractShipping(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r", "");
            lineabbrev = Regex.Replace(lineabbrev, @",", "");
            lineabbrev = Regex.Replace(lineabbrev, @"\.", ",");
            string pattern = "\\$(\\d+,\\d+)\\sshipping";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = rgx.Matches(lineabbrev.TrimStart());
            if (matches.Count > 0)
            {
                return Double.Parse(matches[0].Groups[1].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static int ExtractFollowersEbayCom(string txt)
        {
            string lineabbrev = Regex.Replace(txt, @"\t|\n|\r", "");
            string pattern = "(\\d+)\\s*Watching";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(lineabbrev);
            if (matches.Count == 1)
            {
                return int.Parse(matches[0].Groups[1].ToString());
            }
            return 0;
        }

        public static bool DetermineIfLinkIsJpg(string txt)
        {
            string pattern = "\\.jpg";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(txt);
            if (matches.Count == 1)
            {
                return true;
            }
            return false;
        }

        public static string ExtractNextPage(string line)
        {
            string pattern = "Volgende pagina met resultaten.*href=\"(\\S+.?)\"";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(line);
            if (matches.Count == 1)
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
            int newAmount = 150 + oldAmount;
            return line.Replace("skc=" + oldAmount, "skc=" + newAmount);
        }

        public static string extractUrl(string line)
        {
            string pattern = "href=\"(\\S+)\"";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(line);
            if (matches.Count == 1)
            {
                return matches[0].Groups[1].ToString();
            }
            else
            {
                throw new Exception("Meer of minder dan 1 mogelijke itemurl!");
            }
        }

        public static string ExtractEbayComTitle(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r|;", "");
            string pattern = "(.*?)\\$";
            Regex rgx = new Regex(pattern, RegexOptions.Multiline);
            MatchCollection matches = rgx.Matches(lineabbrev.TrimStart());
            return lineabbrev;
        }

        public static string ExtractEbayComStartPrice(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r", "");
            lineabbrev = Regex.Replace(lineabbrev, @",", "");
            lineabbrev = Regex.Replace(lineabbrev, @"\.", ",");
            string pattern = "^\\$(\\d+,\\d+)";
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

        public static string ExtractEbayComEndPrice(string line)
        {
            string lineabbrev = Regex.Replace(line, @"\t|\n|\r", "");
            lineabbrev = Regex.Replace(lineabbrev, @",", "");
            lineabbrev = Regex.Replace(lineabbrev, @"\.", ",");
            string pattern = "to \\$(\\d+,\\d+)";
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
