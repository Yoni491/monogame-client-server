using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class MenuManager
    {
        Button _singlePlayer, _multiPlayer, _exit, _howToPlay,_highScores;
        int _buttonHeight = 30;
        int _buttonWidth = 100;
        Vector2 _buttonPosition;
        Game_Client _game_client;
        Texture2D _menuBackgroundImage;
        GraphicsDevice _graphicsDevice;
        CharacterSelectMenu _characterSelectMenu;
        bool _showChooseCharacter;
        public MenuManager(Game_Client game_client, GraphicsDevice graphicsDevice)
        {
            _characterSelectMenu = new CharacterSelectMenu(graphicsDevice, game_client);
            _graphicsDevice = graphicsDevice;
            _game_client = game_client;
            _menuBackgroundImage = GraphicManager.getImage("MenuBackground");
            _buttonPosition = new Vector2(7 * (graphicsDevice.Viewport.Bounds.Width / 10), graphicsDevice.Viewport.Bounds.Height / 2);
            _singlePlayer = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White),GraphicManager.GetBasicFont(), _buttonPosition, Color.Green,Color.Gray,"Single player");
            _multiPlayer = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White), GraphicManager.GetBasicFont(),_buttonPosition + new Vector2(0,_buttonHeight +2), Color.Green, Color.Gray, "Multiplayer");
            _howToPlay = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight * 2 + 4), Color.Green, Color.Gray, "How to play");
            _highScores = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight * 3 + 6), Color.Green, Color.Gray, "Scores");
            _exit = new Button(GraphicManager.getRectangleTexture(_buttonWidth, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight*4 + 8), Color.Green, Color.Gray, "Exit game");
        }
        public void Update(GameTime gameTime)
        {
            if (!_showChooseCharacter)
            {
                if (!UIManager._showSettings)
                {
                    if (_singlePlayer.Update(gameTime))
                    {
                        //_game_client._inMenu = false;
                        _showChooseCharacter = true;
                    }
                    if (_multiPlayer.Update(gameTime))
                    {
                        _game_client._inMenu = false;
                    }
                    if (_howToPlay.Update(gameTime))
                    {
                        _game_client._inMenu = false;
                    }
                    if (_highScores.Update(gameTime))
                    {
                        _game_client._inMenu = false;
                    }
                    if (_exit.Update(gameTime))
                    {
                        _game_client._inMenu = false;
                    }
                }
            }
            else
            {
                _characterSelectMenu.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_menuBackgroundImage, new Rectangle(0, 0, GraphicManager.screenWidth, GraphicManager.screenHeight), Color.White);
            if (!_showChooseCharacter)
            {
                _singlePlayer.Draw(spriteBatch);
                _multiPlayer.Draw(spriteBatch);
                _howToPlay.Draw(spriteBatch);
                _highScores.Draw(spriteBatch);
                _exit.Draw(spriteBatch);
            }
            else
            {
                _characterSelectMenu.Draw(spriteBatch);
            }
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(7 * (_graphicsDevice.Viewport.Bounds.Width / 10), _graphicsDevice.Viewport.Bounds.Height / 2);
            _singlePlayer.ResetGraphics(_buttonPosition);
            _multiPlayer.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight + 2));
            _howToPlay.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight * 2 + 4));
            _highScores.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight * 3 + 6));
            _exit.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight * 4 + 8));
        }
    }
}
