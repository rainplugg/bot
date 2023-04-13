using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using TL;

namespace Search
{
   internal class Program
   {
      private static string token { get; set; } = "6069866927:AAFT-AYp4W-y331Kx1KFwH0eVDdIReEyXYM";
      private static TelegramBotClient client;
      static async Task Main(string[] args)
      {
         client = new TelegramBotClient(token);
         await AuthAsync();
         client.StartReceiving();
         client.OnMessage += ClientMessage;
         Console.ReadLine();
      }

      public static List<User> users = new List<User>();
      public static List<Chanel> chanels = new List<Chanel>();
      public static List<Post> posts = new List<Post>();
      public static long channelId = -1001845729440;

      private static async void ClientMessage(object sender, MessageEventArgs e)
      {
         try {
            var message = e.Message;
            if (message.Chat.Id == 560029256 || message.Chat.Id == 885185553) {
               if (message.Text == "/start") {
                  Connect.LoadUser(users);
                  var user = users.Find(x => x.id == message.Chat.Id.ToString());
                  if (user == null)
                     Connect.Query("insert into `User` (id, message) values ('" + message.Chat.Id + "', 'none');");
               }
               else if (message.Text == "/channels") {
                  await GetChanels(message, true, false);
               }
               else if (message.Text.Contains("/addchannel")) {
                  if (message.Text.Contains(" ") && message.Text.Contains("-")) {
                     string[] numbers = message.Text.Split(' ');
                     string number = numbers.Last();
                     if (number.Contains("-")) {
                        number = number.Replace("-", " ");
                        number = number.Trim(' ');
                     }
                     else {
                        await client.SendTextMessageAsync(message.Chat.Id, "⛔️ Неверный запрос (Пример: /addchannel 32 12 3 -кирпич-лодка-кружка-альбом)");
                        return;
                     }
                     string request = string.Empty;
                     for (int i = 1; i < numbers.Length - 1; i++) {
                        var ch = allches.Find(x => x.localId.ToString() == numbers[i]);
                        if (ch != null)
                           request += "insert into `Chanel` (id, topic, title) values ('" + ch.id + "', '" + number + "', '" + ch.title + "');\n";

                     }
                     Connect.Query(request);
                     await client.SendTextMessageAsync(message.Chat.Id, "✅ Каналы добалены");
                  }
                  else
                     await client.SendTextMessageAsync(message.Chat.Id, "⛔️ Неверный запрос (Пример: /addchannel 32 12 3 -кирпич-лодка-кружка-альбом)");
               }
               else if (message.Text.Contains("/delchannel")) {
                  Connect.LoadChannel(chanels);
                  if (message.Text.Contains(" ")) {
                     string[] numbers = message.Text.Split(' ');
                     string request = string.Empty;
                     for (int i = 1; i < numbers.Length; i++) {
                        try {
                           if (chanels[Convert.ToInt32(numbers[i]) - 1] != null)
                              request += "delete from `Chanel` where id = " + chanels[Convert.ToInt32(numbers[i]) - 1].id + ";";
                        } catch { }
                     }
                     Connect.Query(request);
                     await client.SendTextMessageAsync(message.Chat.Id, "✅ Каналы удалены");
                  }
               }
            }
         } catch { }
      }

