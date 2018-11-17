using System;
using System.Configuration;

namespace EbayCrawlerWPF.AppConfig
{
    class SitesSection : ConfigurationSection
    {
        public const String SectionName = "SitesSection";

        public SitesSection()
        {
        }

        [ConfigurationProperty("Sites", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SitesCollection),
        AddItemName = "Site",
        ClearItemsName = "clear",
        RemoveItemName = "remove")]
        public SitesCollection Sites
        {
            get
            {
                SitesCollection sitescoll = (SitesCollection)base["Sites"];
                return sitescoll;
            }
            set
            {
                SitesCollection sitescoll = value;
            }
        }


        public class SitesCollection : ConfigurationElementCollection
        {
            public SitesCollection()
            {
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return new Site();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((Site)element).Name;
            }

            public override ConfigurationElementCollectionType CollectionType
            {
                get
                {
                    return ConfigurationElementCollectionType.AddRemoveClearMap;
                }
            }

            public Site this[int index]
            {
                get
                {
                    return (Site)BaseGet(index);
                }
                set
                {
                    if (BaseGet(index) != null)
                    {
                        BaseRemoveAt(index);
                    }
                    BaseAdd(index, value);
                }
            }

            new public Site this[String Name]
            {
                get
                {
                    return (Site)BaseGet(Name);
                }
            }

            public int IndexOf(Site url)
            {
                return BaseIndexOf(url);
            }

            public void Add(Site url)
            {
                BaseAdd(url);

                // Your custom code goes here.
            }

            protected override void BaseAdd(ConfigurationElement element)
            {
                base.BaseAdd(element, false);

                // Your custom code goes here.
            }

            public void Remove(Site url)
            {
                if (BaseIndexOf(url) >= 0)
                {
                    BaseRemove(url.Name);
                    // Your custom code goes here.
                    Console.WriteLine("Sites : {0} ", "Removed Site! ");
                }
            }

            public void RemoveAt(int index)
            {
                BaseRemoveAt(index);
            }

            public void Remove(String name)
            {
                BaseRemove(name);
            }

            public void Clear()
            {
                BaseClear();
                // Your custom code goes here.
                Console.WriteLine("Sites : {0} ", "Removed All Sites! ");
            }
        }

        //iets om alle instanties mee op te halen

        public class Site : ConfigurationElement
        {
            public const String SectionName = "Site";

            public Site()
            {
            }

            [ConfigurationProperty("Name")]
            public String Name
            {
                get
                {
                    return (String)this["Name"];
                }
                set
                {
                    this["Name"] = value;
                }
            }

            [ConfigurationProperty("BestMatch", DefaultValue = "sop=12.")]
            [RegexStringValidator(@".*sop=12\D.*")]
            public String BestMatch
            {
                get
                {
                    return (String)this["BestMatch"];
                }
                set
                {
                    this["BestMatch"] = value;
                }
            }

            [ConfigurationProperty("EndingSoonest", DefaultValue = "sop=1.")]
            [RegexStringValidator(@".*sop=1\D.*")]
            public String EndingSoonest
            {
                get
                {
                    return (String)this["EndingSoonest"];
                }
                set
                {
                    this["EndingSoonest"] = value;
                }
            }

            [ConfigurationProperty("NewlyListed", DefaultValue = "sop=10.")]
            [RegexStringValidator(@".*sop=10\D.*")]
            public String NewlyListed
            {
                get
                {
                    return (String)this["NewlyListed"];
                }
                set
                {
                    this["NewlyListed"] = value;
                }
            }

            [ConfigurationProperty("HighestPriceTransport", DefaultValue = "sop=16.")]
            [RegexStringValidator(@".*sop=16\D.*")]
            public String HighestPriceTransport
            {
                get
                {
                    return (String)this["HighestPriceTransport"];
                }
                set
                {
                    this["HighestPriceTransport"] = value;
                }
            }

            [ConfigurationProperty("LowestPriceTransport", DefaultValue = "sop=15.")]
            [RegexStringValidator(@".*sop=15\D.*")]
            public String LowestPriceTransport
            {
                get
                {
                    return (String)this["LowestPriceTransport"];
                }
                set
                {
                    this["LowestPriceTransport"] = value;
                }
            }

            [ConfigurationProperty("NearestFirst", DefaultValue = "sop=7.")]
            [RegexStringValidator(@".*sop=7\D.*")]
            public String NearestFirst
            {
                get
                {
                    return (String)this["NearestFirst"];
                }
                set
                {
                    this["NearestFirst"] = value;
                }
            }

            [ConfigurationProperty("SecondUse", DefaultValue = "sop=19.")]
            [RegexStringValidator(@".*sop=19\D.*")]
            public String SecondUse
            {
                get
                {
                    return (String)this["SecondUse"];
                }
                set
                {
                    this["SecondUse"] = value;
                }
            }

            [ConfigurationProperty("HighestPrice", DefaultValue = "sop=3.")]
            [RegexStringValidator(@".*sop=3\D.*")]
            public String HighestPrice
            {
                get
                {
                    return (String)this["HighestPrice"];
                }
                set
                {
                    this["HighestPrice"] = value;
                }
            }

            [ConfigurationProperty("LowestPrice", DefaultValue = "sop=2.")]
            [RegexStringValidator(@".*sop=2\D.*")]
            public String LowestPrice
            {
                get
                {
                    return (String)this["LowestPrice"];
                }
                set
                {
                    this["LowestPrice"] = value;
                }
            }

            //meerdere keywords is met '|'
            [ConfigurationProperty("KW", DefaultValue = "default")]
            public String KW
            {
                get
                {
                    return (String)this["KW"];
                }
                set
                {
                    this["KW"] = value;
                }
            }

        }
    }
}
