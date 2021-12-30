using GameClient.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace GameClient
{
    public class MainMenuScreen
    {
        Button _localGame, _onlineButton, _exitGame, _howToPlay;
        int _buttonHeight = 60;
        int _buttonWidth = 250;
        ScreenPoint _buttonPosition;
        Game_Client _game_client;
        Texture2D _menuBackgroundImage;
        GraphicsDevice _graphicsDevice;
        public CharacterSelectScreen _characterSelectScreen;
        public ConnectionScreen _connectionScreen;
        SelectSaveFileScreen _SelectSaveFileScreen;
        HowToPlayScreen _howToPlayScreen;
        public bool _showChooseCharacterScreen, _showConnectionScreen, _showSelectSaveFileScreen, _showHowToPlay;
        public static bool _connected;
        PlayerManager _playerManager;
        InputManager _inputManager;
        public MainMenuScreen()
        {
        }
        public void Initialize(Game_Client game_client, GraphicsDevice graphicsDevice, ProgressManager progressManager,
            SettingsDataManager settingsDataManager, PlayerManager playerManager, InputManager inputManager)
        {
            _playerManager = playerManager;
            _inputManager = inputManager;
            _connectionScreen = new ConnectionScreen(graphicsDevice, game_client, this, settingsDataManager);
            _characterSelectScreen = new CharacterSelectScreen(graphicsDevice, game_client, this, settingsDataManager, _playerManager, _inputManager);
            _SelectSaveFileScreen = new SelectSaveFileScreen(graphicsDevice, this, progressManager, game_client);
            _howToPlayScreen = new HowToPlayScreen(graphicsDevice, this, progressManager, game_client);
            _graphicsDevice = graphicsDevice;
            _game_client = game_client;
            _menuBackgroundImage = GraphicManager.getImage("backgroundUnboxingrave");
            _buttonPosition = new ScreenPoint(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _localGame = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White), Vector2.Zero, _buttonPosition, Color.Green, Color.Gray, "play local");
            _onlineButton = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White), new Vector2(0, _buttonHeight + 2), _buttonPosition, Color.Green, Color.Gray, "play online");
            _howToPlay = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White), new Vector2(0, _buttonHeight * 2 + 4), _buttonPosition, Color.Green, Color.Gray, "How to play");
            _exitGame = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White), new Vector2(0, _buttonHeight * 3 + 6), _buttonPosition, Color.Green, Color.Gray, "Exit game");
        }
        public void Update(GameTime gameTime)
        {
            if (_showChooseCharacterScreen)
            {
                _characterSelectScreen.Update(gameTime);
            }
            else if (_showConnectionScreen)
            {
                _connectionScreen.Update(gameTime);
            }
            else if (_showSelectSaveFileScreen)
            {
                _SelectSaveFileScreen.Update(gameTime);
            }
            else if (_showHowToPlay)
            {
                _howToPlayScreen.Update(gameTime);
            }
            else
            {
                if (_localGame.Update(gameTime))
                {
                    _showSelectSaveFileScreen = true;
                }
                if (_onlineButton.Update(gameTime))
                {
                    _showConnectionScreen = true;
                }
                if (_howToPlay.Update(gameTime))
                {
                    _showHowToPlay = true;
                }
                if (_exitGame.Update(gameTime))
                {
                    _game_client.Exit();
                    Environment.Exit(0);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_menuBackgroundImage, new Rectangle(0, 0, GraphicManager.screenWidth, GraphicManager.screenHeight), Color.White);
            if (_showChooseCharacterScreen)
            {
                _characterSelectScreen.Draw(spriteBatch);
            }
            else if (_showConnectionScreen)
            {
                _connectionScreen.Draw(spriteBatch);
            }
            else if (_showSelectSaveFileScreen)
            {
                _SelectSaveFileScreen.Draw(spriteBatch);
            }
            else if (_showHowToPlay)
            {
                _howToPlayScreen.Draw(spriteBatch);
            }
            else
            {
                _localGame.Draw(spriteBatch);
                _onlineButton.Draw(spriteBatch);
                _howToPlay.Draw(spriteBatch);
                _exitGame.Draw(spriteBatch);
            }
        }
        public void ResetGraphics()
        {
            _characterSelectScreen.ResetGraphics();
            _connectionScreen.ResetGraphics();
            _SelectSaveFileScreen.ResetGraphics();
            _howToPlayScreen.ResetGraphics();
            _buttonPosition.vector2.X = _graphicsDevice.Viewport.Bounds.Width / 2 - 120;
            _buttonPosition.vector2.Y = _graphicsDevice.Viewport.Bounds.Height / 2 - 30;
            _localGame.ResetGraphics();
            _onlineButton.ResetGraphics();
            _howToPlay.ResetGraphics();
            _exitGame.ResetGraphics();
        }
    }
}
