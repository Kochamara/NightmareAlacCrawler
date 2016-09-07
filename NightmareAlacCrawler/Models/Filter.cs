using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareAlacCrawler.Models
{
    public class Filter
    {
        public string MetadataLabel
        {
            get;
            set;
        }

        public FilterType MetadataType
        {
            get;
            set;
        }
    }

    public enum FilterType
    {
        Title = 0,
        Performer = 1,
        Album = 2
    }
}
