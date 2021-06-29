using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace GameClient
{
    public class ProgressManager
    {
        ProgressData _latestProgressData;
        int _level;
        Player _player;
        InventoryManager _inventoryManager;
        public ProgressManager()
        {
        }
        public void Initialize(Player player, InventoryManager inventoryManager)
        {
            _inventoryManager = inventoryManager;
            _player = player;
        }
        public void CreateProgressData()
        {
            _latestProgressData = new ProgressData(_player._playerNum,LevelManager._currentLevel,_player._animationNum,_player._health,_player._gun._id,_inventoryManager._item_list);
            string fileName = "ProgressData.json";
            string jsonString = JsonSerializer.Serialize(_latestProgressData);
            File.WriteAllText(fileName, jsonString);
        }
        public void UpdateProgressData()
        {

        }
        public void LoadData(bool loadLatest = false, string dataName = null)
        {
            if (loadLatest)
            {
                //PlayerManager.
            }
        }
    }
}
