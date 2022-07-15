using System;
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
    private static readonly string GoogleCredentials = Environment
        .GetEnvironmentVariable("GOOGLE_CREDS")!;
    private const string CredPath = "token.json";
    private const string AppName = "bishop";

    private UserCredential Credentials { get; }
    public DriveService Drive { get; }

    public GriveCredentialsService()
    {
        using var stream = GoogleCredentials.ToStream();
        Credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            Scopes,
            AppName,
            CancellationToken.None,
            new NullDataStore()).Result; // TODO search about this

        Drive = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = Credentials,
            ApplicationName = AppName
        });
    }
    
}