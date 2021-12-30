using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameClient
{
    public class SettingsData
    {
        public string _IP { get; set; }
        public List<String> _nameInputTextBoxList { get; set; }
        public bool _soundOFF { get; set; }
        public bool _musicOFF { get; set; }
        public bool _fullScreenOFF { get; set; }
        private SettingsData()
        {
        }
        public SettingsData(string IP, List<TextInputBox> nameTextBoxList, bool sound, bool music, bool fullscreen)
        {
            _nameInputTextBoxList = new List<string>();
            _IP = IP;
            foreach (TextInputBox box in nameTextBoxList)
            {
                if (string.IsNullOrEmpty(box._text))
                    _nameInputTextBoxList.Add("");
                else
                    _nameInputTextBoxList.Add(box._text);
            }
            _soundOFF = sound;
            _musicOFF = music;
            _fullScreenOFF = fullscreen;
        }
    }
}
