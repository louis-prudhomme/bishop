using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;
using Google.Apis.Drive.v3;
using File = Google.Apis.Drive.v3.Data.File;

namespace Bishop.Config;

public record SimpleFile(string Id);

public class Grive
{
    public DriveService Service { private get; set; } = null!;

    public async Task<IList<SimpleFile>> FetchAllFiles(string parentId, params string[] mimeTypes)
    {
        var listRequest = Service.Files.List();
        listRequest.Fields = "nextPageToken, files(id)";
        listRequest.PageSize = 1000;
        
        listRequest.Q = $"parents in '{parentId}'{mimeTypes.Select(mimeType => $" and mimeType='{mimeType}'").Join()}";

        var result = await listRequest.ExecuteAsync();
        var files = result.Files.Select(file => file.Id).ToList();

        while (!string.IsNullOrEmpty(result.NextPageToken))
        {
            listRequest.PageToken = result.NextPageToken;
            result = await listRequest.ExecuteAsync();
            files.AddRange(result.Files.Select(file => file.Id));
        }

        return files.Select(id => new SimpleFile(id)).ToList();
    }

    public async Task<File?> FetchCompleteFile(SimpleFile file)
    {
        var listRequest = Service.Files.List();
        listRequest.Fields = "nextPageToken, files(id)";
        listRequest.PageSize = 1;
        
        listRequest.Q = $"id in '{file.Id}'";

        var result = await listRequest.ExecuteAsync();
        return result.Files.FirstOrDefault();
    }
}