using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class PacketShort_Server : PacketStructure
    {
        List<Player> _players;
        public PacketShort_Server(List<Player> players) : base(2)
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
