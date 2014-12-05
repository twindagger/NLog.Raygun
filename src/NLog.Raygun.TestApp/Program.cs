using System;
using NLog;

namespace NLog.Raygun.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            Console.WriteLine("Sending Message to RayGun...");
            
            logger.Info("This is a test!");

            try
            {
                throw new Exception("Test Exception");
            }
            catch (Exception exception)
            {
                logger.Error("Test Error", exception);
            }

            Console.WriteLine("Finished...");
            Console.Read();
        }
    }
}
