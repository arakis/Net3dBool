using System;

namespace Net3dBoolDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var demo = new TDemoWindow();

            // Run the game at 60 updates per second
            demo.Run(60.0);
        }
    }
}
