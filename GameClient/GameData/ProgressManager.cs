using System.Text.Json;
using System.IO;

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
        string _fileName = "ProgressData.json";
        static public bool _saveFileAvailable;
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
            if (File.Exists(_fileName))
            {
                string jsonString = File.ReadAllText(_fileName);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    _latestProgressData = JsonSerializer.Deserialize<ProgressData>(jsonString);
                    _progressDataJson = JsonSerializer.Serialize(_latestProgressData);
                    _saveFileAvailable = true;
                }
            }
        }
        public void CreateProgressData()
        {
            _latestProgressData = new ProgressData(_player._playerNum,LevelManager._currentLevel,_player._animationNum,_player._health,
                _player._gun._id,_inventoryManager._inventory_rectangles,_player._nameDisplay._text,_inventoryManager._gold);
            _progressDataJson = JsonSerializer.Serialize(_latestProgressData);
            File.WriteAllText(_fileName, _progressDataJson);
        }
        public void UpdateProgressData()
        {

        }
        public void LoadData()
        {
            if (_progressDataJson != null)
            {
                _latestProgressData = JsonSerializer.Deserialize<ProgressData>(_progressDataJson);
                _playerManager.AddPlayerFromData(_latestProgressData);
                _inventoryManager.ResetInventory();
                foreach (var item in _latestProgressData._item_list)
                {
                    _inventoryManager.AddItemToInventory(_collectionManager.GetItem(item._itemID).DropAndCopy(true), false, item._amount);
                }
                _inventoryManager._gold = _latestProgressData._gold;
                _levelManager.LoadNewLevel(_latestProgressData._level); // has to be in the end of the funcion, because calling CreateProgressData
            }
        }
    }
}
