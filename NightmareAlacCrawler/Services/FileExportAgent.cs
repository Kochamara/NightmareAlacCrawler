using NightmareAlacCrawler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareAlacCrawler.Services
{
    public class FileExportAgent
    {
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

                string artistDirectory = Path.Combine(Constants.ExportDirectoryPath, track.Performer);
                if (!System.IO.Directory.Exists(artistDirectory))
                {
                    System.IO.Directory.CreateDirectory(artistDirectory);
                }

                string albumDirectory = Path.Combine(artistDirectory, track.Album);
                if (!System.IO.Directory.Exists(albumDirectory))
                {
                    System.IO.Directory.CreateDirectory(albumDirectory);
                }

                //TODO: This should use the original file name instead
                string destinationPath = Path.Combine(albumDirectory, track.Title + "." + Constants.TargetFileExtension);
                System.IO.File.Copy(track.FilePath, destinationPath, true);

                //using (FileStream sourceStream = File.OpenRead(originalFilePath))
                //{
                //    using (FileStream destinationStream = File.Create(destinationFilePath))
                //    {
                //        await sourceStream.CopyToAsync(destinationStream);
                //        success = true;
                //    }
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to copy " + track.Title);
                Console.WriteLine(e);
            }

            return success;
        }
    }
}