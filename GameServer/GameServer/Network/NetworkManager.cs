using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class NetworkManager
    {
        float _timer = 0;
        static List<Socket> _socket_list;
        private List<Player> _players;
        public NetworkManager(List<Socket> socket_list,List<Player> players)
        {
            _socket_list = socket_list;
            _players = players;
        }
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= 1.5f)
            {
                _timer = 0;
                PacketShort_Server packet1 = new PacketShort_Server(_players);
                packet1.UpdatePacketPlayers();
                foreach (var socket in _socket_list)
                {
                    if (socket.Connected)
                    {
                        socket.Send(packet1.Data());
                    }
                }
            }
        }
    }
}
