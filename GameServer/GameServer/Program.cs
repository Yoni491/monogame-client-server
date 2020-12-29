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
        
        static List<Player> player_list = new List<Player>();
        static Socket socket;
        static byte[] buffer = new byte[255];
        static void Main()
        {
            using (game = new Game1())
                game.Run();
            

        }
    }
}