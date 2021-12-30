using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameClient
{
    public class InventoryManager
    {
        List<Inventory> _inventories;
        GraphicsDevice _graphicsDevice;
        ItemManager _itemManager;
        public InventoryManager()
        {
            _inventories = new List<Inventory>();
        }
        public void Initialize(GraphicsDevice graphicsDevice, ItemManager itemManager)
        {
            _graphicsDevice = graphicsDevice;
            _itemManager = itemManager;
        }
        public Inventory AddInventory()
        {
            Inventory inventory = new Inventory(_graphicsDevice, _itemManager);
            _inventories.Add(inventory);
            return inventory;
        }
        public void Update()
        {
        }
        public void Draw(SpriteBatch spriteBatch)
        {
        }
        public void ResetGraphics()
        {
        }
    }
}
