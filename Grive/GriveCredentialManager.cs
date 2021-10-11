using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;

namespace Bishop.Grive
{
    public class CredentialManager
    {
        private static string[] SCOPES = { DriveService.Scope.DriveReadonly };
        private const string TOKEN_FILE_NAME = "token.json";
        private const string USER_NAME = "bishop";
        
        public UserCredential Credential { get; }

        public CredentialManager(string toCredentials)
        {
            using var stream = new FileStream(toCredentials, FileMode.Open, FileAccess.Read);
            
            Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                SCOPES,
                USER_NAME,
                CancellationToken.None,
                new FileDataStore(TOKEN_FILE_NAME, true)).Result;
            Console.WriteLine("Credential file saved to: " + TOKEN_FILE_NAME);
        }
        
        
    }
}