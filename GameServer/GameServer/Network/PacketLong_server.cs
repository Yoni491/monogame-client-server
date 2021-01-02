using System.Collections.Generic;

namespace GameServer
{
    public class PacketLong_Server : PacketStructure
    {
        List<Player> _players;
        Player _player;
        public PacketLong_Server(List<Player> players, Player player) : base()
        {
            UpdateType(3);
            _players = players;
            _player = player;
        }
        public void UpdatePacket()
        {
            WriteInt(_player.PlayerNum);
        }

    }
}
