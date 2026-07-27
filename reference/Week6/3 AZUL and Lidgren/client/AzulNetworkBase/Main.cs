using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace AzulNetworkBase
{
    class Program
    {
        //**********   Window auto adjustment stuff. Just ignore ***********
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static public void AdjustWindow()
        {
            Rect rc = System.Windows.SystemParameters.WorkArea;

            IntPtr hwndFound = GetConsoleWindow(); // Console window adjustment
            MoveWindow(hwndFound, 0, (int)rc.Height / 2, (int)rc.Width / 2, (int)rc.Height / 2, true);

            IntPtr hwndFoundg = FindWindow(null, NetworkGame.WindowCaption); // Game window adjustment
            MoveWindow(hwndFoundg, 0, 0, NetworkGame.Width, NetworkGame.Height, true);
        }
        //*******************************************************************

        static void Main(string[] args)
        {
            // Create the instance
            NetworkGame game = new NetworkGame();
            Debug.Assert(game != null);

            // Start the game
            game.Run();
        }
    }
}
