using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Core;

public class AnimatedSprite
{
    public Texture2D Texture { get; }
    public Vector2 Origin { get; set; }
    public Color Tint { get; set; } = Color.White;

    private readonly Dictionary<string, Animation> _animations;
    private Animation? _currentAnim;
    private float _timer;
    private int _currentFrame;
    private bool _playing;

    public string? CurrentAnimationName { get; private set; }
    public bool IsFinished { get; private set; }

    public AnimatedSprite(Texture2D texture, Vector2? origin = null)
    {
        Texture = texture;
        Origin = origin ?? Vector2.Zero;
        _animations = new Dictionary<string, Animation>();
    }

    public void AddAnimation(string name, Animation anim)
    {
        _animations[name] = anim;
    }

    public void Play(string name)
    {
        if (!_animations.TryGetValue(name, out var anim) || anim == _currentAnim)
            return;

        _currentAnim = anim;
        CurrentAnimationName = name;
        _currentFrame = 0;
        _timer = 0f;
        _playing = true;
        IsFinished = false;
    }

    public void Stop()
    {
        _playing = false;
        _currentFrame = 0;
        _timer = 0f;
    }

    public void Update(GameTime gameTime)
    {
        if (!_playing || _currentAnim == null || _currentAnim.Frames.Length == 0)
            return;

        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        var frame = _currentAnim.Frames[_currentFrame];

        if (_timer >= frame.Duration)
        {
            _timer -= frame.Duration;
            _currentFrame++;

            if (_currentFrame >= _currentAnim.Frames.Length)
            {
                if (_currentAnim.IsLooping)
                {
                    _currentFrame = 0;
                }
                else
                {
                    _currentFrame = _currentAnim.Frames.Length - 1;
                    _playing = false;
                    IsFinished = true;
                }
            }
        }
    }

    public Rectangle CurrentSourceRect
    {
        get
        {
            if (_currentAnim == null || _currentFrame >= _currentAnim.Frames.Length)
                return new Rectangle(0, 0, Texture.Width, Texture.Height);

            return _currentAnim.Frames[_currentFrame].SourceRect;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None)
    {
        spriteBatch.Draw(
            Texture,
            position,
            CurrentSourceRect,
            Tint,
            0f,
            Origin,
            1f,
            effects,
            0f);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale, SpriteEffects effects = SpriteEffects.None)
    {
        spriteBatch.Draw(
            Texture,
            position,
            CurrentSourceRect,
            Tint,
            0f,
            Origin,
            scale,
            effects,
            0f);
    }
}
