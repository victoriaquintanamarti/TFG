using Google.Cloud.Language.V1;
using System;

namespace GoogleCloudSamples
{
    public class QuickStart
    {/*
        public static void Main(string[] args)
         {
             string credential_path = @"C:\Users\Victoria\Downloads\analisisfrases19-96218ce0fa79.json";
             System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
             // The text to analyze.
             string text = "Hello World!";
             var client = LanguageServiceClient.Create();
             var response = client.AnalyzeSentiment(new Document()
             {
                 Content = text,
                 Type = Document.Types.Type.PlainText
             });
             var sentiment = response.DocumentSentiment;
             Console.WriteLine($"Score: {sentiment.Score}");
             Console.WriteLine($"Magnitude: {sentiment.Magnitude}");
         }*/
     }
}