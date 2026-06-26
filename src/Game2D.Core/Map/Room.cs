using Microsoft.Xna.Framework;

namespace Game2D.Core;

public struct Room
{
    public int X;
    public int Y;
    public int Width;
    public int Height;

    public int CenterX => X + Width / 2;
    public int CenterY => Y + Height / 2;
    public Point Center => new(CenterX, CenterY);

    public Rectangle Bounds => new(X, Y, Width, Height);
}
