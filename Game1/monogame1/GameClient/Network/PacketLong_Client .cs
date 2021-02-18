namespace GameClient
{
    public class PacketLong_Client : Packet
    {
        Player _player;
        public PacketLong_Client(Player player) : base()
        {
            UpdateType(4);
            _player = player;
            _player.UpdatePacketLong(this);
        }
    }
}
