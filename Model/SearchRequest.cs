using System.Collections.Generic;

namespace EbayCrawlerWPF.Model
{
    public class SearchRequest
    {
        public int Id
        {
            get; set;
        }

        public List<SearchKeyword> PrimaryKeywords
        {
            get; set;
        }

        public List<SearchKeyword> SecondaryKeywords
        {
            get; set;
        }

        public List<SearchKeyword> ExclusiveKeywords
        {
            get; set;
        }
    }
}
