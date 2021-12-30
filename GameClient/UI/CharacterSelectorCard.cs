using GameClient.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameClient
{
    public class CharacterSelectorCard
    {
        int index = 0;
        int[] characterNumbers = { 2, 3, 12, 13, 14, 15, 18, 19, 20 };
        Button _nextCharacter, _previousCharacter;
        public TextInputBox _NameInputTextBox;
        private ScreenMessage _enterNameMessage;
        ScreenPoint _refPoint, _localPoint = new ScreenPoint(0, 0);
        Vector2 _refPosition;
        Texture2D _selectedCardBackground, _unselectedCardBackground;
        bool _playerConnected;
        InputManager _inputManager;
        PlayerManager _playerManager;
        public CharacterSelectorCard(GraphicsDevice graphicsDevice, ScreenPoint refPoint, Vector2 refPosition, InputManager inputManager, PlayerManager playerManager)
        {
            _playerManager = playerManager;
            _inputManager = inputManager;
            _selectedCardBackground = GraphicManager.getImage("matrix");
            _unselectedCardBackground = GraphicManager.getImage("settingBackround");
            _refPoint = refPoint;
            _refPosition = refPosition;
            ResetPositionToRefrence();
            _enterNameMessage = new ScreenMessage(graphicsDevice, "Enter name:", _localPoint, new Vector2(10, 10));
            _NameInputTextBox = new TextInputBox(new Vector2(20, 90), _localPoint, false, clickToType: true, allowRandomName: true);
            _nextCharacter = new Button(GraphicManager.getRectangleTexture(30, 30, Color.White), new Vector2(195, 210), _localPoint, Color.Green, Color.Gray, ">");
            _previousCharacter = new Button(GraphicManager.getRectangleTexture(30, 30, Color.White), new Vector2(75, 210), _localPoint, Color.Green, Color.Gray, "<");
        }
        public void Update(GameTime gameTime)
        {
            if (_playerConnected)
            {
                _NameInputTextBox.Update(gameTime);
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
            }
            else
            {
                int capability = _inputManager.GetCapabilities();
                if (capability != -1)
                {
                    Player player = _playerManager.AddPlayer();
                    _inputManager.AssignCapabiltyToPlayer(capability, player);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_playerConnected)
            {
                spriteBatch.Draw(_selectedCardBackground, new Rectangle((int)_localPoint.vector2.X, (int)_localPoint.vector2.Y, 300, 350), Color.White);
                _NameInputTextBox.Draw(spriteBatch);
                _enterNameMessage.Draw(spriteBatch);
                _nextCharacter.Draw(spriteBatch);
                _previousCharacter.Draw(spriteBatch);
                spriteBatch.Draw(CollectionManager._playerAnimationManager[characterNumbers[index] - 1]._animations[1]._textures[0],
                    _localPoint.vector2 + new Vector2(80, 120), null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(_unselectedCardBackground, new Rectangle((int)_localPoint.vector2.X, (int)_localPoint.vector2.Y, 300, 350), Color.White);
            }
        }
        public void ResetGraphics()
        {
            ResetPositionToRefrence();
            _nextCharacter.ResetGraphics();
            _previousCharacter.ResetGraphics();
            _NameInputTextBox.ResetGraphics();
            _enterNameMessage.ResetGraphics();
        }
        public void ResetPositionToRefrence()
        {
            _localPoint.vector2 = _refPosition + _refPoint.vector2;
        }
    }
}
