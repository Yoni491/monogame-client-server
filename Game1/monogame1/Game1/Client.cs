namespace GameClient
{
    public static class Client
    {
        public static Game_Client game;
        static void Main()
        {
            using (game = new Game_Client())
                game.Run();

        }
    }
}
