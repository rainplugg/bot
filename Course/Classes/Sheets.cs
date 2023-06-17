using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System.IO;
using System.Linq;

namespace Course.Classes
{
   public class Sheets
   {
      public static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
      public static string sheetId = "10i-CJNZy4yGjQz7SJgnM7tu2Zm_X48eVybfkt8IrWOg";
      public const string GoogleCredentialsFileName = "google-credentials.json";
      public static SpreadsheetsResource.ValuesResource serviceValues;

      public static SheetsService GetSheetsService()
      {
         try {
            using (var stream = new FileStream(GoogleCredentialsFileName, FileMode.Open, FileAccess.Read)) {
               var serviceInitializer = new BaseClientService.Initializer {
                  HttpClientInitializer = GoogleCredential.FromStream(stream).CreateScoped(Scopes)
               };
               return new SheetsService(serviceInitializer);
            }
         } catch { return null; }
      }

      public static async void GetDataBase()
      {
         try {
            serviceValues = GetSheetsService().Spreadsheets.Values;
            var response = await serviceValues.Get(sheetId, "Catalog!A:E").ExecuteAsync();
            var values = response.Values;
            if (values == null || !values.Any())
               return;
            values.RemoveAt(0);
            string request = string.Empty;
            Connect.Query("delete from `Catalog` where id >= 0;");
            foreach (var value in values) {
               try {
                  Connect.Query("insert into `Catalog` (category, name, description, seller, source) values ('" + value[0] + "', " +
                                                                                                            "'" + value[1].ToString().Replace("\n", " ").Replace("\t", string.Empty).Replace("`", string.Empty).Replace("'", string.Empty) + "', " +
                                                                                                            "'" + value[2].ToString().Replace("\n", " ").Replace("\t", string.Empty).Replace("`", string.Empty).Replace("'", string.Empty) + "', " +
                                                                                                            "'" + value[3].ToString().Replace("\n", string.Empty).Replace("\t", string.Empty).Replace(" ", string.Empty).Replace("'", "%27") + "', " +
                                                                                                            "'" + value[4].ToString().Replace("\n", string.Empty).Replace("\t", string.Empty).Replace(" ", string.Empty).Replace("'", "%27") + "');\n"); ;
               } catch { }
            }
         } catch { }
      }
   }
}
