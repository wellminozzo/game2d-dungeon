using Microsoft.Xna.Framework;

namespace Game2D.Core;

public class Camera2D
{
    public Vector2 Position { get; set; }
    public float Zoom { get; set; } = 1f;
    public float Rotation { get; set; }
    public int ViewportWidth { get; set; }
    public int ViewportHeight { get; set; }
    public int MapPixelWidth { get; set; }
    public int MapPixelHeight { get; set; }

    public Matrix TransformMatrix =>
        Matrix.CreateTranslation(new Vector3(-Position, 0)) *
        Matrix.CreateRotationZ(Rotation) *
        Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
        Matrix.CreateTranslation(new Vector3(ViewportWidth * 0.5f, ViewportHeight * 0.5f, 0));

    public void Follow(Vector2 target, float lerp = 0.1f)
    {
        var smoothed = Vector2.Lerp(Position, target, lerp);
        Position = smoothed;

        var halfW = ViewportWidth / 2f / Zoom;
        var halfH = ViewportHeight / 2f / Zoom;

        Position = new Vector2(
            MathHelper.Clamp(Position.X, halfW, MapPixelWidth - halfW),
            MathHelper.Clamp(Position.Y, halfH, MapPixelHeight - halfH));
    }

    public Rectangle VisibleArea
    {
        get
        {
            var halfW = ViewportWidth / 2f / Zoom;
            var halfH = ViewportHeight / 2f / Zoom;
            var x = (int)(Position.X - halfW);
            var y = (int)(Position.Y - halfH);
            return new Rectangle(x - 1, y - 1, (int)(halfW * 2) + 2, (int)(halfH * 2) + 2);
        }
    }
}
