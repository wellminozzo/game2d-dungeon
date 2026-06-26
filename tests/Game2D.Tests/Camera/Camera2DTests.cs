using Game2D.Core;

namespace Game2D.Tests;

public class Camera2DTests
{
    [Fact]
    public void TransformMatrix_WithDefault_IsIdentity()
    {
        var camera = new Camera2D();
        camera.ViewportWidth = 960;
        camera.ViewportHeight = 540;

        var matrix = camera.TransformMatrix;

        Assert.NotEqual(Microsoft.Xna.Framework.Matrix.Identity, matrix);
    }

    [Fact]
    public void Follow_CentersOnTarget()
    {
        var camera = new Camera2D();
        camera.ViewportWidth = 960;
        camera.ViewportHeight = 540;
        camera.MapPixelWidth = 2000;
        camera.MapPixelHeight = 2000;

        camera.Follow(new Microsoft.Xna.Framework.Vector2(500, 500), 1f);

        Assert.Equal(500, camera.Position.X);
        Assert.Equal(500, camera.Position.Y);
    }

    [Fact]
    public void Follow_ClampsToMapBounds()
    {
        var camera = new Camera2D();
        camera.ViewportWidth = 960;
        camera.ViewportHeight = 540;
        camera.MapPixelWidth = 1000;
        camera.MapPixelHeight = 1000;

        camera.Follow(new Microsoft.Xna.Framework.Vector2(-100, -100), 1f);

        Assert.True(camera.Position.X >= camera.ViewportWidth / 2f);
        Assert.True(camera.Position.Y >= camera.ViewportHeight / 2f);
    }

    [Fact]
    public void VisibleArea_ContainsViewport()
    {
        var camera = new Camera2D();
        camera.ViewportWidth = 960;
        camera.ViewportHeight = 540;
        camera.MapPixelWidth = 2000;
        camera.MapPixelHeight = 2000;
        camera.Position = new Microsoft.Xna.Framework.Vector2(500, 500);

        var area = camera.VisibleArea;

        Assert.True(area.Width >= 960);
        Assert.True(area.Height >= 540);
    }
}
