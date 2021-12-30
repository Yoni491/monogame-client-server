using GameClient.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace GameClient
{
    public class CharacterSelectScreen
    {
        Button _returnToMain;
        Button _startGame;
        ScreenPoint _cardsPoint;
        private GraphicsDevice _graphicsDevice;
        private Game_Client _game_Client;
        private MainMenuScreen _menuManager;
        SettingsDataManager _settingsDataManager;
        List<CharacterSelectorCard> _cards;
        int _cardsAmount = 4, _cardWidth = 300, _cardHeight = 350;
        PlayerManager _playerManager;
        InputManager _inputManager;
        public CharacterSelectScreen(GraphicsDevice graphicsDevice, Game_Client game_Client, MainMenuScreen menuManager,
            SettingsDataManager settingsDataManager, PlayerManager playerManager, InputManager inputManager)
        {
            _playerManager = playerManager;
            _inputManager = inputManager;
            _graphicsDevice = graphicsDevice;
            _game_Client = game_Client;
            _menuManager = menuManager;
            _cards = new List<CharacterSelectorCard>();
            _settingsDataManager = settingsDataManager;
            _cardsPoint = new ScreenPoint(_graphicsDevice.Viewport.Bounds.Width / 2, GraphicManager.screenHeight / 4);
            int firstCardPosition = GetFirstCardsPosition();
            for (int i = 0; i < _cardsAmount; i++)
            {
                _cards.Add(new CharacterSelectorCard(_graphicsDevice, _cardsPoint, new Vector2((int)firstCardPosition + _cardWidth * i + i * 10, 0), _inputManager, _playerManager));
            }
            _startGame = new Button(GraphicManager.getRectangleTexture(300, 80, Color.White), new Vector2(-150, 360), _cardsPoint, Color.Green, Color.Gray, "StartGame");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(300, 80, Color.White), new Vector2(-150, 450), _cardsPoint, Color.Green, Color.Gray, "Return to main menu");
        }
        public void Update(GameTime gameTime)
        {
            _cards.ForEach(x => x.Update(gameTime));
            if (_startGame.Update(gameTime))
            {
                _settingsDataManager.CreateSettingsData();
                Game_Client._inMenu = false;
                _menuManager._showChooseCharacterScreen = false;
                //_game_Client._playerManager.ResetPlayer(characterNumbers[index], _NameInputTextBox._text);
                if (Game_Client._isMultiplayer)
                {
                    _game_Client._networkManager.SendPacket(gameTime, 2);
                }
                else
                {
                    _game_Client._levelManager.LoadNewLevel(LevelManager.startingLevel);
                }
            }
            if (_returnToMain.Update(gameTime))
            {
                _menuManager._showConnectionScreen = false;
                _menuManager._showChooseCharacterScreen = false;
                _game_Client._networkManager.CloseConnection();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _cards.ForEach(x => x.Draw(spriteBatch));
            _startGame.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _cardsPoint.vector2.X = (_graphicsDevice.Viewport.Bounds.Width / 2);
            _cardsPoint.vector2.Y = GraphicManager.screenHeight / 4;
            _startGame.ResetGraphics();
            _returnToMain.ResetGraphics();
            _cards.ForEach(x => x.ResetGraphics());
        }
        public int GetFirstCardsPosition()
        {
            return -_cardsAmount / 2 * _cardWidth - (_cardsAmount / 2 - 1) * 10;
        }
    }
}
