using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareAlacCrawler.Models
{
    public class Archive
    {
        public Archive()
        {
            IgnoredItems = new List<Filter>();
            PreviouslyCopiedTracks = new List<Track>();
        }

        public List<Filter> IgnoredItems
        {
            get;
            set;
        }

        public List<Track> PreviouslyCopiedTracks
        {
            get;
            set;
        }
    }
}
