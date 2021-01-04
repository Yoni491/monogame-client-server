using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GameClient
{
    public class InventoryManager
    {
        int width = 35;
        int height = 35;
        Texture2D _inventoryBlock;
        Vector2 _position;
        public InventoryManager(GraphicsDevice graphicDevice)
        {
            _inventoryBlock = new Texture2D(Client.game.GraphicsDevice, width, height);
            Color[] data2 = new Color[width * height];
            for (int i = 0; i < data2.Length; ++i)
            {
                if(i >= data2.Length - height|| i<= height || i % width == 0 || (i+1) % width == 0)
                    data2[i] = Color.White;
                else
                    data2[i] = Color.Black;
            }
            _inventoryBlock.SetData(data2);

            _position =new Vector2(3*(graphicDevice.Viewport.Bounds.Width / 10),
            graphicDevice.Viewport.Bounds.Height - (graphicDevice.Viewport.Bounds.Height / 10));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 8; i++)
            {
                spriteBatch.Draw(_inventoryBlock, _position + new Vector2(width*i + i,0), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0.98f);
            }
            

            //float life_precentage = ((float)_health_left / _total_health) * healthbar_width;
            //Color[] data = new Color[healthbar_width];
            //for (int i = 0; i < data.Length; ++i) data[i] = (i < life_precentage ? Color.Green : Color.Red);
            //_healthbar.SetData(data);
            //spriteBatch.Draw(_healthbar, _position, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0.99f);
        }
    }
}
