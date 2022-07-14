using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Bishop.Helper.Grive;

public class GriveCredentialsService
{
    private static readonly string[] Scopes = { DriveService.Scope.DriveReadonly };
    private const string CredPath = "token.json";
    private const string CredentialsFile = "Resources/credentials.json";
    private const string AppName = "bishop";

    public UserCredential Credentials { get; private set; }
    public DriveService Drive { get; private set; }

    public GriveCredentialsService()
    {
        using var stream = new FileStream(CredentialsFile, FileMode.Open, FileAccess.Read);
        Credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            Scopes,
            AppName,
            CancellationToken.None,
            new FileDataStore(CredPath, true)).Result; // TODO search about this

        Drive = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = Credentials,
            ApplicationName = AppName
        });
    }
    
}