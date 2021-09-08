using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Farm
{
    public class Bot
    {
        private static Bot bot;

        private readonly TelegramBotClient client;
        private readonly CancellationTokenSource cancellationTokenSource;

        private Bot()
        {
            client = new TelegramBotClient(Settings.API_TOKEN);
            cancellationTokenSource = new CancellationTokenSource();
        }

        public static Bot GetInstance()
        {
            if (null == bot)
            {
                bot = new Bot();
            }
            return bot;
        }

        public void Start()
        {
            client.StartReceiving(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                cancellationTokenSource.Token);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _                                       => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message
                || update.Message.Type != MessageType.Text)
            {
                return;
            }

            LogMessageFromChat(update.Message);

            if (update.Message.Text.Equals("/start"))
            {
                string responce = "Press to roll the dice";
                ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[][]
                    {
                        new KeyboardButton[] { "Press" }
                    },
                    resizeKeyboard: true);

                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: responce,
                    replyMarkup: replyKeyboardMarkup
                );
                Log.Information($"Respoce from bot:\r\n{responce}");
            }

            if (update.Message.Text.Equals("Press"))
            {
                string responce = "Dice are rolled";
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: responce
                );
                Message message1 = await botClient.SendStickerAsync(
                chatId: update.Message.Chat.Id,
                sticker: "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-fred.webp"
                );
                Log.Information($"Respoce from bot:\r\n{responce}");
            }
        }

        private static void LogMessageFromChat(Message message)
        {
            Log.Information(string.Format("From: {0} {1}\r\nCaht ID: {2}\r\nMessage: {3}",
                            message.Chat.FirstName, message.Chat.LastName,
                            message.Chat.Id,
                            message.Text));
        }
    }
}