      public static Messages_Chats chats = new Messages_Chats();
      public static async Task GetChanels(Telegram.Bot.Types.Message message, bool write, bool load)
      {
         try {
            Connect.LoadChannel(chanels);

            if (load == true)
               chats = await wclient.Messages_GetAllChats();
            int number = 1;
            allches.Clear();
            string response = "Список доступных каналов для парсинга:\n";
            foreach (var element in chats.chats) {
               try {
                  if (element.Value.ToString().Contains("Channel") || element.Value.ToString().Contains("Chat") || element.Value.ToString().Contains("Group")) {
                     ChatBase chat = element.Value;
                     var find = chanels.Find(x => x.id == chat.ID);
                     if (find == null) {
                        allches.Add(new Allch(number, chat.ID.ToString(), null, chat.Title));
                        response += number + ". " + chat.Title + "\n";
                        number++;
                     }
                  }
               } catch { }
            }
            number = 1;
            string responseNext = string.Empty, responseEnd = "\nДля добавления/удаления канала в/из парсинг(-а) введите /addchannel [номер] -[ключевое слово(-а)] или /delchannel [номер] -[ключевое слово] (номера можно вводить через пробел, например /addchannel 12 32 43 10 -кирпич-лодка-кружка-альбом)";
            if (chanels.Count > 0) {
               responseNext += "\nСписок каналов находящихся в парсинге:\n";
               for (int i = 0; i < chanels.Count; i++) {
                  responseNext += number + ". " + chanels[i].title + " (" + chanels[i].topic + ")\n";
                  number++;
               }
            }
            if (write == true) {
               if (response.Length + responseNext.Length <= 4096) {
                  response += responseNext;
                  if (response.Length + responseEnd.Length <= 4096)
                     await client.SendTextMessageAsync(message.Chat.Id, response + responseEnd);
                  else {
                     await client.SendTextMessageAsync(message.Chat.Id, response);
                     await client.SendTextMessageAsync(message.Chat.Id, responseEnd);
                  }
               }
               else {
                  await client.SendTextMessageAsync(message.Chat.Id, response);
                  if (responseEnd.Length + responseNext.Length <= 4096)
                     await client.SendTextMessageAsync(message.Chat.Id, responseNext + responseEnd);
                  else {
                     if (responseNext != string.Empty)
                        await client.SendTextMessageAsync(message.Chat.Id, responseNext);
                     await client.SendTextMessageAsync(message.Chat.Id, responseEnd);
                  }
               }
            }
         } catch { }
      }

