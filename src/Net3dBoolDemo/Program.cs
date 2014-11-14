using System;

namespace Net3dBoolDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var game = new TDemoWindow();

            // Run the game at 60 updates per second
            game.Run(60.0);
        }
    }
}
