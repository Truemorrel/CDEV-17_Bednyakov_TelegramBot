///Learning task steam CDEV, module 11
///
namespace TelegramBot
{
    using System;
    using System.Threading.Tasks;
    using Telegram.Bot;
    internal class Program
    {

        static async Task Main(string[] args)
        {
            ///
            var botClient = new TelegramBotClient("5224117853:AAGCWDg8X_xYiBHeqBf86X8XKecH8FTI8lw");

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
        }
    }
}