      public static async Task GetMessage()
      {
         try {
            Connect.LoadPost(posts);
            Connect.LoadChannel(chanels);
            var chats = await wclient.Messages_GetAllChats();
            for (int i = 0; i < chanels.Count; i++) {
               bool isGroup = false;
               InputPeer peer = chats.chats[chanels[i].id];
               foreach (var element in chats.chats) {
                  try {
                     if (element.Value.ToString().Contains("Group") && element.Value.ID == peer.ID) {
                        isGroup = true;
                        break;
                     }
                  } catch { }
               }
               if (isGroup == true) {
                  var messages = await wclient.Messages_GetHistory(peer, 0, limit: 50);
                  foreach (var msgBase in messages.Messages) {
                     try {
                        Message message = (Message)msgBase;
                        if (posts.Find(x => x.id.Split('|')[0] == chanels[i].id.ToString() && x.id.Split('|')[1] == message.id.ToString()) == null) {
                           bool write = false;
                           if (chanels[i].topic.Contains(" ")) {
                              for (int k = 0; k < chanels[i].topic.Split(' ').Length; k++) {
                                 if (message.message.ToLower().Contains(chanels[i].topic.Split(' ')[k].ToLower())) {
                                    write = true;
                                    break;
                                 }
                              }
                           }
                           else if (message.message.ToLower().Contains(chanels[i].topic.ToLower()))
                              write = true;
                           if (write == true) {
                              var user = messages.UserOrChat(message.From);
                              if (user.MainUsername != null) {
                                 await client.SendTextMessageAsync(channelId, "Канал: " + chanels[i].title + "\nПользователь: @" + user.MainUsername + "\nСообщение: " + message.message);
                                 Connect.Query("insert into `Post` (id) values ('" + chanels[i].id + "|" + message.id + "');");
                                 await Task.Delay(2500);
                              }
                              else {
                                 await client.SendTextMessageAsync(channelId, "Канал: " + chanels[i].title + "\nПользователь: https://web.telegram.org/z/#" + message.from_id.ID + "\nСообщение: " + message.message, disableWebPagePreview: true);
                                 Connect.Query("insert into `Post` (id) values ('" + chanels[i].id + "|" + message.id + "');");
                                 await Task.Delay(2500);
                              }
                           }
                        }
                     } catch { }
                  }
               }
               else {
                  var messages = await wclient.Messages_GetHistory(peer, 0, limit: 5);
                  foreach (var msgBase in messages.Messages) {
                     try {
                        Message message = (Message)msgBase;
                        if (message.replies != null) {
                           var replies = await wclient.Messages_GetReplies(peer, message.id, limit: 50);
                           for (int j = 0; j < replies.Messages.Count(); j++) {
                              try {
                                 Message tempMessage = (Message)replies.Messages[j];
                                 if (posts.Find(x => x.id.Split('|')[0] == chanels[i].id.ToString() && x.id.Split('|')[1] == tempMessage.id.ToString()) == null) {
                                    bool write = false;
                                    if (chanels[i].topic.Contains(" ")) {
                                       for (int k = 0; k < chanels[i].topic.Split(' ').Length; k++) {
                                          if (tempMessage.message.ToLower().Contains(chanels[i].topic.Split(' ')[k].ToLower())) {
                                             write = true;
                                             break;
                                          }
                                       }
                                    }
                                    else if (tempMessage.message.ToLower().Contains(chanels[i].topic.ToLower()))
                                       write = true;
                                    if (write == true) {
                                       var user = replies.UserOrChat(tempMessage.From);
                                       if (user.MainUsername != null) {
                                          await client.SendTextMessageAsync(channelId, "Канал: " + chanels[i].title + "\nПользователь: @" + user.MainUsername + "\nСообщение: " + tempMessage.message);
                                          Connect.Query("insert into `Post` (id) values ('" + chanels[i].id + "|" + tempMessage.id + "');");
                                          await Task.Delay(2500);
                                       }
                                       else {
                                          await client.SendTextMessageAsync(channelId, "Канал: " + chanels[i].title + "\nПользователь: https://web.telegram.org/z/#" + message.from_id.ID + "\nСообщение: " + message.message, disableWebPagePreview: true);
                                          Connect.Query("insert into `Post` (id) values ('" + chanels[i].id + "|" + tempMessage.id + "');");
                                          await Task.Delay(2500);
                                       }
                                    }
                                 }
                              } catch { }
                           }
                        }
                     } catch { }
                  }
               }
            }
         } catch { }
      }

      public static WTelegram.Client wclient;
      public static async Task AuthAsync()
      {
         try {
            //wclient = new WTelegram.Client(22250500, "2d44181c5ee1edd3db00202dd2991e3e");
            //await wclient.ConnectAsync();
            //await wclient.Login("+79519552442");

            wclient = new WTelegram.Client(26393043, "242c6983f21390982b5780e4b8813394");
            await wclient.ConnectAsync();
            await wclient.Login("+79936960969");
            if (wclient.User == null) {
               Console.WriteLine("Введите код из SMS: ");
               string code = Console.ReadLine();
               Console.WriteLine(await wclient.Login(code));
               Console.WriteLine(wclient.User);
            }
            new Thread(() => {
               Parse();
            }).Start();
         } catch { }
      }

      public static List<Allch> allches = new List<Allch>();
      public class Allch
      {
         public int localId { get; set; }
         public string id { get; set; }
         public string hash { get; set; }
         public string title { get; set; }
         public Allch(int localId, string id, string hash, string title)
         {
            this.localId = localId;
            this.id = id;
            this.hash = hash;
            this.title = title;
         }
      }

      public async static void Parse()
      {
         try {
            while (true) {
               Connect.LoadChannel(chanels);
               await GetChanels(null, false, true);
               if (chanels.Count > 0) {
                  await GetMessage();
                  await Task.Delay(5000);
               }
               else await Task.Delay(52000);
            }
         } catch {
            new Thread(() => {
               Parse();
            }).Start();
         }
      }
   }
}
