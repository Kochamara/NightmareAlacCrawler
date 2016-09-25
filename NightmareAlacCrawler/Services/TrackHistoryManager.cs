using NightmareAlacCrawler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NightmareAlacCrawler.Services
{
    public class TrackHistoryManager
    {
        private static TrackHistoryManager _instance;
        private Archive _fileHistory;
        private string _archiveFilePath;

        public static TrackHistoryManager GetInstance()
        {
            return _instance ?? (_instance = new TrackHistoryManager());
        }

        private TrackHistoryManager()
        {
            _archiveFilePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), Constants.ArchiveFileName);
            _fileHistory = LoadArchive();
        }

        private Archive LoadArchive()
        {
            Archive archive = null;

            try
            {
                using (FileStream stream = File.Open(_archiveFilePath, FileMode.Open))
                {
                    if (stream != null)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Archive));
                        archive = (Archive)serializer.Deserialize(stream);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                archive = new Archive();
                Console.WriteLine("New archive being created");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return archive;
        }

        public bool SaveArchive()
        {
            bool success = false;

            try
            {
                using (FileStream stream = File.Open(_archiveFilePath, FileMode.OpenOrCreate))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Archive));
                    var writer = XmlWriter.Create(stream);
                    serializer.Serialize(writer, _fileHistory);
                    success = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return success;
        }

        public void AddPreviouslyCopiedTrackToArchive(Track track)
        {
            if (track != null)
            {
                _fileHistory.PreviouslyCopiedTracks.Add(track);
            }
        }

        public bool WasTrackPreviouslyCopied(Track track)
        {
            bool foundMatch = false;

            if (_fileHistory.PreviouslyCopiedTracks.Any(x => x.Title.Equals(track.Title) && x.Album.Equals(track.Album) && x.FilePath.Equals(track.FilePath) && x.Performer.Equals(track.Performer)))
            {
                foundMatch = true;
            }

            return foundMatch;
        }

        public void RecordNewTracksToArchive(List<Track> tracks)
        {
            _fileHistory.PreviouslyCopiedTracks.AddRange(tracks);
        }

        public bool IsTrackOnIgnoreList(Track track)
        {
            bool ignoreTrack = false;

            if ((track != null) && (_fileHistory.IgnoredItems != null) && (_fileHistory.IgnoredItems.Count > 0))
            {
                foreach (var filter in _fileHistory.IgnoredItems)
                {
                    if (!ignoreTrack)
                    {
                        if (!String.IsNullOrWhiteSpace(filter.MetadataLabel))
                        {
                            switch (filter.MetadataType)
                            {
                                case FilterType.Performer:
                                    if (!String.IsNullOrWhiteSpace(track.Performer) && String.Equals(track.Performer, filter.MetadataLabel, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ignoreTrack = true;
                                    }
                                    break;
                                case FilterType.Album:
                                    if (!String.IsNullOrWhiteSpace(track.Album) && String.Equals(track.Album, filter.MetadataLabel, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ignoreTrack = true;
                                    }
                                    break;
                                case FilterType.Title:
                                default:
                                    if (!String.IsNullOrWhiteSpace(track.Title) && String.Equals(track.Title, filter.MetadataLabel, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ignoreTrack = true;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Warning! There's a metadata filter without a label. Why did this happen?");
                        }
                    }
                }
            }

            return ignoreTrack;
        }

        public void RecordNewFilterToIgnoreList(Filter filter)
        {
            _fileHistory.IgnoredItems.Add(filter);
        }

        public void ClearPreviouslyRecordedTracks()
        {
            _fileHistory.PreviouslyCopiedTracks = new List<Track>();
        }

        public void ClearPreviouslyEnteredFilters()
        {
            _fileHistory.IgnoredItems = new List<Filter>();
        }
    }
}