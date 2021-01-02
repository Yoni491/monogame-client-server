using System.Collections.Generic;

namespace GameServer
{
    public class PacketLong_Server : PacketStructure
    {
        List<Player> _players;
        public PacketLong_Server(List<Player> players) : base()
        {
            UpdateType(5);
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
