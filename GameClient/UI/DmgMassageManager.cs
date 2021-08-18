using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class DmgMassageManager
    {
        static GraphicsDevice _graphicsDevice;
        static List<DmgMessage> dmgMessageList;
        public DmgMassageManager(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            dmgMessageList = new List<DmgMessage>();
        }
        static public void CreateDmgMessage(int dmg,Vector2 position, Color color,float timeDisplayed=1.5f)
        {
            dmgMessageList.Add(new DmgMessage(_graphicsDevice, dmg, position, timeDisplayed, color));
        }
        public void Update(GameTime gameTime)
        {
            foreach (var item in dmgMessageList)
            {
                item.Update(gameTime);
            }
            dmgMessageList.RemoveAll(x => x._destroy);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in dmgMessageList)
            {
                item.Draw(spriteBatch);
            }
        }
        static public void Reset()
        {
            dmgMessageList.Clear();
        }
    }
}
