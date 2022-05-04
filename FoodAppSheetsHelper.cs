using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using System.IO;

public class GoogleSheetsHelper
{
    public SheetsService service { get; set; }
    const string applicationName = "Food_Place_App";
    static readonly string[] scopes = { SheetsService.Scope.Spreadsheets };

    public GoogleSheetsHelper()
    {
        InitializeService();
    }

    private void InitializeService()
    {
        var credential = GetCredentialsFromFile();
        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = applicationName
        });
    }

    private GoogleCredential GetCredentialsFromFile()
    {
        GoogleCredential credential;
        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
        }

        return credential;
    }
}

