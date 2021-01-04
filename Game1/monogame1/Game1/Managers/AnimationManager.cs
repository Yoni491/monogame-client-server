using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameClient
{
    public class AnimationManager
    {
        private Animation _animation;
        private float _timer;
        public Vector2 Position { get; set; }
        public Animation Animation { get => _animation; set => _animation = value; }

        
        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }
        public void Draw(SpriteBatch spriteBatch,float layer)
        {
            spriteBatch.Draw(_animation.Texture,Position,
                new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0,_animation.FrameWidth,_animation.FrameHeight),
                Color.White,0,Vector2.Zero,1,SpriteEffects.None,
                layer);
        }
        public void Play(Animation animation)
        {
            if (_animation == animation)
                return;
            _animation = animation;
            _animation.CurrentFrame = 0;
            _animation.FrameSpeed = 0.2f;
        }
        public void Stop()
        {
            _timer = 0;
            _animation.CurrentFrame = 0;
        }
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;
                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }
        public Vector2 getAnimationPickPosition()
        {
            return new Vector2(_animation.FrameWidth / 2, _animation.FrameHeight);
        }
    }
}
