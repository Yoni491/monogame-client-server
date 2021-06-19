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
        public AudioManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }
        static public void PlaySound(string text)
        {
            _soundEffect = _contentManager.Load<SoundEffect>("Sound/" + text);
            if (text == "DestroyBox")
                _soundEffect.Play(0.05f, 0, 0);
            else
            {
                _soundEffect.Play();
            }
            //MediaPlayer.Play(_soundEffect);
            //  Uncomment the following line will also loop the song
            //  MediaPlayer.IsRepeating = true;
            //MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }
    }

}
