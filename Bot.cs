using System;
using Telegram.Bot;

namespace Farm
{
    public class Bot
    {
        private static Bot bot;

        private readonly TelegramBotClient client;

        private Bot()
        {
            client = new TelegramBotClient(Settings.API_TOKEN);  
            client.OnMessage += BotOnMessage;
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
            allowedUsers = DataProvider.GetAllUsers();
            client.StartReceiving();
        }

        public void Stop()
        {
            client.StopReceiving();
        }

        private void BotOnMessage(object sender, MessageEventArgs e)
        {
            LogMessageFromChat(e.Message);

            if (IsMessageTypeAllowed(e.Message.Type))
            {
                string inMessage = e.Message.Text.ToLower();
                string outMessage;

                if (IsChatAllowed(e.Message.Chat.Id)
                    && IsLoginAllowed(inMessage))
                {
                    string userData = DataProvider.GetUserData(inMessage);
                    outMessage = userData;
                }
                else
                {
                    outMessage = string.Format("Hello {0}!", e.Message.Chat.FirstName);
                }
                SendMessageToChat(outMessage, e.Message.Chat.Id);

                Log.Information(string.Format("Respoce from bot:\r\n{0}", outMessage));
            }
        }

        private void SendMessageToChat(string message, long chatId)
        {
            client.SendTextMessageAsync(chatId, message);
        }
    }
}
