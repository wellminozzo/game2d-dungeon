using Game2D.Core;

namespace Game2D.Tests;

public class EnemyManagerTests
{
    [Fact]
    public void SpawnEnemies_EmptyRooms_NoEnemies()
    {
        var manager = new EnemyManager();
        var map = new TileMap(20, 20);
        map.Fill(Tile.Floor);

        manager.SpawnEnemies(new List<Room>(), map, 42);

        Assert.Equal(0, manager.Count);
    }

    [Fact]
    public void SpawnEnemies_SkipsFirstRoom_WithEnemies()
    {
        var manager = new EnemyManager();
        var map = new TileMap(30, 30);
        map.Fill(Tile.Floor);

        var rooms = new List<Room>
        {
            new() { X = 2, Y = 2, Width = 6, Height = 6 },
            new() { X = 10, Y = 2, Width = 6, Height = 6 },
        };

        manager.SpawnEnemies(rooms, map, 123);

        Assert.True(manager.Count >= 1);
    }

    [Fact]
    public void SpawnEnemies_SameSeed_SameEnemyCount()
    {
        var map = new TileMap(30, 30);
        map.Fill(Tile.Floor);

        var rooms = new List<Room>
        {
            new() { X = 2, Y = 2, Width = 6, Height = 6 },
            new() { X = 10, Y = 2, Width = 6, Height = 6 },
        };

        var m1 = new EnemyManager();
        m1.SpawnEnemies(rooms, map, 42);

        var m2 = new EnemyManager();
        m2.SpawnEnemies(rooms, map, 42);

        Assert.Equal(m1.Count, m2.Count);
    }

    [Fact]
    public void Clear_RemovesAllEnemies()
    {
        var manager = new EnemyManager();
        var map = new TileMap(30, 30);
        map.Fill(Tile.Floor);

        var rooms = new List<Room>
        {
            new() { X = 2, Y = 2, Width = 6, Height = 6 },
            new() { X = 10, Y = 2, Width = 6, Height = 6 },
        };

        manager.SpawnEnemies(rooms, map, 42);
        manager.UnloadContent();

        Assert.Equal(0, manager.Count);
    }
}
