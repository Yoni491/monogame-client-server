using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameClient
{
    public class CharacterSelectMenu
    {
        int index = 0;
        int[] characterNumbers = { 2,3,12,13,14,15,18,19,20};
        Button _nextCharacter,_previousCharacter,_returnToMain;
        Button _startGame;
        Vector2 _buttonPosition;
        private Game_Client _game_Client;
        private  MenuManager _menuManager;

        public CharacterSelectMenu(GraphicsDevice graphicsDevice,Game_Client game_Client,MenuManager menuManager)
        {
            _buttonPosition = new Vector2(graphicsDevice.Viewport.Bounds.Width / 2, graphicsDevice.Viewport.Bounds.Height / 2);
            _nextCharacter = new Button(GraphicManager.getRectangleTexture(10, 10, Color.White), GraphicManager.GetBasicFont(), _buttonPosition, Color.Green, Color.Gray, ">");
            _previousCharacter = new Button(GraphicManager.getRectangleTexture(10, 10, Color.White), GraphicManager.GetBasicFont(), _buttonPosition +new Vector2(-12,0), Color.Green, Color.Gray, "<");
            _startGame = new Button(GraphicManager.getRectangleTexture(100, 30, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0,12), Color.Green, Color.Gray, "StartGame");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(160, 30, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, 100), Color.Green, Color.Gray, "Return to main menu");
            _game_Client = game_Client;
            _menuManager = menuManager;
        }
        public void Update(GameTime gameTime)
        {
            if(_nextCharacter.Update(gameTime))
            {
                index++;
                if(characterNumbers.Length <= index)
                {
                    index = 0;
                }
            }
            if (_previousCharacter.Update(gameTime))
            {
                index--;
                if (index < 0)
                {
                    index = characterNumbers.Length - 1;
                }
            }
            if(_startGame.Update(gameTime))
            {
                Game_Client._inMenu = false;
                _game_Client._playerManager._player._animationManager = CollectionManager.GetAnimationManagerCopy(characterNumbers[index],1.5f);
                _game_Client._playerManager._player._animationNum = characterNumbers[index];
                Console.WriteLine("b" + characterNumbers[index]);
                _menuManager._showChooseCharacterMenu = false;
            }
            if(_returnToMain.Update(gameTime))
            {
                _menuManager._showMultiplayerMenu = false;
                _menuManager._showChooseCharacterMenu = false;
                _game_Client._networkManager.CloseConnection();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _nextCharacter.Draw(spriteBatch);
            _previousCharacter.Draw(spriteBatch);
            _startGame.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
            spriteBatch.Draw(CollectionManager._playerAnimationManager[characterNumbers[index] - 1]._animations[1]._textures[0],new Vector2(GraphicManager.screenWidth / 2,GraphicManager.screenHeight / 2 -50),Color.White);
        }
    }
}
