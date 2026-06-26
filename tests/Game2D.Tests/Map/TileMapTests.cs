using Game2D.Core;

namespace Game2D.Tests;

public class TileMapTests
{
    [Fact]
    public void Constructor_CreatesGrid_WithCorrectDimensions()
    {
        var map = new TileMap(50, 40);

        Assert.Equal(50, map.Width);
        Assert.Equal(40, map.Height);
        Assert.Equal(32, map.TileSize);
        Assert.Equal(1600, map.PixelWidth);
        Assert.Equal(1280, map.PixelHeight);
    }

    [Fact]
    public void Fill_SetsAllTiles()
    {
        var map = new TileMap(10, 10);
        map.Fill(Tile.Floor);
        map.Fill(Tile.Wall);

        Assert.False(map.IsWalkable(5, 5));
    }

    [Fact]
    public void GetTile_OutOfBounds_ReturnsWall()
    {
        var map = new TileMap(10, 10);

        Assert.False(map.GetTile(-1, 0).IsWalkable);
        Assert.False(map.GetTile(0, -1).IsWalkable);
        Assert.False(map.GetTile(10, 0).IsWalkable);
        Assert.False(map.GetTile(0, 10).IsWalkable);
    }

    [Fact]
    public void SetTile_StoresTile()
    {
        var map = new TileMap(10, 10);
        map.SetTile(3, 4, Tile.Floor);

        var tile = map.GetTile(3, 4);
        Assert.Equal(TileType.Floor, tile.Type);
        Assert.True(tile.IsWalkable);
    }

    [Fact]
    public void SetTile_OutOfBounds_DoesNothing()
    {
        var map = new TileMap(10, 10);
        map.SetTile(100, 100, Tile.Floor);

        var tile = map.GetTile(100, 100);
        Assert.Equal(TileType.Wall, tile.Type);
    }

    [Fact]
    public void WorldToTile_ConvertsCorrectly()
    {
        var map = new TileMap(10, 10);

        var tile = map.WorldToTile(new Microsoft.Xna.Framework.Vector2(64, 96));

        Assert.Equal(2, tile.X);
        Assert.Equal(3, tile.Y);
    }

    [Fact]
    public void TileToWorld_ReturnsCenter()
    {
        var map = new TileMap(10, 10);

        var pos = map.TileToWorld(2, 3);

        Assert.Equal(80f, pos.X);
        Assert.Equal(112f, pos.Y);
    }

    [Fact]
    public void GetTileRect_ReturnsCorrectRectangle()
    {
        var map = new TileMap(10, 10);

        var rect = map.GetTileRect(2, 3);

        Assert.Equal(64, rect.X);
        Assert.Equal(96, rect.Y);
        Assert.Equal(32, rect.Width);
        Assert.Equal(32, rect.Height);
    }

    [Fact]
    public void DrawRoom_CreatesWallsAndFloor()
    {
        var map = new TileMap(20, 20);
        map.Fill(Tile.Void);
        map.DrawRoom(2, 2, 6, 5);

        Assert.True(map.IsWalkable(3, 3));
        Assert.False(map.IsWalkable(2, 2));
        Assert.False(map.IsWalkable(7, 2));
        Assert.False(map.IsWalkable(2, 6));
        Assert.False(map.IsWalkable(7, 6));
    }

    [Fact]
    public void IsWalkable_FloorTile_ReturnsTrue()
    {
        var map = new TileMap(10, 10);
        map.SetTile(3, 3, Tile.Floor);

        Assert.True(map.IsWalkable(3, 3));
    }

    [Fact]
    public void IsWalkable_WallTile_ReturnsFalse()
    {
        var map = new TileMap(10, 10);
        map.SetTile(3, 3, Tile.Wall);

        Assert.False(map.IsWalkable(3, 3));
    }
}
