using System.Net.Http.Headers;

namespace PPE.Data.Services;

public class SharePointService
{
    private readonly string _accessToken;
    private readonly string _siteUrl;
    private readonly string _folderPath;

    public SharePointService(string accessToken, string siteUrl, string folderPath)
    {
        _accessToken = accessToken;
        _siteUrl = siteUrl;
        _folderPath = folderPath;
    }

    public async Task UploadFileAsync(Stream fileStream, string fileName)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

        var uploadUrl = $"{_siteUrl}/_api/web/GetFolderByServerRelativeUrl('{_folderPath}')/Files/add(url='{fileName}', overwrite=true)";

        using var content = new StreamContent(fileStream);
        // Read the stream into a byte array
        var fileBytes = await content.ReadAsByteArrayAsync();

        using var byteContent = new ByteArrayContent(fileBytes);
        // Set content type and make the request
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        var response = await httpClient.PostAsync(uploadUrl, byteContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error uploading file: {response.ReasonPhrase}");
        }
        
    }
}