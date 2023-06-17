#pragma warning disable CS0618 // Тип или член устарел

using Course.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace Course
{
   internal class Program
   {
      public static List<User> users = new List<User>();
      public static List<Catalog> catalogs = new List<Catalog>();


      readonly static long specialChat = -1001905068451;

      private static string Token { get; set; } = "5879520439:AAHkeDmAPyzMgw2tId5lsushLR6kRB0PTtM";
      private static TelegramBotClient client;
      static void Main()
      {
         Connect.LoadCatalog(catalogs);
         client = new TelegramBotClient(Token);
         client.StartReceiving();
         client.OnMessage += ClientMessage;
         client.OnUpdate += UpdateData;
         client.OnCallbackQuery += (object sc, CallbackQueryEventArgs ev) => {
            InlineButtonOperation(sc, ev);
         };
         Console.ReadLine();
      }

      private static async void ClientMessage(object sender, MessageEventArgs e)
      {
         try {
            var message = e.Message;
            Connect.LoadUser(users);
            var user = users.Find(x => x.Id == message.Chat.Id);
            if (message.Text == "/start") {
               if (user == null)
                  Connect.Query("insert into `User` (id, message, sub, id_list, balance, id_invited) values (" + message.Chat.Id + ", 'none', 'none', 0, 0, 0);");
               if (user != null && user.Sub == "active")
                  await client.SendPhotoAsync(message.Chat.Id, "AgACAgIAAxkBAAIEUWSHJx77F35S9B6HN8i_7gABueVmfwACTMgxG1N3OEjQJw2iZI-9fgEAAwIAA3gAAy8E", "Привет *" + message.From.FirstName + "*, мы рады приветствовать вас в сообщесте \"*Самоучки*\"\n" +
                                                                     "У нас вы сможете найти большое количество обучающих курсов лучших онлайн школ образования - SkillBox. " +
                                                                     "Нетология, Яндекс.Практикум, Udemy, SkillFactory и многие другие схемы заработка.\n" +
                                                                     "Мы ежедневно обновляем базу слитых курсов, добавляем новые темы и свежие сливы складчин в наше сообщество. Материалы для скачивания доступны через торрент и облачные сервисы", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: Keyboard.subMainKey);
               else
                  await client.SendPhotoAsync(message.Chat.Id, "AgACAgIAAxkBAAIEUWSHJx77F35S9B6HN8i_7gABueVmfwACTMgxG1N3OEjQJw2iZI-9fgEAAwIAA3gAAy8E", "Привет *" + message.From.FirstName + "*, мы рады приветствовать вас в сообщесте \"*Самоучки*\"\n" +
                                                                        "У нас вы сможете найти большое количество обучающих курсов лучших онлайн школ образования - SkillBox. " +
                                                                        "Нетология, Яндекс.Практикум, Udemy, SkillFactory и многие другие схемы заработка.\n" +
                                                                        "Мы ежедневно обновляем базу слитых курсов, добавляем новые темы и свежие сливы складчин в наше сообщество. Материалы для скачивания доступны через торрент и облачные сервисы", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: Keyboard.mainKey);
            }
            else if (message.Text.Contains("/start ")) {
               if (user == null)
                  Connect.Query("insert into `User` (id, message, sub, id_list, balance, id_invited) values (" + message.Chat.Id + ", 'none', 'none', 0, 0, " + message.Text.Split(' ')[1] + ");");
               else if (user.Id_invited == 0 && user.Id.ToString() != message.Text.Split(' ')[1])
                  Connect.Query("update `User` set id_invited = " + message.Text.Split(' ')[1] + " where id = " + message.Chat.Id + ";");
               if (user != null && user.Sub == "active")
                  await client.SendPhotoAsync(message.Chat.Id, "AgACAgIAAxkBAAIEUWSHJx77F35S9B6HN8i_7gABueVmfwACTMgxG1N3OEjQJw2iZI-9fgEAAwIAA3gAAy8E", "Привет *" + message.From.FirstName + "*, мы рады приветствовать вас в сообщесте \"*Самоучки*\"\n" +
                                                                     "У нас вы сможете найти большое количество обучающих курсов лучших онлайн школ образования - SkillBox. " +
                                                                     "Нетология, Яндекс.Практикум, Udemy, SkillFactory и многие другие схемы заработка.\n" +
                                                                     "Мы ежедневно обновляем базу слитых курсов, добавляем новые темы и свежие сливы складчин в наше сообщество. Материалы для скачивания доступны через торрент и облачные сервисы", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: Keyboard.subMainKey);
               else
                  await client.SendPhotoAsync(message.Chat.Id, "AgACAgIAAxkBAAIEUWSHJx77F35S9B6HN8i_7gABueVmfwACTMgxG1N3OEjQJw2iZI-9fgEAAwIAA3gAAy8E", "Привет *" + message.From.FirstName + "*, мы рады приветствовать вас в сообщесте \"*Самоучки*\"\n" +
                                                                        "У нас вы сможете найти большое количество обучающих курсов лучших онлайн школ образования - SkillBox. " +
                                                                        "Нетология, Яндекс.Практикум, Udemy, SkillFactory и многие другие схемы заработка.\n" +
                                                                        "Мы ежедневно обновляем базу слитых курсов, добавляем новые темы и свежие сливы складчин в наше сообщество. Материалы для скачивания доступны через торрент и облачные сервисы", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: Keyboard.mainKey);
            }
            else if (message.Text == "/help") {
               await client.SendTextMessageAsync(message.Chat.Id, "Тут вы можете задать вопрос в поддержку, либо найти ответ самостоятельно", replyMarkup: Keyboard.helpKey);
            }
            else if (user.Message != "none") {
               if (user.Message == "waitspecial") {
                  try {
                     await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId - 1, replyMarkup: null);
                  } catch { }
                  Connect.Query("update `User` set message = 'none' where id = '" + message.Chat.Id + "';");
                  await client.SendTextMessageAsync(specialChat, "⛔️ Нет ответа\n\nПользователь: " + message.From.FirstName + "\nСодержимое обращения:\n" + message.Text + "\n\n" + message.Chat.Id);
                  await client.SendTextMessageAsync(message.Chat.Id, "✅ Сообщение отправлено специалистам, ожидайте ответа, он поступит в этот чат");
               }
            }
            else {
               var item = catalogs.Find(x => x.Id == Convert.ToInt32(message.Text));
               if (item != null) {
                  Connect.LoadUser(users);
                  if (user.Sub == "none") {
                     InlineKeyboardMarkup courseKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Написать специалисту", "WriteSpecialChat") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Premium подписка", "PremiumSub") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Назад", "GoToList_" + item.Id) } });
                     await client.SendTextMessageAsync(message.Chat.Id, "Название: " + item.Name + "\nОписание: " + item.Description + "\n\nПродажник: " + item.Seller + "\nСсылка на скачивание: Чтобы получить доступ к ссылке, оформите Premium подписку", replyMarkup: courseKey, disableWebPagePreview: true);
                  }
                  else {
                     InlineKeyboardMarkup courseKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Написать специалисту", "WriteSpecialChat") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Назад", "GoToList_" + item.Id) } });
                     await client.SendTextMessageAsync(message.Chat.Id, "Название: " + item.Name + "\nОписание: " + item.Description + "\n\nПродажник: " + item.Seller + "\nСсылка на скачивание: " + item.Source, disableWebPagePreview: true, replyMarkup: courseKey);
                  }
                  try {
                     await client.DeleteMessageAsync(message.Chat.Id, users.Find(x => x.Id == message.Chat.Id).Id_list);
                  } catch { }
               }
            }
         } catch { }
      }

      private static async void InlineButtonOperation(object sc, CallbackQueryEventArgs ev)
      {
         try {
            var message = ev.CallbackQuery.Message;
            var data = ev.CallbackQuery.Data;
            if (data == "Help") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               Connect.Query("update `User` set message = 'none' where id = '" + message.Chat.Id + "';");
               await client.SendTextMessageAsync(message.Chat.Id, "Тут вы можете задать вопрос в поддержку, либо найти ответ самостоятельно", replyMarkup: Keyboard.helpKey);
            }
            else if (data == "WriteSpecialChat") {
               Connect.Query("update `User` set message = 'waitspecial' where id = '" + message.Chat.Id + "';");
               await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, message.From.FirstName + " напишите свой вопрос.\n\nВам ответит первый освободившийся специалист.", replyMarkup: new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } }));
            }
            else if (data == "Copyholder") {
               await client.SendTextMessageAsync(message.Chat.Id, "Проект \"*Самоучки*\" организован в форме образовательного некоммерческого клуба. \"*Самоучки*\" находится в интернете, не является юридическим лицом и не ведет свою деятельность на территории какой либо страны.\n" +
                                                                  "Главная цель проекта - предоставление пользователям информации для обучения по различным направлениям.\n" +
                                                                  "Проект \"*Самоучки*\" предоставляет пользователям телеграмм бота только информационную и техническую возможность для организации главной цели проекта. Авторские права на продукты принадлежат только их владельцам. Запрещается любое коммерческое использование материалов, в том числе и их распространение.\n\n" +
                                                                  "Администрация проекта сообщает, что вся опубликованная информация в нашем телеграмм боте взята из открытых источников, либо прислана и опубликована непосредственно самими пользователями нашего сайта. Если Вы заметили нарушение своих авторских прав, то можете связаться с нами написва специалисту для удаления Вашего продукта с нашнего сайта.\n\n" +
                                                                  "Обратите внимание:\n\n" +
                                                                  "1. Сертификат, выданный сервисом CopyTrust не является официальным подтверждением Ваших авторских прав.\n" +
                                                                  "2. Не рассматриваются жалобы, которые содержат угрозы или оскорбления в адрес нашего бота или Администрации.\n" +
                                                                  "3. Мы не реагируем на необоснованные жалобы третьих сторон. Жалобы рассматриваются только от Авторов материалов или же от их представителей!\n" +
                                                                  "4. Мы в праве отказать в рассмотрении жалобы из-за неодостаточного количества данных, которые позволяют идентифицировать Вас, как правообладателя.", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: Keyboard.holderKey);
            }
            else if (data == "PremiumSub") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               int price = 999;
               if (users.Find(x => x.Id == message.Chat.Id).Id_invited != 0)
                  price = 999 - (999 / 100 * 15);
               await client.SendPhotoAsync(message.Chat.Id, "AgACAgIAAxkBAAIEUmSHJz4We4TS1FqnBsdJWe2h-yKjAAJNyDEbU3c4SJrQJPluVBrvAQADAgADeAADLwQ", "Оформляя Premium подписку в нашем боте, для вас открывается доступ ко всем курсам, огромному количеству знаний и возможностей. Ежедневно пополняем новыми и актуальными материалами.\n\n" +
                                                                                     "Наша особенность - это огроамная база самых топовых обучающих курсов. Мы не публикуем старыи и неактуальные материалы, чтобы не тратить понапрасну ваше время! В нашей базе вы найдете весь самый востребовательный контент от таких именитых бизнес школ как SkillBox, Нетология, Яндекс.Практикум, Udemy, SkillFactory и многие другие.\n\n" +
                                                                                     "Premium подписка даёт вам:\n" +
                                                                                     "- Доступ ко всем обучающим курсам из разделов Онлайн школы и Обучающие курсы;" +
                                                                                     "- Новые актуальные материалы каждый день;\n" +
                                                                                     "- Возможность получать рассылку топовых обучений;\n" +
                                                                                     "- Регулярное обновление заблокированных ссылок.\n\n" +
                                                                                     "Стоимость Premium подписки:\n" +
                                                                                     "Доступ навсегда - " + price + " ₽", replyMarkup: Keyboard.subKey);
            }
            else if (data == "LessonCourse") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               var schemes = catalogs.GroupBy(x => x.Category).Select(x => x.First()).ToList();
               await client.SendTextMessageAsync(message.Chat.Id, "Курсы", replyMarkup: (InlineKeyboardMarkup)GetCourse(schemes));
            }
            else if (data.Contains("OpenCourse_")) {
               Connect.LoadUser(users);
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, users.Find(x => x.Id == message.Chat.Id).Id_list);
               } catch { }
               var schemes = catalogs.FindAll(x => x.Category.Replace(" ", string.Empty).Replace(",", string.Empty) == data.Split('_')[1]);
               var msg = await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, GetList(0, schemes) + "\n‼️Для просмотра курса введите его номер (перед названием)", Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: (InlineKeyboardMarkup)GetCatalog(0, schemes, schemes[0].Category));
               Connect.Query("update `User` set id_list = " + msg.MessageId + " where id = " + message.Chat.Id + ";");
            }
            else if (data == "EarningSchemes") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               Connect.LoadUser(users);
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, users.Find(x => x.Id == message.Chat.Id).Id_list);
               } catch { }
               var schemes = catalogs.FindAll(x => x.Category == "Схемы заработка");
               var msg = await client.SendTextMessageAsync(message.Chat.Id, GetList(0, schemes) + "\n‼️Для просмотра курса введите его номер (перед названием)", Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: (InlineKeyboardMarkup)GetCatalog(0, schemes, "Схемы заработка"));
               Connect.Query("update `User` set id_list = " + msg.MessageId + " where id = " + message.Chat.Id + ";");
            }
            else if (data.Contains("ChangeCatalogPage_")) {
               var schemes = catalogs.FindAll(x => x.Category == data.Split('_')[2]);
               await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, GetList(Convert.ToInt32(data.Split('_')[1]), schemes) + "\n\n‼️Для просмотра курса введите его номер (перед названием)", Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: (InlineKeyboardMarkup)GetCatalog(Convert.ToInt32(data.Split('_')[1]), schemes, data.Split('_')[2]));
            }
            else if (data == "GoToMain") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               Connect.LoadUser(users);
               if (users.Find(x => x.Id == message.Chat.Id).Sub == "active")
                  await client.SendPhotoAsync(message.Chat.Id, "AgACAgIAAxkBAAIEUWSHJx77F35S9B6HN8i_7gABueVmfwACTMgxG1N3OEjQJw2iZI-9fgEAAwIAA3gAAy8E", replyMarkup: Keyboard.subMainKey);
               else
                  await client.SendPhotoAsync(message.Chat.Id, "AgACAgIAAxkBAAIEUWSHJx77F35S9B6HN8i_7gABueVmfwACTMgxG1N3OEjQJw2iZI-9fgEAAwIAA3gAAy8E", replyMarkup: Keyboard.mainKey);
            }
            else if (data == "PayToSub") {
               if (users.Find(x => x.Id == message.Chat.Id).Sub == "active")
                  await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, "У вас уже есть активная Premium подпсика", replyMarkup: new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } }));
               else {
                  try {
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  } catch { }
                  int price = 999;
                  if (users.Find(x => x.Id == message.Chat.Id).Id_invited != 0)
                     price = 999 - (999 / 100 * 15);
                  await client.SendInvoiceAsync(message.Chat.Id, "Premium подписка", "Приобретение Premium подписки навсегда", message.Chat.Id.ToString(), "381764678:TEST:58972", "rub", new List<LabeledPrice>() { new LabeledPrice("Premium подписка", price * 100) });
               }
            }
            else if (data.Contains("GoToList_")) {
               try {
                  await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId, replyMarkup: null);
               } catch { }
               var item = catalogs.Find(x => x.Id == Convert.ToInt32(data.Split('_')[1]));
               if (item != null) {
                  var schemes = catalogs.FindAll(x => x.Category == item.Category);
                  double count = Math.Ceiling(Convert.ToDouble(schemes.Count) / 30);
                  if (schemes != null) {
                     for (int i = 0; i < count; i++) {
                        if (GetList(i, schemes).Contains(item.Id.ToString()) && GetList(i, schemes).Contains(item.Name)) {
                           Connect.LoadUser(users);
                           try {
                              await client.DeleteMessageAsync(message.Chat.Id, users.Find(x => x.Id == message.Chat.Id).Id_list);
                           } catch { }
                           var msg = await client.SendTextMessageAsync(message.Chat.Id, GetList(i, schemes) + "\n‼️Для просмотра курса введите его номер (перед названием)", Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: (InlineKeyboardMarkup)GetCatalog(i, schemes, item.Category));
                           Connect.Query("update `User` set id_list = " + msg.MessageId + " where id = " + message.Chat.Id + ";");
                           return;
                        }
                     }
                  }
               }
            }
            else if (data == "LocalArea") {
               Connect.LoadUser(users);
               var user = users.Find(x => x.Id == message.Chat.Id);
               if (user != null) {
                  string premium = "не подключена ⛔️", refer = "отсутствует ⛔️";
                  try {
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  } catch { }
                  if (user.Sub == "active")
                     premium = "подключена ✅";
                  if (user.Id_invited != 0)
                     refer = user.Id_invited + " ✅";
                  if (user.Sub == "active") {
                     if (user.Id_invited != 0)
                        await client.SendTextMessageAsync(message.Chat.Id, "Личный кабинет\n\nID: " + user.Id + "\nUsername: " + message.From.FirstName + "\nКод пригласившего: " + refer + "\nPremium подписка: " + premium + "\nБаланс: " + user.Balance + " ₽\n\nВаша реферальная ссылка: https://t.me/samouchki_bot?start=" + message.Chat.Id, replyMarkup: Keyboard.areaSubKey);
                     else
                        await client.SendTextMessageAsync(message.Chat.Id, "Личный кабинет\n\nID: " + user.Id + "\nUsername: " + message.From.FirstName + "\nКод пригласившего: " + refer + "\nPremium подписка: " + premium + "\nБаланс: " + user.Balance + " ₽\n\nВаша реферальная ссылка: https://t.me/samouchki_bot?start=" + message.Chat.Id, replyMarkup: Keyboard.areaSubRefKey);
                  }
                  else {
                     if (user.Id_invited != 0)
                        await client.SendTextMessageAsync(message.Chat.Id, "Личный кабинет\n\nID: " + user.Id + "\nUsername: " + message.From.FirstName + "\nКод пригласившего: " + refer + "\nPremium подписка: " + premium + "\nБаланс: " + user.Balance + " ₽\n\nВаша реферальная ссылка: https://t.me/samouchki_bot?start=" + message.Chat.Id, replyMarkup: Keyboard.areaKey);
                     else
                        await client.SendTextMessageAsync(message.Chat.Id, "Личный кабинет\n\nID: " + user.Id + "\nUsername: " + message.From.FirstName + "\nКод пригласившего: " + refer + "\nPremium подписка: " + premium + "\nБаланс: " + user.Balance + " ₽\n\nВаша реферальная ссылка: https://t.me/samouchki_bot?start=" + message.Chat.Id, replyMarkup: Keyboard.areaRefKey);
                  }
               }
            }
            else if (data == "ReferList") {
               var referals = users.FindAll(x => x.Id_invited == message.Chat.Id);
               if (referals != null) {
                  string response = "*Ваши рефералы*\n\n";
                  for (int i = 0; i < referals.Count; i++)
                     response += Convert.ToInt32(i + 1) + ". " + referals[i].Id + "\n";
                  response += "\nОбщее количество рефералов: " + referals.Count;
                  await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, response, Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: Keyboard.referKey);
               }
               else
                  await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, "У вас нет ни одного реферала", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: Keyboard.referKey);
            }
            else if (data == "GetMoney") {
               Connect.LoadUser(users);
               var user = users.Find(x => x.Id == message.Chat.Id);
               if (user != null) {
                  if (user.Balance >= 1000) {
                     // cont
                  }
                  else
                     await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, "⛔️ Вывод средств доступен от 1000 рублей", replyMarkup: Keyboard.referKey);
               }
            }
            else if (data == "none")
               return;
            return;
         } catch { return; }
      }

      private static async void UpdateData(object sender, UpdateEventArgs e)
      {
         try {
            var update = e.Update;
            if (update.ChannelPost != null) {
               if (update.ChannelPost.Chat.Id == specialChat) {
                  if (update.ChannelPost.ReplyToMessage != null) {
                     if (update.ChannelPost.ReplyToMessage.Text.Split(' ')[0] == "⛔️") {
                        string[] text = update.ChannelPost.ReplyToMessage.Text.Split(' ');
                        text[0] = "✅";
                        text[1] = "Есть";
                        text[2] = "Ответ";
                        string result = string.Empty;
                        foreach (var str in text)
                           result += str + " ";
                        await client.EditMessageTextAsync(specialChat, update.ChannelPost.ReplyToMessage.MessageId, result);
                        await client.SendTextMessageAsync(update.ChannelPost.ReplyToMessage.Text.Split('\n').Last(), "✅ Ответ на обращение:\n" + update.ChannelPost.Text);
                     }
                  }
               }
            }
            else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.PreCheckoutQuery) {
               Connect.Query("update `User` set sub = 'active' where id = " + update.PreCheckoutQuery.From.Id + ";");
               await client.AnswerPreCheckoutQueryAsync(update.PreCheckoutQuery.Id);
               await client.SendTextMessageAsync(update.PreCheckoutQuery.From.Id, "✅ Подписка успешно активирована", replyMarkup: new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } }));
               var user = users.Find(x => x.Id == update.PreCheckoutQuery.From.Id);
               if (user.Id_invited != 0) {
                  int price = users.Find(x => x.Id == user.Id_invited).Balance + (999 / 100 * 25);
                  Connect.Query("Update `User` set balance = " + price + " where id = " + user.Id_invited + ";");
               }
            }
            return;
         } catch { }
      }

      private static string GetList(int page, List<Catalog> schemes)
      {
         try {
            string response = string.Empty;
            for (int i = 30 * page; i < 30 * page + 30; i++)
               response += "<code>" + schemes[i].Id + "</code>. " + schemes[i].Name + "\n";
            return response;
         } catch { return null; }
      }

      private static InlineKeyboardButton[][] GetCatalog(int page, List<Catalog> schemes, string name)
      {
         try {
            var keyboard = new List<InlineKeyboardButton[]>();
            int down = -1;
            if (page != 0)
               down = page - 1;
            else
               down = page;
            int up = -1;
            if (schemes.Count / 30 >= page + 1)
               up = page + 1;
            else
               up = page;
            keyboard.Add(new[] { InlineKeyboardButton.WithCallbackData("<<", "ChangeCatalogPage_" + down + "_" + name),
               InlineKeyboardButton.WithCallbackData(Convert.ToInt32(page + 1) + " / " + Math.Ceiling(Convert.ToDouble(schemes.Count) / 30), "none"),
               InlineKeyboardButton.WithCallbackData(">>", "ChangeCatalogPage_" + up + "_" + name) });
            keyboard.Add(new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") });
            return keyboard.ToArray();
         } catch { return null; }
      }

      private static InlineKeyboardButton[][] GetCourse(List<Catalog> courses)
      {
         try {
            var keyboard = new List<InlineKeyboardButton[]>();
            InlineKeyboardButton[] board = new InlineKeyboardButton[0];
            for (int i = 0; i < courses.Count; i++) {
               if (board.Length == 2) {
                  keyboard.Add(board);
                  board = new InlineKeyboardButton[0];
               }
               if (courses[i].Category != "Схемы заработка") {
                  Array.Resize(ref board, board.Length + 1);
                  if (board[0] == null)
                     board[0] = InlineKeyboardButton.WithCallbackData(courses[i].Category, "OpenCourse_" + courses[i].Category.Replace(" ", string.Empty).Replace(",", string.Empty));
                  else
                     board[1] = InlineKeyboardButton.WithCallbackData(courses[i].Category, "OpenCourse_" + courses[i].Category.Replace(" ", string.Empty).Replace(",", string.Empty));
               }
               if (i + 1 >= courses.Count)
                  keyboard.Add(board);
            }
            keyboard.Add(new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") });
            return keyboard.ToArray();
         } catch { return null; }
      }
   }
}

#pragma warning restore CS0618 // Тип или член устарел