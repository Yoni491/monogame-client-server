namespace GameClient
{
    public class PacketLongClient : Packet
    {
        Player _player;
        public PacketLongClient(Player player) : base()
        {
            UpdateType(4);
            _player = player;
            _player.UpdatePacketLong(this);
        }
    }
}
