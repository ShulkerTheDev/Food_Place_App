using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SheetsQuickstart
{
    class Program
    {
        static readonly string[] Scopes = {SheetsService.Scope.Spreadsheets};
        static readonly string ApplicationName = "Food_Place_App";
        static readonly string SpreadsheetId = "18jdL_LoJIg73CR9T2x0nSNZqwwYyZceiDEKKp6SjZuE";
        static readonly string sheet = "Food places";
        static SheetsService service;

        static void Main(string[] args)
        {
            
            GoogleCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            }); 
            
            ReadEntries();
        }
          static void ReadEntries()
        {
            var range = $"{sheet}!A1:M49";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;

            if(values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                  Console.WriteLine("{0} {1} | {2} | {3}", row[5], row[4], row[3], row[1]);
                }
              
            }
            else
            {
                Console.WriteLine("No data found");
            }

        }        
    }
}