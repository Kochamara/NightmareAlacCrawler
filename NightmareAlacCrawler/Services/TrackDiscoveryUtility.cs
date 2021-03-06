﻿using NightmareAlacCrawler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareAlacCrawler.Services
{
    public class TrackDiscoveryUtility
    {
        private TrackHistoryManager _historyManager;
        private const int NumberOfTracksToCollectInTestMode = 50;

        public TrackDiscoveryUtility()
        {
            _historyManager = TrackHistoryManager.GetInstance();
        }

        public List<Track> RecursivelyCollectTracks(string directoryPath, bool testMode)
        {
            var tracks = new List<Track>();

            try
            {
                foreach (string filePath in Directory.GetFiles(directoryPath))
                {
                    var file = GetTrackModelIfValid(filePath);
                    if (file != null)
                    {
                        tracks.Add(file);
                        _historyManager.AddPreviouslyCopiedTrackToArchive(file);
                    }
                }

                foreach (string subDirectoryPath in Directory.GetDirectories(directoryPath))
                {
                    // Given iTunes' folder structure, we're going to overshoot NumberOfTracksToCollectInTestMode, but it's
                    // also not like we're going to overshoot it by THAT much.
                    if (!testMode || tracks.Count() < NumberOfTracksToCollectInTestMode)
                    {
                        var newTracks = RecursivelyCollectTracks(subDirectoryPath, testMode);
                        tracks.AddRange(newTracks);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }

            return tracks;
        }

        private Track GetTrackModelIfValid(string filePath)
        {
            Track validTrack = null;

            if (!String.IsNullOrWhiteSpace(filePath) && filePath.EndsWith(Constants.TargetFileExtension))
            {
                var fileMetadata = TagLib.File.Create(filePath);
                if ((fileMetadata != null) && (fileMetadata.Tag != null) && (fileMetadata.Properties != null) && !String.IsNullOrWhiteSpace(fileMetadata.Properties.Description) && fileMetadata.Properties.Description.Contains(Constants.TargetFileEncoding))
                {
                    var track = new Track()
                    {
                        Title = fileMetadata.Tag.Title,
                        Performer = fileMetadata.Tag.FirstPerformer,
                        Album = fileMetadata.Tag.Album,
                        FilePath = filePath,
                    };

                    bool isIgnored = _historyManager.IsTrackOnIgnoreList(track);
                    bool wasPreviouslyCopied = _historyManager.WasTrackPreviouslyCopied(track);

                    if (!isIgnored && !wasPreviouslyCopied)
                    {
                        validTrack = track;
                    }
                }
            }

            return validTrack;
        }

        //private string GetHash(string filePath)
        //{
        //    string hash = null;

        //    if (!String.IsNullOrWhiteSpace(filePath))
        //    {
        //        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        //        {
        //            using (var stream = File.OpenRead(filePath))
        //            {
        //                hash = Encoding.Default.GetString(sha256.ComputeHash(stream));
        //            }
        //        }
        //    }

        //    return hash;
        //}
    }
}
