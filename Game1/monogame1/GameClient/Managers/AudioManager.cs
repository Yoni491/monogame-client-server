using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameClient
{
    class AudioManager
    {
        private static ContentManager _contentManager;
        private static SoundEffect _soundEffect;
        private static Song _song;
        private static string _currentSong = "";
        static bool _muteSound;
        public AudioManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }
        static public void PlaySound(string text,float volume = 1)
        {
            if(!_muteSound)
            {
                _soundEffect = _contentManager.Load<SoundEffect>("Sound/" + text);
                if (text == "DestroyBox")
                    _soundEffect.Play(0.05f, 0, 0);
                else
                {
                    _soundEffect.Play(volume, 0, 0);
                }
            }
            
        }
        static public void PlaySong(int currentLevel = -1, bool menu = false)
        {
            string songName = "";
            if(menu)
            {
                songName = "3";
            }
            else if(currentLevel >=0 && currentLevel <= 3)
            {
                songName = "2";
            }
            else if(currentLevel >= 4 && currentLevel <= 30)
            {
                songName = "1";
            }
            if(_currentSong != songName)
            {
                _song = _contentManager.Load<Song>("Sound/Songs/" + songName);
                MediaPlayer.Play(_song);
                MediaPlayer.IsRepeating = true;
                //MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
                _currentSong = songName;
            }
        }
        static public void MuteSound(bool muteSound)
        {
            _muteSound = muteSound;
        }
        static public void MuteMusic(bool muteMusic)
        {
            MediaPlayer.IsMuted = muteMusic;
        }
    }

}
