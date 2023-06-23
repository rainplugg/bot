using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Course.Classes
{
   public class Connect
   {
      public static SQLiteDataReader Query(string str)
      {
         SQLiteConnection SQLiteConnection = new SQLiteConnection("Data Source=|DataDirectory|course.db");
         SQLiteCommand SQLiteCommand = new SQLiteCommand(str, SQLiteConnection);
         try {
            SQLiteConnection.Open();
            SQLiteDataReader reader = SQLiteCommand.ExecuteReader();
            return reader;
         } catch { 
            return null; 
         }
      }

      public static void LoadUser(List<User> data)
      {
         try {
            data.Clear();
            SQLiteDataReader query = Query("select * from `User`;");
            if (query != null) {
               while (query.Read()) {
                  data.Add(new User(
                     Convert.ToInt64(query.GetValue(0)),
                     query.GetValue(1).ToString(),
                     query.GetValue(2).ToString(),
                     Convert.ToInt32(query.GetValue(3)),
                     Convert.ToInt32(query.GetValue(4)),
                     Convert.ToInt64(query.GetValue(5)),
                     Convert.ToInt32(query.GetValue(6))
                  ));
               }
            }
         } catch { }
      }

      public static void LoadCatalog(List<Catalog> data)
      {
         try {
            data.Clear();
            SQLiteDataReader query = Query("select * from `Catalog`;");
            if (query != null) {
               while (query.Read()) {
                  data.Add(new Catalog(
                     Convert.ToInt32(query.GetValue(0)),
                     query.GetValue(1).ToString(),
                     query.GetValue(2).ToString(),
                     query.GetValue(3).ToString(),
                     query.GetValue(4).ToString(),
                     query.GetValue(5).ToString()
                  ));
               }
            }
         } catch { }
      }
   }
}
