using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareAlacCrawler
{
    public class Constants
    {
        //public const string ParentDirectoryPath = @"E:\Dropbox\iTunes\Music\LCD Soundsystem\Sound of Silver";
        public const string ParentDirectoryPath = @"E:\Dropbox\iTunes\Music";
        public const string ExportDirectoryPath = @"E:\AlacCrawlerOutput";
        public const string TargetFileEncoding = "alac";
        public const string TargetFileExtension = "m4a";
        public const string ArchiveFileName = "Archive.xml";

        public class Commands
        {
            public const string ClearHistoryCommand = "clearHistory";
            public const string ClearFiltersCommand = "clearFilters";
            public const string AddNewIgnoredPerformerCommand = "addNewIgnoredPerformer";
            public const string AddNewIgnoredAlbumCommand = "addNewIgnoredAlbum";
            public const string AddNewIgnoredTrackNameCommand = "addNewIgnoredTrack";
            public const string PerformCopyCommand = "performCopy";
            public const string PerformTestCopyCommand = "performTest";
            public const string QuitCommand = "quit";
        }
    }
}
