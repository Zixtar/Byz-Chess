using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Server
{
    class Program
    {
        private static TcpListener _server;
        private static String DateTrimise;
        private static NetworkStream[] DateNetwork = new NetworkStream[2];
        private static Thread[] t = new Thread[2];
        private static int counter = 0;

        static void Main(string[] args)
        {
            _server = new TcpListener(System.Net.IPAddress.Any, 3000);
            _server.Start();
            while (counter < 2)
            {
                TcpClient client = _server.AcceptTcpClient();
                System.Console.WriteLine("Client " + counter + " connected");
                t[counter] = new Thread(() => ServerLoop(client, counter - 1));
                t[counter].Start();
                counter++;
            }
        }
        static void ServerLoop(TcpClient client, int ThreadNr)
        {
            while (counter < 2) ;
            while (DateNetwork[0] == null || DateNetwork[1] == null)
            {
                DateNetwork[ThreadNr] = client.GetStream();
            }
            int writeTo = ThreadNr == 0 ? 1 : 0;
            using (StreamReader citireServer = new StreamReader(DateNetwork[ThreadNr]))
            using (StreamWriter scriere = new StreamWriter(DateNetwork[writeTo]))
            {
                while (true)
                {
                    try
                    {
                        scriere.AutoFlush = true;
                        string text = citireServer.ReadLine();

                        if (text[0] == 'C')
                        {
                            if (counter == 2)
                            {
                                scriere.WriteLine($"S{writeTo + 1}");
                            }
                        }

                        scriere.WriteLine(text);

                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.ToString());
                    }
                }
            }
        }
    }
}
