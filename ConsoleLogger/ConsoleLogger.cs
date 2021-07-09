using System;
using TriangleLogoDrawer.ApplicationCore.Interfaces;

namespace ConsoleLogger
{
    public class ConsoleLogger : ILogger
    {
        private const string endingProccess = "{0} ending {1}. object {2}";
        private const string endedProccess = "{0} ended {1}. object {2}";
        private const string startingProccess = "{0} starting {1}. object {2}";
        private const string startededProccess = "{0} started {1}. object {2}";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public void EndingProccess(object sender, string proccess)
        {
            Console.WriteLine(string.Format(endingProccess, nameof(sender), proccess, sender));
        }

        public void EndedProccess(object sender, string proccess)
        {
            Console.WriteLine(string.Format(endedProccess, nameof(sender), proccess, sender));
        }

        public void LogMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void StartedProccess(object sender, string proccess)
        {
            Console.WriteLine(string.Format(startededProccess, nameof(sender), proccess, sender));
        }

        public void StartingProccess(object sender, string proccess)
        {
            Console.WriteLine(string.Format(startingProccess, nameof(sender), proccess, sender));
        }
    }
}
