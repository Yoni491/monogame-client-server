using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.Xna.Framework;

namespace GameClient
{
    public class ProgressManager
    {
        ProgressData _latestProgressData;
        Player _player;
        InventoryManager _inventoryManager;
        PlayerManager _playerManager;
        LevelManager _levelManager;
        CollectionManager _collectionManager;
        string _progressDataJson;
        string _fileName;
        public ProgressManager()
        {
        }
        public void Initialize(Player player, InventoryManager inventoryManager, PlayerManager playerManager, LevelManager levelManager, CollectionManager collectionManager)
        {
            _inventoryManager = inventoryManager;
            _player = player;
            _playerManager = playerManager;
            _levelManager = levelManager;
            _collectionManager = collectionManager;
        }
        public void CreateProgressData()
        {
            _latestProgressData = new ProgressData(_player._playerNum,LevelManager._currentLevel,_player._animationNum,_player._health,_player._gun._id,_inventoryManager._item_list);
            _fileName = "ProgressData.json";
            _progressDataJson = JsonSerializer.Serialize(_latestProgressData);
            File.WriteAllText(_fileName, _progressDataJson);
        }
        public void UpdateProgressData()
        {

        }
        public void LoadData(bool loadLatest = false, string dataName = null)
        {
            if (loadLatest)
            {

                _latestProgressData = JsonSerializer.Deserialize<ProgressData>(_progressDataJson);
                _playerManager.AddPlayerFromData(_latestProgressData);
                _levelManager.LoadNewLevel(_latestProgressData._level);
                foreach (var item in _latestProgressData._item_list)
                {
                    Item currentItem = _collectionManager.GetItem(item._itemID).Drop(true);
                    for (int i = 0; i < item._amount; i++)
                    {

                    }
                    _inventoryManager.AddItemToInventory() ;
                }
            }
        }
    }
}
