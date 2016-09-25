using NightmareAlacCrawler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NightmareAlacCrawler.Services
{
    public class FileExportAgent
    {
        public const bool ExportAllFilesToSameDirectory = true;

        public async static Task CopyTracksAsync(List<Track> tracksToCopy)
        {
            Console.WriteLine("Begining file copy...");
            foreach (var track in tracksToCopy)
            {
                bool success = await CopyTrackAsync(track);
                if (!success)
                {
                    Console.WriteLine("Warning! Unable to copy " + track.Title);
                }
            }
            Console.WriteLine("File copy completed. Output located in " + Constants.ExportDirectoryPath);
        }

        public async static Task<bool> CopyTrackAsync(Track track)
        {
            bool success = false;

            try
            {
                if (!System.IO.Directory.Exists(Constants.ExportDirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(Constants.ExportDirectoryPath);
                }

                string destinationPath = string.Empty;

                if (ExportAllFilesToSameDirectory)
                {
                    var originalFile = new FileInfo(track.FilePath);
                    var longFileName = RemoveSpecialCharacters(track.Performer + " " + track.Album + " ") + originalFile.Name;
                    destinationPath = Path.Combine(Constants.ExportDirectoryPath, longFileName);
                }
                else
                {
                    string artistDirectory = Path.Combine(Constants.ExportDirectoryPath, RemoveSpecialCharacters(track.Performer));
                    if (!System.IO.Directory.Exists(artistDirectory))
                    {
                        System.IO.Directory.CreateDirectory(artistDirectory);
                    }

                    string albumDirectory = Path.Combine(artistDirectory, RemoveSpecialCharacters(track.Album));
                    if (!System.IO.Directory.Exists(albumDirectory))
                    {
                        System.IO.Directory.CreateDirectory(albumDirectory);
                    }

                    var originalFile = new FileInfo(track.FilePath);
                    destinationPath = Path.Combine(albumDirectory, originalFile.Name);
                }
                
                System.IO.File.Copy(track.FilePath, destinationPath, true);
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to copy " + track.Title);
                Console.WriteLine(e);
            }

            return success;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_. ]+", "", RegexOptions.Compiled);
        }
    }
}