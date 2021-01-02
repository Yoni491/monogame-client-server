using System.Collections.Generic;

namespace GameServer
{
    public class PacketShort_Server : PacketStructure
    {
        List<Player> _players;
        public PacketShort_Server(List<Player> players) : base()
        {
            _players = players;
            base.UpdateType(2);
        }
        public void updatePacket()
        {
            foreach (var player in _players)
            {
                player.UpdatePacketShort(this);
            }
        }
    }
}
