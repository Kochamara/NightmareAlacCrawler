using NightmareAlacCrawler.Services;
using NightmareAlacCrawler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NightmareAlacCrawler
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Welcome to the NightmareAlacCrawler! I'm so sorry that you give a fuck about lossless audio, but here we are!\n");
                PrintCommands();

                var discoveryUtility = new TrackDiscoveryUtility();
                var historyManager = TrackHistoryManager.GetInstance();
                string command = string.Empty;

                while (!String.Equals(command, Constants.Commands.QuitCommand, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Enter your command!");
                    command = Console.ReadLine().Trim();

                    if (command.Equals(Constants.Commands.PerformCopyCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        var tracksToCopy = discoveryUtility.RecursivelyCollectTracks(Constants.ParentDirectoryPath, false, false);
                        historyManager.SaveArchive();
                        var task = FileExportAgent.CopyTracksAsync(tracksToCopy);
                    }
                    else if (command.Equals(Constants.Commands.PerformTestCopyCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        var tracksToCopy = discoveryUtility.RecursivelyCollectTracks(Constants.ParentDirectoryPath, false, true);
                        historyManager.SaveArchive();
                        var task = FileExportAgent.CopyTracksAsync(tracksToCopy);
                    }
                    else if (command.Equals(Constants.Commands.AddNewIgnoredPerformerCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Enter the name of the performer to ignore:");
                        string ignored = Console.ReadLine();
                        historyManager.RecordNewFilterToIgnoreList(new Filter()
                        {
                            MetadataLabel = ignored,
                            MetadataType = FilterType.Performer
                        });
                        historyManager.SaveArchive();
                        Console.WriteLine("Got it!");
                    }
                    else if (command.Equals(Constants.Commands.AddNewIgnoredAlbumCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Enter the name of the album to ignore:");
                        string ignored = Console.ReadLine();
                        historyManager.RecordNewFilterToIgnoreList(new Filter()
                        {
                            MetadataLabel = ignored,
                            MetadataType = FilterType.Album
                        });
                        historyManager.SaveArchive();
                        Console.WriteLine("Got it!");
                    }
                    else if (command.Equals(Constants.Commands.AddNewIgnoredTrackNameCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Enter the name of the track title to ignore:");
                        string ignored = Console.ReadLine();
                        historyManager.RecordNewFilterToIgnoreList(new Filter()
                        {
                            MetadataLabel = ignored,
                            MetadataType = FilterType.Title
                        });
                        historyManager.SaveArchive();
                        Console.WriteLine("Got it!");
                    }
                    else if (command.Equals(Constants.Commands.ClearHistoryCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        historyManager.ClearPreviouslyRecordedTracks();
                    }
                    else if (command.Equals(Constants.Commands.ClearFiltersCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        historyManager.ClearPreviouslyEnteredFilters();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }

        public static void PrintCommands()
        {
            Console.WriteLine("========================================================================================");
            Console.WriteLine("Commands:\n");
            Console.WriteLine(Constants.Commands.PerformCopyCommand + " - Performs copy of ALAC files");
            Console.WriteLine(Constants.Commands.PerformTestCopyCommand + " - (Test function) Performs copy of first 50 ALAC files");
            Console.WriteLine(Constants.Commands.AddNewIgnoredPerformerCommand + " - Allows you to define a new performer to ignore in the file copy");
            Console.WriteLine(Constants.Commands.AddNewIgnoredAlbumCommand + " - Allows you to define a new album to ignore in the file copy");
            Console.WriteLine(Constants.Commands.AddNewIgnoredTrackNameCommand + " - Allows you to define a new track name to ignore in the file copy");
            Console.WriteLine(Constants.Commands.ClearHistoryCommand + " - Clears the history of previously copied tracks");
            Console.WriteLine(Constants.Commands.ClearFiltersCommand + " - Clears the history of previously filtered out tracks");
            Console.WriteLine(Constants.Commands.QuitCommand + " - Escape the nightare");
            Console.WriteLine("========================================================================================\n");
        }
    }
}