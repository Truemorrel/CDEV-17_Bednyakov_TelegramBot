// <copyright file="Program.cs" company="CDEV-17">
// Copyright (c) CDEV-17. All rights reserved.
// </copyright>
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Task_11_3_7
{
    /// <summary>
    /// Program class.
    /// </summary>
    internal class Program
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
        /// Method initialize bot instance with credentials.
        /// </summary>
        public void Initialize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
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
        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Получено сообщение в чате: {e.Message.Chat.Id}.");

                Bot_SendMessageAsync(e.Message.Chat, "Вы написали:\n" + e.Message.Text);
            }
        }

        /// <summary>
        /// Method sending text messages to chat.
        /// </summary>
        /// <param name="chatId"> ID of the chat. </param>
        /// <param name="message"> Text message. </param>
        /// <returns> Retuns nothing. </returns>
        private async Task Bot_SendMessageAsync(Chat chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId: chatId, text: "Вы написали:\n" + message);
        }
    }
}
