using Game2D.Core;

namespace Game2D.Tests;

public class DungeonGeneratorTests
{
    [Fact]
    public void Generate_ReturnsNonEmptyMap()
    {
        var gen = new DungeonGenerator
        {
            MapWidth = 60,
            MapHeight = 40,
            Seed = 42,
        };

        var result = gen.Generate();

        Assert.NotNull(result.TileMap);
        Assert.Equal(60, result.TileMap.Width);
        Assert.Equal(40, result.TileMap.Height);
    }

    [Fact]
    public void Generate_CreatesAtLeastOneRoom()
    {
        var gen = new DungeonGenerator
        {
            Seed = 42,
        };

        var result = gen.Generate();

        Assert.NotEmpty(result.Rooms);
    }

    [Fact]
    public void Generate_SpawnPosition_IsWalkable()
    {
        var gen = new DungeonGenerator
        {
            Seed = 42,
        };

        var result = gen.Generate();
        var tilePos = result.TileMap.WorldToTile(result.SpawnPosition);

        Assert.True(result.TileMap.IsWalkable(tilePos.X, tilePos.Y));
    }

    [Fact]
    public void Generate_SameSeed_ProducesIdenticalMap()
    {
        var gen1 = new DungeonGenerator { Seed = 123 };
        var gen2 = new DungeonGenerator { Seed = 123 };

        var result1 = gen1.Generate();
        var result2 = gen2.Generate();

        Assert.Equal(result1.Rooms.Count, result2.Rooms.Count);

        for (var i = 0; i < result1.Rooms.Count; i++)
        {
            Assert.Equal(result1.Rooms[i].X, result2.Rooms[i].X);
            Assert.Equal(result1.Rooms[i].Y, result2.Rooms[i].Y);
            Assert.Equal(result1.Rooms[i].Width, result2.Rooms[i].Width);
            Assert.Equal(result1.Rooms[i].Height, result2.Rooms[i].Height);
        }
    }

    [Fact]
    public void Generate_DifferentSeeds_ProduceDifferentMaps()
    {
        var gen1 = new DungeonGenerator { Seed = 1 };
        var gen2 = new DungeonGenerator { Seed = 999 };

        var result1 = gen1.Generate();
        var result2 = gen2.Generate();

        var same = true;
        for (var i = 0; i < Math.Min(result1.Rooms.Count, result2.Rooms.Count); i++)
        {
            if (result1.Rooms[i].X != result2.Rooms[i].X ||
                result1.Rooms[i].Y != result2.Rooms[i].Y)
            {
                same = false;
                break;
            }
        }

        Assert.False(same);
    }

    [Fact]
    public void Generate_AllRooms_AreWithinMapBounds()
    {
        var gen = new DungeonGenerator
        {
            MapWidth = 40,
            MapHeight = 30,
            Seed = 42,
        };

        var result = gen.Generate();

        foreach (var room in result.Rooms)
        {
            Assert.InRange(room.X, 1, gen.MapWidth - 2);
            Assert.InRange(room.Y, 1, gen.MapHeight - 2);
            Assert.InRange(room.X + room.Width, 1, gen.MapWidth - 1);
            Assert.InRange(room.Y + room.Height, 1, gen.MapHeight - 1);
        }
    }

    [Fact]
    public void Generate_RoomSizes_WithinConfiguredBounds()
    {
        var gen = new DungeonGenerator
        {
            MinRoomSize = 3,
            MaxRoomSize = 8,
            Seed = 42,
        };

        var result = gen.Generate();

        foreach (var room in result.Rooms)
        {
            Assert.InRange(room.Width, 3, 8);
            Assert.InRange(room.Height, 3, 8);
        }
    }

    [Fact]
    public void Generate_RoomsHaveWalkableTiles()
    {
        var gen = new DungeonGenerator
        {
            Seed = 42,
        };

        var result = gen.Generate();

        foreach (var room in result.Rooms)
        {
            Assert.True(result.TileMap.IsWalkable(room.CenterX, room.CenterY));
        }
    }

    [Fact]
    public void BspNode_Split_CreatesChildren()
    {
        var node = new BspNode(new Microsoft.Xna.Framework.Rectangle(0, 0, 40, 40));
        var rng = new Random(42);

        var result = node.Split(8, rng);

        Assert.True(result);
        Assert.NotNull(node.Left);
        Assert.NotNull(node.Right);
    }

    [Fact]
    public void BspNode_Split_SmallBounds_ReturnsFalse()
    {
        var node = new BspNode(new Microsoft.Xna.Framework.Rectangle(0, 0, 5, 5));
        var rng = new Random(42);

        var result = node.Split(8, rng);

        Assert.False(result);
        Assert.True(node.IsLeaf);
    }
}
