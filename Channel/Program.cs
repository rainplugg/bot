using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using TL;

namespace Chanel
{
   internal class Program
   {
      private static string token { get; set; } = "5764441358:AAGeAkDB4DKv8g5qIGyDVh0sUPoYE7FzRvE";
      //private static string token { get; set; } = "6123367281:AAFrPTXtsRggDpUdP4j-7rzR40YL0FfUGEU";
      private static TelegramBotClient client;
      static async Task Main(string[] args)
      {
         client = new TelegramBotClient(token);
         client.StartReceiving();
         client.OnMessage += ClientMessage;
         await AuthAsync();
         Console.ReadLine();
      }
      public static long channelId = -1001695523633;
      public static List<User> users = new List<User>();
      public static List<Chanel> chanels = new List<Chanel>();
      public static List<Post> posts = new List<Post>();

      private static async void ClientMessage(object sender, MessageEventArgs e)
      {
         try {
            var message = e.Message;
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
               try {
                  if (message.Text.Contains(" ") && message.Text.Split(' ').Length == 3) {
                     Connect.LoadChannel(chanels);
                     string[] numbers = message.Text.Split(' ');
                     string request = string.Empty;
                     var ch = allches.Find(x => x.localId.ToString() == numbers[1]);
                     if (chanels.Find(x => x.id.ToString() == ch.id && x.topic == numbers[2]) == null) {
                        if (ch != null)
                           request += "insert into `Chanel` (id, topic, title) values ('" + ch.id + "', '" + numbers[2] + "', '" + ch.title + "');\n";
                        Connect.Query(request);
                        await client.SendTextMessageAsync(message.Chat.Id, "✅ Каналы добалены");
                     }
                     else
                        await client.SendTextMessageAsync(message.Chat.Id, "⛔️ Данная связка каналов уже существует");
                  }
                  else
                     await client.SendTextMessageAsync(message.Chat.Id, "⛔️ Неверный запрос (Пример: /addchannel [id канала от] [id темы в])");
               } catch {
                  await client.SendTextMessageAsync(message.Chat.Id, "⛔️ Неверный запрос (Пример: /addchannel [id канала от] [id темы в])");
               }
            }
            else if (message.Text.Contains("/delchannel")) {
               Connect.LoadChannel(chanels);
               if (message.Text.Contains(" ")) {
                  string[] numbers = message.Text.Split(' ');
                  string request = string.Empty;
                  for (int i = 1; i < numbers.Length; i++) {
                     try {
                        if (chanels[Convert.ToInt32(numbers[i]) - 1] != null)
                           request += "delete from `Chanel` where id = " + chanels[Convert.ToInt32(numbers[i]) - 1].id + " and topic = '" + chanels[Convert.ToInt32(numbers[i]) - 1].topic + "';";
                     } catch { }
                  }
                  Connect.Query(request);
                  await client.SendTextMessageAsync(message.Chat.Id, "✅ Каналы удалены");
               }
            }
            else if (message.Text == "/getdb") {
               string path = Path.GetFullPath("chanel.db");
               File.Copy(path, Path.GetFullPath("ch.db"), true);
               using (var stream = File.OpenRead(Path.GetFullPath("ch.db"))) {
                  InputOnlineFile file = new InputOnlineFile(stream);
                  await client.SendDocumentAsync(885185553, file);
               }
               File.Delete(Path.GetFullPath("ch.db"));
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
                     allches.Add(new Allch(number, chat.ID.ToString(), null, chat.Title));
                     response += number + ". " + chat.Title + "\n";
                     number++;
                  }
               } catch { }
            }
            number = 1;
            if (chanels.Count > 0) {
               response += "\nСписок каналов находящихся в парсинге:\n";
               for (int i = 0; i < chanels.Count; i++) {
                  response += number + ". " + chanels[i].title + " (" + chanels[i].topic + ")\n";
                  number++;
               }
            }
            if (write == true)
               await client.SendTextMessageAsync(message.Chat.Id, response);
         } catch { }
      }

      static Chanel actualChannel = null;
      public static async Task Client_OnUpdate(IObject arg)
      {
         try {
            Connect.LoadChannel(chanels);
            var sfsd = arg.GetType();
            if (arg.GetType().Name == "Updates") {
               Updates upd = (Updates)arg;
               foreach (var update in upd.UpdateList) {
                  try {
                     if (update.GetType().Name == "UpdateNewMessage") {
                        UpdateNewMessage newMessage = (UpdateNewMessage)update;
                        actualChannel = null;
                        actualChannel = chanels.Find(x => x.id == newMessage.message.Peer.ID);
                        Message message = (Message)newMessage.message;
                        if (actualChannel != null)
                           await SendUpdate(message);
                     }
                     else if (update.GetType().Name == "UpdateNewChannelMessage") {
                        UpdateNewChannelMessage newMessage = (UpdateNewChannelMessage)update;
                        actualChannel = null;
                        actualChannel = chanels.Find(x => x.id == newMessage.message.Peer.ID);
                        Message message = (Message)newMessage.message;
                        if (actualChannel != null)
                           await SendUpdate(message);
                     }
                  } catch { }
               }
            }
         } catch { }
      }

      public static async Task SendUpdate(Message message)
      {
         try {
            List<Post> findId = new List<Post>();
            Connect.LoadPost(posts);
            var chan = chanels.Find(x => x.id == message.Peer.ID);
            if (message.reply_to != null)
               if (chan != null && message.reply_to.reply_to_msg_id != 0)
                  findId = posts.FindAll(x => x.id.Split('|')[0] == message.Peer.ID.ToString() && x.reply.Split('|')[0] == message.reply_to.reply_to_msg_id.ToString());
            string encodeMessage = System.Web.HttpUtility.UrlEncode(message.message);
            if (message.media == null) {
               if (findId != null && findId.Count > 0)
                  await HttpRequest($"https://api.telegram.org/bot{token}/sendMessage?chat_id={channelId}&message_thread_id={chan.topic}&text={encodeMessage}&disable_web_page_preview=False&reply_to_message_id=" + findId[findId.Count - 1].reply.Split('|')[1], chan, message, null, null, null);
               else
                  await HttpRequest($"https://api.telegram.org/bot{token}/sendMessage?chat_id={channelId}&message_thread_id={chan.topic}&text={encodeMessage}&disable_web_page_preview=False", chan, message, null, null, null);
            }
            else {
               string filename = string.Empty, caption = string.Empty;
               try {
                  if (message.message != null)
                     caption = message.message;
                  MessageMediaPhoto media = (MessageMediaPhoto)message.media;
                  if (media.photo != null) {
                     Photo photo = (Photo)media.photo;
                     filename = $"{photo.id}.jpg";
                     using var fileStream = File.Create(filename);
                     var type = await wclient.DownloadFileAsync(photo, fileStream);
                     fileStream.Close();
                  }
                  if (File.Exists(Path.GetFullPath(filename))) {
                     byte[] bData = File.ReadAllBytes(Path.GetFullPath(filename));
                     await HttpRequest("https://api.telegram.org/bot" + token + "/sendPhoto?chat_id=" + channelId + "&message_thread_id=" + chan.topic + "&caption=" + encodeMessage, chan, message, bData, filename, "photo");
                  }
               } catch {
                  try {
                     if (message.message != null)
                        caption = message.message;
                     MessageMediaDocument document = (MessageMediaDocument)message.media;
                     if (document.document != null) {
                        Document doc = (Document)document.document;
                        filename = doc.Filename;
                        filename ??= $"{doc.id}.{doc.mime_type[(doc.mime_type.IndexOf('/') + 1)..]}";
                        using var fileStream = File.Create(filename);
                        await wclient.DownloadFileAsync(doc, fileStream);
                        fileStream.Close();
                     }
                     if (File.Exists(Path.GetFullPath(filename))) {
                        Document doc = (Document)document.document;
                        byte[] bData = File.ReadAllBytes(Path.GetFullPath(filename));
                        using (var ms = new MemoryStream(bData)) {
                           string url = string.Empty;
                           if (doc.mime_type.Contains("ogg")) {
                              if (findId != null && findId.Count > 0)
                                 await HttpRequest("https://api.telegram.org/bot" + token + "/sendVoice?chat_id=" + channelId + "&message_thread_id=" + chan.topic + "&caption=" + encodeMessage + "&disable_web_page_preview=False&reply_to_message_id=" + findId[findId.Count - 1].reply.Split('|')[1], chan, message, bData, filename, "voice");
                              else
                                 await HttpRequest("https://api.telegram.org/bot" + token + "/sendVoice?chat_id=" + channelId + "&message_thread_id=" + chan.topic + "&caption=" + encodeMessage, chan, message, bData, filename, "voice");
                           }
                           else if (doc.mime_type.Contains("mpeg") || doc.mime_type.Contains("mp3")) {
                              if (findId != null && findId.Count > 0)
                                 await HttpRequest("https://api.telegram.org/bot" + token + "/sendAudio?chat_id=" + channelId + "&message_thread_id=" + chan.topic + "&caption=" + encodeMessage + "&disable_web_page_preview=False&reply_to_message_id=" + findId[findId.Count - 1].reply.Split('|')[1], chan, message, bData, filename, "audio");
                              else
                                 await HttpRequest("https://api.telegram.org/bot" + token + "/sendAudio?chat_id=" + channelId + "&message_thread_id=" + chan.topic + "&caption=" + encodeMessage, chan, message, bData, filename, "audio");
                           }
                           else {
                              if (findId != null && findId.Count > 0)
                                 await HttpRequest("https://api.telegram.org/bot" + token + "/sendVideo?chat_id=" + channelId + "&message_thread_id=" + chan.topic + "&caption=" + encodeMessage + "&disable_web_page_preview=False&reply_to_message_id=" + findId[findId.Count - 1].reply.Split('|')[1], chan, message, bData, filename, "video");
                              else
                                 await HttpRequest("https://api.telegram.org/bot" + token + "/sendVideo?chat_id=" + channelId + "&message_thread_id=" + chan.topic + "&caption=" + encodeMessage, chan, message, bData, filename, "video");
                           }
                        }
                     }
                  } catch { }
               }
            }
         } catch { }
      }

      public static WTelegram.Client wclient;
      public static async Task AuthAsync()
      {
         try {
            wclient = new WTelegram.Client(15154180, "01525307f48c1d6e59a7e796963b18d6");
            await wclient.ConnectAsync();
            await wclient.Login("+79996668870");
            //wclient = new WTelegram.Client(22250500, "2d44181c5ee1edd3db00202dd2991e3e");
            //await wclient.ConnectAsync();
            //await wclient.Login("+79519552442");
            if (wclient.User == null) {
               Console.WriteLine("Введите код из SMS: ");
               string code = Console.ReadLine();
               Console.WriteLine(await wclient.Login(code));
               Console.WriteLine(wclient.User);
            }
            wclient.OnUpdate += Client_OnUpdate;
            new Thread(() => {
               Parse();
            }).Start();
         } catch { }
      }

      public static async Task HttpRequest(string url, Chanel chan, Message message, byte[] bData, string filename, string type)
      {
         try {
            await Task.Delay(5000);
            using (var http = new HttpClient()) {
               if (bData != null) {
                  using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture))) {
                     content.Add(new StreamContent(new MemoryStream(bData)), type, filename);
                     using (var res = await http.PostAsync(url, content)) {
                        MessageId messageId = JsonConvert.DeserializeObject<MessageId>(await res.Content.ReadAsStringAsync());
                        Connect.Query("insert into `Post` (id, reply) values ('" + chan.id + "', '" + Convert.ToInt32(message.id) + "|" + messageId.result.message_id + "');");
                        File.Delete(Path.GetFullPath(filename));
                     }
                  }
               }
               else {
                  using (var res = await http.PostAsync(url, null)) {
                     MessageId messageId = JsonConvert.DeserializeObject<MessageId>(await res.Content.ReadAsStringAsync());
                     Connect.Query("insert into `Post` (id, reply) values ('" + chan.id + "', '" + Convert.ToInt32(message.id) + "|" + messageId.result.message_id + "');");
                  }
               }
               http.Dispose();
            }
         } catch { }
      }

      public async static void Parse()
      {
         try {
            while (true) {
               await GetChanels(null, false, true);
               await Task.Delay(70000);
            }
         } catch {
            new Thread(() => {
               Parse();
            }).Start();
         }
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

      public class Result
      {
         public int message_id { get; set; }
      }

      public class MessageId
      {
         public Result result { get; set; }
      }
   }
}
