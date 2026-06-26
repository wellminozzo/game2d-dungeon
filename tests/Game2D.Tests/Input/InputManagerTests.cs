using Game2D.Core;

namespace Game2D.Tests;

public class InputManagerTests
{
    [Fact]
    public void Direction_InitiallyZero()
    {
        var input = new InputManager();
        input.Update();

        Assert.Equal(Microsoft.Xna.Framework.Vector2.Zero, input.Direction);
    }

    [Fact]
    public void IsDown_AfterUpdate_ReturnsFalse()
    {
        var input = new InputManager();
        input.Update();

        Assert.False(input.IsDown(Microsoft.Xna.Framework.Input.Keys.A));
    }

    [Fact]
    public void IsPressed_Initially_ReturnsFalse()
    {
        var input = new InputManager();
        input.Update();

        Assert.False(input.IsPressed(Microsoft.Xna.Framework.Input.Keys.Enter));
    }
}
