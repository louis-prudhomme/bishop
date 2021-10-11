using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v3.Data.File;

namespace Bishop.Grive
{
    public class GriveWrapper
    {
        private const string APP_NAME = "Bishop";
        
        private readonly GriveCredentialManager _griveCredentialManager;
        private readonly DriveService service;

        private GriveWrapper(GriveCredentialManager griveCredentialManager)
        {
            _griveCredentialManager = griveCredentialManager;

            // Create Drive API service.
            service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = griveCredentialManager.Credential,
                ApplicationName = APP_NAME,
            });
        }

        public async Task<string> FetchFolderIdAsync(string folderName)
        {
            var listRequest = GetListRequestWithQ($"name contains '{folderName}' and mimeType = 'application/vnd.google-apps.folder'");
            var results = await listRequest.ExecuteAsync();
            return results.Files.First().Id;
        }

        public async Task<FileList> FetchFilesInFolderAsync(string folderId)
        {
            var listRequest = GetListRequestWithQ($"parents in '{folderId}'");
            return await listRequest.ExecuteAsync();
        }

        public async Task<File> FetchFileInFolderAsync(string fileName, string folderId)
        {
            var listRequest = GetListRequestWithQ($"parents in '{folderId}' and name contains '{fileName}'");
            var results = await listRequest.ExecuteAsync();
            return results.Files.First();
        }

        private FilesResource.ListRequest GetListRequest()
        {
            var listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name, webContentLink, webViewLink)";
            return listRequest;
        }

        private FilesResource.ListRequest GetListRequestWithQ(string q)
        {
            var listRequest = GetListRequest();
            listRequest.Q = q;
            return listRequest;
        }

        public static GriveWrapper Instance { get; private set; }
        public static void Init(GriveCredentialManager credentialManager)
        {
            Instance = new GriveWrapper(credentialManager);
        }
    }
}