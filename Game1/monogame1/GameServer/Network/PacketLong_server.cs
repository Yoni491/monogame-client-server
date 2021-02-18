//using System.Collections.Generic;

//namespace GameServer
//{
//    public class PacketLong_Server : PacketStructure
//    {
//        List<NetworkPlayerOld> _players;
//        NetworkPlayerOld _player;
//        public PacketLong_Server(List<NetworkPlayerOld> players, NetworkPlayerOld player) : base()
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
