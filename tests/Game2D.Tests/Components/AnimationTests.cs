using Game2D.Core;

namespace Game2D.Tests;

public class AnimationTests
{
    [Fact]
    public void Animation_TotalDuration_CalculatedCorrectly()
    {
        var frames = new[]
        {
            new AnimationFrame(new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), 0.2f),
            new AnimationFrame(new Microsoft.Xna.Framework.Rectangle(16, 0, 16, 16), 0.3f),
        };

        var anim = new Animation("test", frames);

        Assert.Equal(0.5f, anim.TotalDuration, 3);
    }

    [Fact]
    public void Animation_DefaultIsLooping()
    {
        var frames = new[]
        {
            new AnimationFrame(new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), 0.1f),
        };

        var anim = new Animation("test", frames);

        Assert.True(anim.IsLooping);
    }

    [Fact]
    public void Animation_CanBeNonLooping()
    {
        var frames = new[]
        {
            new AnimationFrame(new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), 0.1f),
        };

        var anim = new Animation("test", frames, false);

        Assert.False(anim.IsLooping);
    }

    [Fact]
    public void AnimationFrame_StoresProperties()
    {
        var rect = new Microsoft.Xna.Framework.Rectangle(10, 20, 32, 32);
        var frame = new AnimationFrame(rect, 0.15f);

        Assert.Equal(rect, frame.SourceRect);
        Assert.Equal(0.15f, frame.Duration);
    }
}
