using System.Text.Json;
using System.IO;
using System.Collections.Generic;
namespace GameClient
{
    public class ProgressManager
    {
        List<ProgressDataPlayer> _latestProgressData;
        List<Player> _players;
        InventoryManager _inventoryManager;
        PlayerManager _playerManager;
        LevelManager _levelManager;
        CollectionManager _collectionManager;
        string _progressDataJson;
        string _fileName = "ProgressData.json";
        static public bool _saveFileAvailable;
        public ProgressManager()
        {
        }
        public void Initialize(List<Player> players, InventoryManager inventoryManager, PlayerManager playerManager, LevelManager levelManager, CollectionManager collectionManager)
        {
            _inventoryManager = inventoryManager;
            _players = players;
            _playerManager = playerManager;
            _levelManager = levelManager;
            _collectionManager = collectionManager;
            //if (File.Exists(_fileName))
            //{
            //    string jsonString = File.ReadAllText(_fileName);
            //    if (!string.IsNullOrEmpty(jsonString))
            //    {
            //        _latestProgressData = JsonSerializer.Deserialize<List<ProgressDataPlayer>>(jsonString);
            //        _progressDataJson = JsonSerializer.Serialize(_latestProgressData);
            //        _saveFileAvailable = true;
            //    }
            //}
        }
        public void CreateProgressData()
        {
            //foreach (var player in _players)
            //{
            //    _latestProgressData.Add(new ProgressDataPlayer(player._playerNum, LevelManager._currentLevel, player._animationNum, player._health,
            //    player._gun._id, player._inventory._inventory_rectangles, player._nameDisplay._text, player._inventory._gold, player._));
            //}
            //_progressDataJson = JsonSerializer.Serialize(_latestProgressData);
            //File.WriteAllText(_fileName, _progressDataJson);
        }
        public void UpdateProgressData()
        {
        }
        public void LoadData()
        {
            //if (_progressDataJson != null)
            //{
            //    int lastLevel;
            //    _latestProgressData = JsonSerializer.Deserialize<List<ProgressDataPlayer>>(_progressDataJson);
            //    foreach (var playerData in _latestProgressData)
            //    {
            //        _playerManager.AddPlayerFromData(playerData);
            //        _inventoryManager.ResetInventory();
            //        foreach (var item in playerData._item_list)
            //        {
            //            _inventoryManager.AddItemToInventory(_collectionManager.GetItem(item._itemID).DropAndCopy(true), false, item._amount);
            //        }
            //        _inventoryManager._gold = _latestProgressData._gold;
            //    }

            //    _levelManager.LoadNewLevel(lastLevel); // has to be in the end of the funcion, because calling CreateProgressData
            //}
        }
    }
}
