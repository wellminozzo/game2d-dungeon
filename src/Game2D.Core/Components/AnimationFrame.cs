using Microsoft.Xna.Framework;

namespace Game2D.Core;

public struct AnimationFrame
{
    public Rectangle SourceRect { get; }
    public float Duration { get; }

    public AnimationFrame(Rectangle sourceRect, float duration)
    {
        SourceRect = sourceRect;
        Duration = duration;
    }
}
