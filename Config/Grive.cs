using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;
using Google.Apis.Drive.v3;
using File = Google.Apis.Drive.v3.Data.File;

namespace Bishop.Config;

public record SimpleFile(string Id, string Name);

public class Grive
{
    public DriveService Service { private get; set; } = null!;

    public async Task<IList<SimpleFile>> FetchAllFiles(string parentId, params string[] mimeTypes)
    {
        var listRequest = Service.Files.List();
        listRequest.Fields = "nextPageToken, files(id, name)";
        listRequest.PageSize = 1000;
        
        listRequest.Q = $"parents in '{parentId}'{mimeTypes.Select(mimeType => $" and mimeType='{mimeType}'").Join()}";

        var result = await listRequest.ExecuteAsync();
        var files = result.Files.Select(file => new SimpleFile(file.Id, file.Name)).ToList();

        while (!string.IsNullOrEmpty(result.NextPageToken))
        {
            listRequest.PageToken = result.NextPageToken;
            result = await listRequest.ExecuteAsync();
            files.AddRange(result.Files.Select(file => new SimpleFile(file.Id, file.Name)));
        }

        return files;
    }

    public async Task<File?> FetchCompleteFile(SimpleFile file)
    {
        var listRequest = Service.Files.Get(file.Id);
        listRequest.Fields = "id, name, resourceKey, thumbnailLink";

        return await listRequest.ExecuteAsync();
    }
}