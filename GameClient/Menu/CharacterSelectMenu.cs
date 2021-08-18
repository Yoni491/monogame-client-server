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
        private GraphicsDevice _graphicsDevice;
        private Game_Client _game_Client;
        private  MainMenuManager _menuManager;
        public TextInputBox _NameInputTextBox;
        private ScreenMessage _enterNameMessage;
        SettingsDataManager _settingsDataManager;

        public CharacterSelectMenu(GraphicsDevice graphicsDevice,Game_Client game_Client,MainMenuManager menuManager,SettingsDataManager settingsDataManager)
        {
            _settingsDataManager = settingsDataManager;
            _buttonPosition = new Vector2(GraphicManager.screenWidth / 2 - 100, GraphicManager.screenHeight / 2 - 150);
            _NameInputTextBox = new TextInputBox(_buttonPosition + new Vector2(-47,0), false);
            _enterNameMessage = new ScreenMessage(graphicsDevice, "Enter name:", _buttonPosition + new Vector2(-230, -10));
            _nextCharacter = new Button(GraphicManager.getRectangleTexture(30, 30, Color.White), _buttonPosition + new Vector2(115,90), Color.Green, Color.Gray, ">");
            _previousCharacter = new Button(GraphicManager.getRectangleTexture(30, 30, Color.White), _buttonPosition +new Vector2(-5,90), Color.Green, Color.Gray, "<");
            _startGame = new Button(GraphicManager.getRectangleTexture(300, 90, Color.White), _buttonPosition + new Vector2(-70,200), Color.Green, Color.Gray, "StartGame");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(300, 90, Color.White), _buttonPosition + new Vector2(-70, 300), Color.Green, Color.Gray, "Return to main menu");
            _graphicsDevice = graphicsDevice;
            _game_Client = game_Client;
            _menuManager = menuManager;
        }
        public void Update(GameTime gameTime)
        {
            _NameInputTextBox.Update();
            if (_nextCharacter.Update(gameTime))
            {
                index++;
                if (characterNumbers.Length <= index)
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
            if (_startGame.Update(gameTime))
            {
                _settingsDataManager.CreateSettingsData();
                Game_Client._inMenu = false;
                _menuManager._showChooseCharacterMenu = false;
                _game_Client._playerManager.ResetPlayer(characterNumbers[index], _NameInputTextBox._text);
                if (Game_Client._isMultiplayer)
                {
                    _game_Client._networkManager.SendPacket(gameTime,2);
                }
                else
                {
                    _game_Client._levelManager.LoadNewLevel(LevelManager.startingLevel);
                }
            }
            if (_returnToMain.Update(gameTime))
            {
                _menuManager._showMultiplayerMenu = false;
                _menuManager._showChooseCharacterMenu = false;
                _game_Client._networkManager.CloseConnection();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _NameInputTextBox.Draw(spriteBatch);
            _enterNameMessage.Draw(spriteBatch);
            _nextCharacter.Draw(spriteBatch);
            _previousCharacter.Draw(spriteBatch);
            _startGame.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
            spriteBatch.Draw(CollectionManager._playerAnimationManager[characterNumbers[index] - 1]._animations[1]._textures[0],
                new Vector2(GraphicManager.screenWidth / 2 -100,GraphicManager.screenHeight / 2 -150),null,Color.White,0,Vector2.Zero,3f,SpriteEffects.None,1);
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(GraphicManager.screenWidth / 2 - 100, GraphicManager.screenHeight / 2 - 150);
            _nextCharacter.ResetGraphics(_buttonPosition + new Vector2(115, 90));
            _previousCharacter.ResetGraphics(_buttonPosition + new Vector2(-5, 90));
            _startGame.ResetGraphics(_buttonPosition + new Vector2(-70, 200));
            _returnToMain.ResetGraphics(_buttonPosition + new Vector2(-70, 300));
            _NameInputTextBox.ResetGraphics(_buttonPosition + new Vector2(-47,0));
            _enterNameMessage.ResetGraphics(_buttonPosition + new Vector2(-230, -10));
        }
    }
}
