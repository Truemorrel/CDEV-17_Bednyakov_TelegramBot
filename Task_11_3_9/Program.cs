// <copyright file="Program.cs" company="CDEV-17">
// Copyright (c) CDEV-17. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Task_11_3_9
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

    /// <summary>
    /// Class running bot client.
    /// </summary>
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
            logic = new BotMessageLogic(this);
        }

        /// <summary>
        /// Start message event handling.
        /// </summary>
        public void Start()
        {
            botClient.OnMessage += logic.Bot_OnMessage;
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
        /// Method sending text messages to chat.
        /// </summary>
        /// <param name="chatId"> ID of the chat. </param>
        /// <param name="message"> Text message. </param>
        public void Bot_SendMessage(Chat chatId, string message)
        {
            botClient.SendTextMessageAsync(chatId: chatId, text: message);
        }
    }

    /// <summary>
    /// Class contains message methods.
    /// </summary>
    public class BotMessageLogic
    {
        /// <summary>
        /// BotWorker member object.
        /// </summary>
        private BotWorker botWorker;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotMessageLogic"/> class.
        /// </summary>
        /// <param name="worker"> Binding worker class object. </param>
        public BotMessageLogic(BotWorker worker)
        {
            botWorker = worker;
        }

        /// <summary>
        /// Response automation.
        /// </summary>
        /// <param name="chatId"> chat object </param>
        /// <param name="message"> message to send </param>
        public void Response(Chat chatId, string message)
        {
            botWorker.Bot_SendMessage(chatId, message);
        }

        /// <summary>
        /// Message handler.
        /// </summary>
        /// <param name="sender">A source of the event object. </param>
        /// <param name="e">Event data object.</param>
        public void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Получено сообщение в чате: {e.Message.Chat.Id}.");
                
                Response(e.Message.Chat, "Вы написали:\n" + e.Message.Text);
            }
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
    }
}
