using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GameServer
{
    public class Program : Game
    {
        public static Game1 game;
        static List<Socket> socket_list = new List<Socket>();
        static List<Player> player_list = new List<Player>();
        static void Main()
        {
            Thread t = new Thread(accept_sockets);
            t.Start();

            t.Join();

            using (game = new Game1())
                game.Run();
            //acc2.Send(buffer, 0, buffer.Length, 0);

            //int rec = acc.Receive(buffer, 0, buffer.Length, 0);
            //Array.Resize(ref buffer, rec);
            //Console.WriteLine("Received: {0}", Encoding.Default.GetString(buffer));
            //sck.Close();
            //acc.Close();

        }
        static void accept_sockets()
        {
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Bind(new IPEndPoint(0, 1994));
            sck.Listen(0);
            int i = 0;
            while (true)
            {
                socket_list.Add(sck.Accept());
                byte[] buffer = Encoding.Default.GetBytes("Hello client");
                socket_list[i++].Send(buffer, 0, buffer.Length, 0);

            }
        }
        static void receieve_Info(int socket_number)
        {
            byte[] buffer = new byte[255];
            //Player player = new Player();
            while (true)
            {
                socket_list[socket_number].Receive(buffer, 0, buffer.Length, 0);
            }
        }
    }
}