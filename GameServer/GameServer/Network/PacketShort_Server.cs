using System.Collections.Generic;

namespace GameServer
{
    public class PacketShort_Server : PacketStructure
    {
        List<NetworkPlayer> _players;
        public PacketShort_Server(List<NetworkPlayer> players) : base()
        {
            _players = players;
            base.UpdateType(2);
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
