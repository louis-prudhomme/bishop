using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Bishop.Grive
{
    public class GriveWrapper
    {
        private const string APP_NAME = "Bishop";
        
        private readonly GriveCredentialManager _griveCredentialManager;
        private readonly DriveService service;
        public string PigturesFolderId { get; }

        private GriveWrapper(GriveCredentialManager griveCredentialManager)
        {
            _griveCredentialManager = griveCredentialManager;

            // Create Drive API service.
            service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = griveCredentialManager.Credential,
                ApplicationName = APP_NAME,
            });

            PigturesFolderId = FetchPigturesFolderId();
        }

        private string FetchPigturesFolderId()
        {
            var listRequest = service.Files.List();
            listRequest.PageSize = 1;
            listRequest.Fields = "nextPageToken, files(id)";
            listRequest.Q = "name contains 'Pigtures'";
            return listRequest.Execute().Files.First().Id;
        }

        public static GriveWrapper Instance { get; private set; }
        public static void Init(GriveCredentialManager credentialManager)
        {
            Instance = new GriveWrapper(credentialManager);
        }
    }
}