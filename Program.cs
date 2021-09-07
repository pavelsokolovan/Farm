using System;
using Serilog;

namespace Farm
{
    class Program
    {
        static void Main(string[] args)
        {
            PrepareLogger();
            
            Bot bot = Bot.GetInstance();

            try
            {
                bot.Start();
                Log.Information("bot is started!");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                throw e;
            }
            finally
            {
                bot.Stop();
                Log.Information("bot is stopped!");
            }
        }

        private static void PrepareLogger()
        {
            string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] | {Message:l}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console(outputTemplate: outputTemplate)
                        .WriteTo.File("logfile.log", outputTemplate: outputTemplate)
                        .CreateLogger();
        } 
    }
}
