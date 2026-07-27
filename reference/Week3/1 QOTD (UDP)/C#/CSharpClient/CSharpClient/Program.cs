using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace CSharpClient
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        static void AdjustWindow()
        {
            System.Windows.Rect rc = System.Windows.SystemParameters.WorkArea;
            IntPtr hwndFound = GetConsoleWindow();
            MoveWindow(hwndFound, 0, 0, (int)rc.Width / 2, (int)rc.Height / 2, true);
        }
        static void Main(string[] args)
        {
            AdjustWindow();
            
            byte[] data = new byte[1024];
            string input, stringData;
            IPEndPoint ipep = new IPEndPoint( IPAddress.Parse("104.9.242.101"), 17); // QOTD Server
            //IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050); // locel Echo Server demo

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


            string welcome = "Hello";
            data = Encoding.ASCII.GetBytes(welcome);
            server.SendTo(data, data.Length, SocketFlags.None, ipep);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)sender;

            data = new byte[1024];
            int recv = server.ReceiveFrom(data, ref Remote);

            Console.WriteLine("Accessing QOTD server");

            while (true)
            {
                Console.Write("Message to send: ");
                input = Console.ReadLine();
                if (input == "exit")
                    break;

                Console.WriteLine("Sending text: '{0}'", input);
                server.SendTo(Encoding.ASCII.GetBytes(input), Remote);
                data = new byte[1024];
                recv = server.ReceiveFrom(data, ref Remote);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine("Data Received:");
                Console.WriteLine(stringData);
                Console.WriteLine();
            }
            Console.WriteLine("Stopping client");
            server.Close();
        }
    }
}
