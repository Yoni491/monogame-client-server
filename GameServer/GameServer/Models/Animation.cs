using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    public class Animation
    {
        public int CurrentFrame { get;set; }
        public int FrameCount { get; set; }
        public int FrameHeight { get { return Texture.Height; } }
        public float FrameSpeed { get; set; }
        public int FrameWidth { get { return Texture.Width/FrameCount; } }
        public bool isLooping { get; set; }

        public Texture2D Texture { get; private set; }
        public Animation(Texture2D texture,int frameCount)
        {
            Texture = texture;
            FrameCount = frameCount;
            isLooping = true;
            FrameSpeed = 0.2f;
        }
    }
}
