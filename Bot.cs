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
        private FarmDice farmDice;

        private readonly TelegramBotClient client;
        private readonly CancellationTokenSource cancellationTokenSource;

        private Bot()
        {
            client = new TelegramBotClient(Settings.API_TOKEN);
            cancellationTokenSource = new CancellationTokenSource();
            farmDice = new FarmDice();
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
                farmDice = new FarmDice();
                Log.Information($"Respoce from bot:\r\n{responce}");
            }

            if (update.Message.Text.Equals("Press"))
            {
                var dice = farmDice.RollDice();
                await botClient.SendStickerAsync(
                    chatId: update.Message.Chat.Id,
                    sticker: "https://github.com/pavelsokolovan/Farm/raw/main/Stickers/" + dice.Item1 + ".webp"
                );
                await botClient.SendStickerAsync(
                    chatId: update.Message.Chat.Id,
                    sticker: "https://github.com/pavelsokolovan/Farm/raw/main/Stickers/" + dice.Item2 + ".webp"
                );
                string responce = $"Dice are rolled: {dice.Item1} and {dice.Item2}";
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
