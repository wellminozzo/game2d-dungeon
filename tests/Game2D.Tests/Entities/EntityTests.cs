using Game2D.Core;

namespace Game2D.Tests;

public class EntityTests
{
    [Fact]
    public void Bounds_CalculatedFromPositionAndSize()
    {
        var entity = new TestEntity(
            new Microsoft.Xna.Framework.Vector2(100, 100),
            new Microsoft.Xna.Framework.Vector2(32, 32));

        var bounds = entity.Bounds;

        Assert.Equal(84, bounds.X);
        Assert.Equal(84, bounds.Y);
        Assert.Equal(32, bounds.Width);
        Assert.Equal(32, bounds.Height);
    }

    private class TestEntity(Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 size)
        : Entity(position, size) { }
}
