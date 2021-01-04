namespace GameClient
{
    public class PacketShort_Client : PacketStructure
    {
        Player _player;
        public PacketShort_Client(Player player) : base()
        {
            _player = player;
        }
        public void UpdatePacket()
        {
            updateType(1);
            _player.UpdatePacketShort(this);
        }
    }
}
