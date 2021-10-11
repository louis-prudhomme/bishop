using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Bishop.Grive
{
    public class GriveWrapper
    {
        private readonly GriveCredentialManager _griveCredentialManager;
        private const string APP_NAME = "Bishop";

        public GriveWrapper(GriveCredentialManager griveCredentialManager)
        {
            _griveCredentialManager = griveCredentialManager;

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = griveCredentialManager.Credential,
                ApplicationName = APP_NAME,
            });

            // Define parameters of request.
            var listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            var files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }

            Console.Read();
        }
    }
}