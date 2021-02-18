using Microsoft.Xna.Framework;
namespace GameServer
{
    public class Server : Game
    {
        public static Game_Server game;
        static void Main()
        {
            using (game = new Game_Server())
                game.Run();


        }
    }
}