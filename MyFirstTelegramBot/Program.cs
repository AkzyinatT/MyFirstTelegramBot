// See https://aka.ms/new-console-template for more information
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyFirstTelegramBot
{

    class Program
    {
static ITelegramBotClient bot = new TelegramBotClient("TOKEN");
public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message; 
                switch(message.Text.ToLower()) 
                { 
                    case "/start":  
                    { 
                        await botClient.SendTextMessageAsync(message.Chat, "Welcome to your personal horoscope page!"); 
                        await botClient.SendTextMessageAsync(message.Chat, """ 
                        <b><u>Bot menu</u></b>: 
                        /menu           - menu
                        /poll           - send a poll 
                        /register       - reply 
                        /send_sticker   - send sticker
                        """, parseMode: ParseMode.Html,  
                        replyMarkup: new ReplyKeyboardRemove()); // also remove keyboard to clean-up things 
                        break; 
                    }
                    case "/menu":  
                    { 
                        InlineKeyboardButton[] row1 = new InlineKeyboardButton[]  
                        { 
                            InlineKeyboardButton.WithUrl("Today's horoscope", "https://www.horoscope.com/us/horoscopes/general/index-horoscope-general-daily.aspx") 
                        }; 

                         InlineKeyboardButton[] row2 = new InlineKeyboardButton[]  
                        { 
                            InlineKeyboardButton.WithUrl("Weekly horoscope", "https://www.horoscope.com/us/horoscopes/general/index-horoscope-general-weekly.aspx") 
                        }; 

                         InlineKeyboardButton[] row3 = new InlineKeyboardButton[]  
                        { 
                            InlineKeyboardButton.WithUrl("Monthly horoscope", "https://www.horoscope.com/us/horoscopes/general/index-horoscope-general-monthly.aspx") 
                        }; 
 
                        InlineKeyboardButton[][] buttons = new InlineKeyboardButton[][] 
                        { 
                            row1,
                            row2,
                            row3
                        }; 
 
                        // Define and Initialize 
                        InlineKeyboardMarkup inline = new InlineKeyboardMarkup(buttons); 
                        await bot.SendTextMessageAsync(message.Chat, "Menu:", replyMarkup: inline); 
                        break; 
                    }
                    case "/poll": 
                    { 
                        await botClient.SendPollAsync(message.Chat, "What is your horoscope sign?", ["Taurus", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius", "Capricorn", "Aquarius", "Pisces"], isAnonymous: false, allowsMultipleAnswers: true); 
                        break; 
                    }
                    case "/register":
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "You will receive your horoscope");
                        break;
                    }

                    case "/send_sticker":
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "https://telegrambots.github.io/book/docs/sticker-dali.webp");
                        break;
                    }


                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}