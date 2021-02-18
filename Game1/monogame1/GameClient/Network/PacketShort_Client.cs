namespace GameClient
{
    public class PacketShort_Client : PacketStructure
    {
        Player _player;
        public PacketShort_Client() : base()
        {
            
        }
        public void Initialize(Player player)
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
