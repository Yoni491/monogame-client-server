using System.Text.Json;
using System.IO;

namespace GameClient
{
    public class SettingsDataManager
    {
        SettingsData _latestSettingsData;
        string _settingsDataJson;
        string _fileName = "SettingsData.json";
        static public bool _saveFileAvailable;
        CharacterSelectScreen _characterSelectMenu;
        ConnectionScreen _multiplayerMenu;
        SettingsScreen _settingsScreen;
        public SettingsDataManager()
        {
        }
        public void Initialize(CharacterSelectScreen characterSelectMenu, ConnectionScreen multiplayerMenu, SettingsScreen settingsScreen)
        {
            _characterSelectMenu = characterSelectMenu;
            _multiplayerMenu = multiplayerMenu;
            _settingsScreen = settingsScreen;
            if (File.Exists(_fileName))
            {
                string jsonString = File.ReadAllText(_fileName);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    _latestSettingsData = JsonSerializer.Deserialize<SettingsData>(jsonString);
                    _settingsDataJson = JsonSerializer.Serialize(_latestSettingsData);
                    _saveFileAvailable = true;
                }
            }
            LoadData();
        }
        public void CreateSettingsData()
        {
            _latestSettingsData = new SettingsData(_multiplayerMenu._IPtextBox._text, _characterSelectMenu._NameInputTextBox._text, _settingsScreen._soundOFF, _settingsScreen._musicOFF, _settingsScreen._fullScreenOFF);
            _settingsDataJson = JsonSerializer.Serialize(_latestSettingsData);
            File.WriteAllText(_fileName, _settingsDataJson);
        }
        public void LoadData()
        {
            if (_settingsDataJson != null)
            {
                _latestSettingsData = JsonSerializer.Deserialize<SettingsData>(_settingsDataJson);

                if (_latestSettingsData._musicOFF)
                {
                    _settingsScreen._musicOFF = _latestSettingsData._musicOFF;
                    _settingsScreen._muteMusicButton.ChangeText("Unmute music");
                    AudioManager.MuteMusic(_latestSettingsData._musicOFF);
                }

                if (_latestSettingsData._soundOFF)
                {
                    _settingsScreen._soundOFF = _latestSettingsData._soundOFF;
                    _settingsScreen._muteSoundButton.ChangeText("Unmute sound");
                    AudioManager.MuteSound(_latestSettingsData._soundOFF);
                }

                if (_latestSettingsData._fullScreenOFF)
                {
                    _settingsScreen._fullScreenOFF = _latestSettingsData._fullScreenOFF;
                    _settingsScreen._fullScreenButton.ChangeText("Exit full Screen");
                    GraphicManager.ChangeToFullScreen(_latestSettingsData._fullScreenOFF);
                    Game_Client.ResetGraphics();
                }

                _characterSelectMenu._NameInputTextBox._text = _latestSettingsData._nameInGame;
                _multiplayerMenu._IPtextBox._text = _latestSettingsData._IP;
            }
        }
    }
}
