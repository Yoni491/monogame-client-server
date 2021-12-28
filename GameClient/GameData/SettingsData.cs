using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class SettingsData
    {

        public string _IP { get; set; }
        public string _nameInGame { get; set; }
        public bool _soundOFF { get; set; }
        public bool _musicOFF { get; set; }
        public bool _fullScreenOFF { get; set; }

        private SettingsData()
        {

        }
        public SettingsData(string IP, string nameInGame, bool sound, bool music, bool fullscreen)
        {
            _IP = IP;
            _nameInGame = nameInGame;
            _soundOFF = sound;
            _musicOFF = music;
            _fullScreenOFF = fullscreen;
        }

    }
}
