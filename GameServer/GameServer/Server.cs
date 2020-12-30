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
    public class Server : Game
    {
        public static Game_Server game;
        static void Main()
        {
            using (game = new Game_Server())
                game.Run();
            

        }
    }
}