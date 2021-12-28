using System.Diagnostics;

namespace GameClient
{
    public static class Client
    {
        public static Game_Client game;
        static void Main(string[] args)
        {
            ////run more proceesses for debugging:
            //if (args.Length == 0)
            //{
            //    Process process = new Process();
            //    process.StartInfo.FileName = "GameClient.exe";
            //    process.StartInfo.Arguments = "1";
            //    process.Start();
            //}
            try
            {
                using (game = new Game_Client())
                    game.Run();

            }
            catch
            {
                System.Console.WriteLine("Client crushed");
            }


        }
    }
}
