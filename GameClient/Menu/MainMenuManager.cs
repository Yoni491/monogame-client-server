using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class MainMenuManager
    {
        Button _singlePlayer, _multiPlayerButton, _exitGame, _howToPlay;
        int _buttonHeight = 60;
        int _buttonWidth = 250;
        Vector2 _buttonPosition;
        Game_Client _game_client;
        Texture2D _menuBackgroundImage;
        GraphicsDevice _graphicsDevice;
        CharacterSelectMenu _characterSelectMenu;
        MultiplayerMenu _multiplayerMenu;
        SelectSaveFileScreen _SelectSaveFileScreen;
        HowToPlayScreen _howToPlayScreen;
        public bool _showChooseCharacterMenu, _showMultiplayerMenu,_showSelectSaveFileMenu,_showHowToPlay;
        public static bool _connected;
        public MainMenuManager()
        {
            
        }
        public void Initialize(Game_Client game_client, GraphicsDevice graphicsDevice, ProgressManager progressManager)
        {
            _multiplayerMenu = new MultiplayerMenu(graphicsDevice, game_client, this);
            _characterSelectMenu = new CharacterSelectMenu(graphicsDevice, game_client, this);
            _SelectSaveFileScreen = new SelectSaveFileScreen(graphicsDevice, this, progressManager,game_client);
            _howToPlayScreen = new HowToPlayScreen(graphicsDevice, this, progressManager, game_client);
            _graphicsDevice = graphicsDevice;
            _game_client = game_client;
            _menuBackgroundImage = GraphicManager.getImage("backgroundUnboxingrave");
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _singlePlayer = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White), _buttonPosition, Color.Green, Color.Gray, "Single player");
            _multiPlayerButton = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White),  _buttonPosition + new Vector2(0, _buttonHeight + 2), Color.Green, Color.Gray, "Multiplayer");
            _howToPlay = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White),  _buttonPosition + new Vector2(0, _buttonHeight * 2 + 4), Color.Green, Color.Gray, "How to play");
            _exitGame = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White),  _buttonPosition + new Vector2(0, _buttonHeight * 3 + 6), Color.Green, Color.Gray, "Exit game");
        }
        public void Update(GameTime gameTime)
        {
            
            if(_showChooseCharacterMenu)
            {
                _characterSelectMenu.Update(gameTime);
            }
            else if(_showMultiplayerMenu)
            {
                _multiplayerMenu.Update(gameTime);
            }
            else if(_showSelectSaveFileMenu)
            {
                _SelectSaveFileScreen.Update(gameTime);
            }
            else if(_showHowToPlay)
            {
                _howToPlayScreen.Update(gameTime);
            }
            else
            {
                if (_singlePlayer.Update(gameTime))
                {
                    _showSelectSaveFileMenu = true;
                }
                if (_multiPlayerButton.Update(gameTime))
                {
                    _showMultiplayerMenu = true;
                }
                if (_howToPlay.Update(gameTime))
                {
                    _showHowToPlay = true;
                }
                if (_exitGame.Update(gameTime))
                {
                    _game_client.Exit();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_menuBackgroundImage, new Rectangle(0, 0, GraphicManager.screenWidth, GraphicManager.screenHeight), Color.White);

            if (_showChooseCharacterMenu)
            {
                _characterSelectMenu.Draw(spriteBatch);
            }
            else if (_showMultiplayerMenu)
            {
                _multiplayerMenu.Draw(spriteBatch);
            }
            else if (_showSelectSaveFileMenu)
            {
                _SelectSaveFileScreen.Draw(spriteBatch);
            }
            else if (_showHowToPlay)
            {
                _howToPlayScreen.Draw(spriteBatch);
            }
            else
            {
                _singlePlayer.Draw(spriteBatch);
                _multiPlayerButton.Draw(spriteBatch);
                _howToPlay.Draw(spriteBatch);
                _exitGame.Draw(spriteBatch);
            }
        }
        public void ResetGraphics()
        {
            _characterSelectMenu.ResetGraphics();
            _multiplayerMenu.ResetGraphics();
            _SelectSaveFileScreen.ResetGraphics();
            _howToPlayScreen.ResetGraphics();
            _buttonPosition = new Vector2((_graphicsDevice.Viewport.Bounds.Width / 2 - 120), _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _singlePlayer.ResetGraphics(_buttonPosition);
            _multiPlayerButton.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight + 2));
            _howToPlay.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight * 2 + 4));
            _exitGame.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight * 3 + 6));
        }
    }
}
