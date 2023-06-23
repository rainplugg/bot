using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Course.Classes
{
   public class Keyboard
   {
      public readonly static InlineKeyboardMarkup mainKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Обучающие курсы", "LessonCourse") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Схемы заработка", "EarningSchemes") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Premium подписка", "PremiumSub") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Личный кабинет", "LocalArea") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Помощь", "Help") } });

      public readonly static InlineKeyboardMarkup subMainKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Обучающие курсы", "LessonCourse") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Схемы заработка", "EarningSchemes") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Личный кабинет", "LocalArea") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Помощь", "Help") } });

      public readonly static InlineKeyboardMarkup helpKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Написать специалисту", "WriteSpecialChat") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Правообладателям", "Copyholder") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } });

      public readonly static InlineKeyboardMarkup holderKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Написать специалисту", "WriteSpecialChat") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } });

      public readonly static InlineKeyboardMarkup subKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Оплатить", "PayToSub") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } });

      public readonly static InlineKeyboardMarkup courseKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Написать специалисту", "WriteSpecialChat") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Premium подписка", "PremiumSub") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Назад", "GoToList") } });
      
      public readonly static InlineKeyboardMarkup courseSubKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Написать специалисту", "WriteSpecialChat") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Назад", "GoToList") } });

      public readonly static InlineKeyboardMarkup areaSubKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Рефералы", "ReferList") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Вывод средств", "GetMoney") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } });

      public readonly static InlineKeyboardMarkup areaSubRefKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Рефералы", "ReferList") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Вывод средств", "GetMoney") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } });

      public readonly static InlineKeyboardMarkup areaKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Рефералы", "ReferList") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Вывод средств", "GetMoney") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Premium подписка", "PremiumSub") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } });

      public readonly static InlineKeyboardMarkup areaRefKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Рефералы", "ReferList") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Вывод средств", "GetMoney") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("Premium подписка", "PremiumSub") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } });

      public readonly static InlineKeyboardMarkup referKey = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Назад", "LocalArea") },
                                                                                      new[] { InlineKeyboardButton.WithCallbackData("На главную", "GoToMain") } });
   }
}
