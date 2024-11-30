namespace MagicVilla_VillaAPI.Logging
{
    public class LoggingV2 : ILogging
    {
        void ILogging.Log(string message, string type)
        {
            if (type == "error")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Error - " + message);
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                if (type == "warning")
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Warning - " + message);
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.WriteLine(message);
                }
            }
        }
    }
}
