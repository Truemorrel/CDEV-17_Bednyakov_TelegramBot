// <copyright file="Program.cs" company="CDEV-17">
// Copyright (c) CDEV-17. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Task_11_3_19
{
    /// <summary>
    /// Program class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args"> Not using. </param>
        public static void Main(string[] args)
        {
            var bot = new BotWorker();
            bot.Initialize();
            bot.Start();

            Console.WriteLine("Напишите stop для прекращения работы");

            string command;
            do
            {
                command = Console.ReadLine();
            }
            while (command != "stop");

            bot.Stop();
        }
    }

    public class BotWorker
    {
        /// <summary>
        /// Bot client descriptor.
        /// </summary>
        private ITelegramBotClient botClient;

        /// <summary>
        /// Bot message logic descriptor.
        /// </summary>
        private BotMessageLogic logic;

        /// <summary>
        /// Method initialize bot instance with credentials.
        /// </summary>
        public void Initialize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            logic = new BotMessageLogic(botClient);
        }

        /// <summary>
        /// Start message event handling.
        /// </summary>
        public void Start()
        {
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }

        /// <summary>
        /// Stop message event handling.
        /// </summary>
        public void Stop()
        {
            botClient.StopReceiving();
        }

        /// <summary>
        /// Message handler.
        /// </summary>
        /// <param name="sender">A source of the event object. </param>
        /// <param name="e">Event data object.</param>
        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null)
            {
                await logic.Response(e);
            }
        }
        ///// <summary>
        ///// Method sending text messages to chat.
        ///// </summary>
        ///// <param name="chatId"> ID of the chat. </param>
        ///// <param name="message"> Text message. </param>
        //public void Bot_SendMessage(Chat chatId, string message)
        //{
        //    botClient.SendTextMessageAsync(chatId: chatId, text: message);
        //}
    }

    /// <summary>
    /// Class running bot client.
    /// </summary>
    /// <summary>
    /// Class contains message methods.
    /// </summary>
    public class BotMessageLogic
    {
        /// <summary>
        /// BotWorker member object.
        /// </summary>
        private Messenger messanger;
        private ITelegramBotClient botClient;

        private Dictionary<long, Conversation> chatList;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotMessageLogic"/> class.
        /// </summary>
        /// <param name="worker"> Binding worker class object. </param>
        public BotMessageLogic(ITelegramBotClient botClient)
        {
            messanger = new Messenger();
            chatList = new Dictionary<long, Conversation>();
            this.botClient = botClient;
        }

        /// <summary>
        /// Response automation.
        /// </summary>
        /// <param name="chatId"> chat object </param>
        /// <param name="message"> message to send </param>
        public async Task Response(MessageEventArgs e)
        {
            var Id = e.Message.Chat.Id;

            if (!chatList.ContainsKey(Id))
            {
                var newchat = new Conversation(e.Message.Chat);

                chatList.Add(Id, newchat);
            }

            var chat = chatList[Id];

            chat.AddMessage(e.Message);

            await SendTextMessage(chat);
        }

        private async Task SendTextMessage(Conversation chat)
        {
            var text = messanger.CreateTextMessage(chat);

            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    class Messenger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateTextMessage(Conversation chat)
        {
            var text = "";
            switch (chat.GetLastMessage())
            {
                case "/saymehi":
                    text = "привет"; break;
                case "/askme":
                    text = "как дела?"; break;
                default:
                    var delimiter = ",";
                    text = "История ваших сообщений: " + string.Join(delimiter, chat.GetTextMessages().ToArray());
                    break;
            }
            return text;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Conversation
    {
        private Chat telegramChat;

        private List<Message> telegramMessages;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();

            foreach (var message in telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }

            return textMessages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long GetId() => telegramChat.Id;

        /// <summary>
        /// 
        /// </summary>
        public string GetLastMessage() => telegramMessages[telegramMessages.Count - 1].Text;
    }

    /// <summary>
    /// Telegram bot account credentials class.
    /// </summary>
    public static class BotCredentials
    {
        /// <summary>
        /// Access token.
        /// </summary>
        public static readonly string BotToken = "5224117853:AAGCWDg8X_xYiBHeqBf86X8XKecH8FTI8lw";
    }
}
