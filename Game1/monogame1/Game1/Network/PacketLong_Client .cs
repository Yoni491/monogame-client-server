namespace GameClient
{
    public class PacketLong_Client : PacketStructure
    {
        Player _player;
        public PacketLong_Client(Player player) : base()
        {
            updateType(4);
            _player = player;
            _player.UpdatePacketLong(this);
        }
    }
}
