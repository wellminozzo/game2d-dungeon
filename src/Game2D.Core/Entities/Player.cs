using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Core;

public class Player : Entity
{
    private readonly InputManager _input;
    private readonly TileMap _tileMap;
    private readonly float _speed;
    private AnimatedSprite _anim = null!;
    private Vector2 _facing;

    public Player(Vector2 position, InputManager input, TileMap tileMap, float speed = 180f)
        : base(position, new Vector2(24, 24))
    {
        _input = input;
        _tileMap = tileMap;
        _speed = speed;
        _facing = new Vector2(0, 1);
    }

    public void SetAnimatedSprite(AnimatedSprite anim)
    {
        _anim = anim;
        _anim.Play("idle_down");
        _anim.Origin = new Vector2(12, 12);
    }

    public override void Update(GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var dir = _input.Direction;
        var movement = dir * _speed * dt;

        TryMove(movement);

        if (dir != Vector2.Zero)
            _facing = dir;

        UpdateAnimation(dir);
        _anim.Update(gameTime);
    }

    private void UpdateAnimation(Vector2 dir)
    {
        var isMoving = dir != Vector2.Zero;
        var prefix = isMoving ? "walk" : "idle";

        string animName;
        if (Math.Abs(_facing.X) > Math.Abs(_facing.Y))
            animName = $"{prefix}_{(_facing.X > 0 ? "right" : "left")}";
        else
            animName = $"{prefix}_{(_facing.Y > 0 ? "down" : "up")}";

        _anim.Play(animName);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_anim == null) return;
        _anim.Tint = Color.White;
        _anim.Draw(spriteBatch, Position);
    }

    private void TryMove(Vector2 movement)
    {
        var ts = _tileMap.TileSize;

        var newX = Position.X + movement.X;
        var playerRectX = new Rectangle(
            (int)(newX - Size.X / 2f),
            (int)(Position.Y - Size.Y / 2f),
            (int)Size.X,
            (int)Size.Y);

        if (!CollidesWithUnwalkable(playerRectX, ts))
            Position = new Vector2(newX, Position.Y);

        var newY = Position.Y + movement.Y;
        var playerRectY = new Rectangle(
            (int)(Position.X - Size.X / 2f),
            (int)(newY - Size.Y / 2f),
            (int)Size.X,
            (int)Size.Y);

        if (!CollidesWithUnwalkable(playerRectY, ts))
            Position = new Vector2(Position.X, newY);

        var halfSize = Size / 2f;
        Position = new Vector2(
            MathHelper.Clamp(Position.X, halfSize.X, _tileMap.PixelWidth - halfSize.X),
            MathHelper.Clamp(Position.Y, halfSize.Y, _tileMap.PixelHeight - halfSize.Y));
    }

    private bool CollidesWithUnwalkable(Rectangle playerRect, int tileSize)
    {
        var minTile = new Point(playerRect.Left / tileSize, playerRect.Top / tileSize);
        var maxTile = new Point((playerRect.Right - 1) / tileSize, (playerRect.Bottom - 1) / tileSize);

        for (var x = minTile.X; x <= maxTile.X; x++)
        for (var y = minTile.Y; y <= maxTile.Y; y++)
        {
            if (!_tileMap.IsWalkable(x, y))
                return true;
        }

        return false;
    }
}
