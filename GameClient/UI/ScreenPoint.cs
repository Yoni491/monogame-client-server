using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient.UI
{
    public class ScreenPoint
    {

        public Vector2 vector2;

        public ScreenPoint(float x, float y)
        {
            this.vector2 = new Vector2((int)x, (int)y);
        }
    }
}
