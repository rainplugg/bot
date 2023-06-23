using Telegram.Bot.Requests;

namespace Course.Classes
{
   public class User
   {
      public long Id { get; set; }
      public string Message { get; set; }
      public string Sub { get; set; }
      public int Id_list { get; set; }
      public int Balance { get; set; }
      public long Id_invited { get; set; }
      public int Sale { get; set; }
      public User(long id, string message, string sub, int id_list, int balance, long id_invited, int sale)
      {
         Id = id;
         Message = message;
         Sub = sub;
         Id_list = id_list;
         Balance = balance;
         Id_invited = id_invited;
         Sale = sale;
      }
   }
}
