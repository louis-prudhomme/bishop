using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;
using File = Google.Apis.Drive.v3.Data.File;

namespace Bishop.Grive
{
    public class PigturesGriveManager
    {
        private const string PigturesFolderName = "Pigtures";
        private readonly string pigturesFolderId;

        public PigturesGriveManager()
        {
            pigturesFolderId = GriveWrapper.Instance.FetchFolderIdAsync(PigturesFolderName).Result;
        }

        public async Task<FileList> ListFiles()
        {
            return await GriveWrapper.Instance.FetchFilesInFolderAsync(pigturesFolderId);
        }

        public async Task<File> FetchPig(int id)
        {
            return await GriveWrapper.Instance.FetchFileInFolderAsync($"Pigture ({id})", pigturesFolderId);
        }
    }
}