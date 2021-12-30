using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace GameClient
{
    public class NameGenerator
    {
        static string[] names = new string[18300];
        int i = 0;
        public NameGenerator()
        {
            foreach (string line in System.IO.File.ReadLines(Directory.GetCurrentDirectory() + "/Content/etc/names.txt"))
            {
                names[i++] = line;
            }
        }
        public static string GenerateRandomName()
        {
            Random x = new Random();
            return names[(int)((float)x.NextDouble() * 18200)];
        }
    }
}