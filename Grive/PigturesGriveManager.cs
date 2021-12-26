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
        private readonly string _pigturesFolderId;

        public PigturesGriveManager()
        {
            _pigturesFolderId = GriveWrapper.Instance.FetchFolderIdAsync(PigturesFolderName).Result;
        }

        public async Task<FileList> ListFiles()
        {
            return await GriveWrapper.Instance.FetchFilesInFolderAsync(_pigturesFolderId);
        }

        public async Task<File> FetchPig(int id)
        {
            return await GriveWrapper.Instance.FetchFileInFolderAsync($"Pigture ({id})", _pigturesFolderId);
        }
    }
}