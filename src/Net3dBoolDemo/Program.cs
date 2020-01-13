using System;
using OpenTK;

namespace Net3dBoolDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Toolkit.Init(new ToolkitOptions { 
                Backend = PlatformBackend.PreferX11,
            });

            var demo = new TDemoWindow();

            // Run the game at 60 updates per second
            demo.Run(60.0);
        }
    }
}
