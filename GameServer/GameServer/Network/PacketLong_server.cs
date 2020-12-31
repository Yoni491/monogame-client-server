using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class PacketLong_Server : PacketStructure
    {
        List<Player> _players;
        public PacketLong_Server(List<Player> players) : base(5)
        {
            _players = players;
        }
        public void UpdatePacketPlayers()
        {
            foreach (var player in _players)
            {
                player.UpdatePacketShort(this);
            }
        }
    }
}
