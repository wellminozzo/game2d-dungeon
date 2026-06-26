using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Core;

public abstract class Entity
{
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public Texture2D? Texture { get; set; }
    public Color Tint { get; set; } = Color.White;

    public Rectangle Bounds => new(
        (int)(Position.X - Size.X / 2f),
        (int)(Position.Y - Size.Y / 2f),
        (int)Size.X,
        (int)Size.Y);

    protected Entity(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;
    }

    public Rectangle GetTileRect(int tileSize)
    {
        return new Rectangle(
            (int)(Position.X - Size.X / 2f),
            (int)(Position.Y - Size.Y / 2f),
            (int)Size.X,
            (int)Size.Y);
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (Texture != null)
            spriteBatch.Draw(Texture, Bounds, Tint);
    }
}
