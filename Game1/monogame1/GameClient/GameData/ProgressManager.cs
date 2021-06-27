using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    class ProgressManager
    {
        ProgressData _progressData;
        int _level;
        Player _player;
        InventoryManager _inventoryManager;
        public ProgressManager(InventoryManager inventoryManager, Player player)
        {

        }
        public void CreateProgressData()
        {
            _progressData = new ProgressData();
        }
        public void UpdateProgressData()
        {

        }
    }
}
