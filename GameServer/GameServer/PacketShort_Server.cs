using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class PacketShort_Server : PacketStructure
    {
        List<Player> _players;
        public PacketShort_Server(List<Player> players) : base(1000,2)
        {
            _players = players;
        }
        public void UpdatePacket()
        {
            foreach (var player in _players)
            {
                player.UpdatePacketShort(this);
            }
        }
    }
}
