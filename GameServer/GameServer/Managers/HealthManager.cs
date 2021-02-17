using Microsoft.Xna.Framework;

namespace GameServer
{
    public class HealthManager
    {
        public int _total_health;

        public int _health_left;

        public Vector2 _position;


        public HealthManager(int total_health)
        {
            _total_health = total_health;
            _health_left = total_health;
        }

    }
}
