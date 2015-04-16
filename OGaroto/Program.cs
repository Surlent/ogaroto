using System;

namespace OGaroto
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GarotoGame game = new GarotoGame())
            {
                
                game.Run();
            }
        }
    }
}

