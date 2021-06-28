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
        ProgressData _progressData;
        int _level;
        Player _player;
        InventoryManager _inventoryManager;
        public ProgressManager()
        {
        }
        public void Initialize(Player player)
        {
            _player = player;
        }
        public void CreateProgressData()
        {
            string fileName = "JsonTestPlayer1.json";
            string jsonString = JsonSerializer.Serialize(_player);
            File.WriteAllText(fileName, jsonString);
            //_progressData = new ProgressData(_player._playerNum,LevelManager._currentLevel,_player._health);
        }
        public void UpdateProgressData()
        {

        }
    }
}
