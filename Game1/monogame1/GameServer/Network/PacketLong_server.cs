//using System.Collections.Generic;

//namespace GameServer
//{
//    public class PacketLong_Server : PacketStructure
//    {
//        List<NetworkPlayer> _players;
//        NetworkPlayer _player;
//        public PacketLong_Server(List<NetworkPlayer> players, NetworkPlayer player) : base()
//        {
//            UpdateType(3);
//            _players = players;
//            _player = player;
//        }
//        public void UpdatePacket()
//        {
//            WriteInt(_player.PlayerNum);
//        }

//    }
//}
