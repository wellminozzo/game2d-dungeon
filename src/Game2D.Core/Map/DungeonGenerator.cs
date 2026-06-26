using Microsoft.Xna.Framework;

namespace Game2D.Core;

public class DungeonGenerator
{
    public int MapWidth { get; set; } = 60;
    public int MapHeight { get; set; } = 40;
    public int MinLeafSize { get; set; } = 8;
    public int MinRoomSize { get; set; } = 4;
    public int MaxRoomSize { get; set; } = 10;
    public int MaxDepth { get; set; } = 5;
    public int Seed { get; set; }

    public DungeonResult Generate()
    {
        var actualSeed = Seed != 0 ? Seed : Environment.TickCount;
        var rng = new Random(actualSeed);
        var map = new TileMap(MapWidth, MapHeight);
        map.Fill(Tile.Wall);

        var root = new BspNode(new Rectangle(1, 1, MapWidth - 2, MapHeight - 2));

        SplitNode(root, 0, rng);

        root.CreateRooms(MinRoomSize, MaxRoomSize, rng);

        root.ConnectRooms(map, rng);

        var leaves = new List<BspNode>();
        root.GetLeaves(leaves);

        var rooms = new List<Room>();
        foreach (var leaf in leaves)
        {
            if (leaf.Room == null) continue;
            var room = leaf.Room.Value;
            CarveRoom(map, room);
            rooms.Add(room);
        }

        var spawnRoom = rooms.Count > 0 ? rooms[0] : new Room { X = 5, Y = 5, Width = 6, Height = 6 };
        if (!map.IsWalkable(spawnRoom.CenterX, spawnRoom.CenterY))
            CarveRoom(map, spawnRoom);

        var spawnPos = new Vector2(
            spawnRoom.CenterX * map.TileSize + map.TileSize / 2f,
            spawnRoom.CenterY * map.TileSize + map.TileSize / 2f);

        return new DungeonResult
        {
            TileMap = map,
            Rooms = rooms,
            SpawnPosition = spawnPos,
            RootNode = root,
            UsedSeed = actualSeed,
        };
    }

    private void SplitNode(BspNode node, int depth, Random rng)
    {
        if (depth >= MaxDepth)
            return;

        if (!node.Split(MinLeafSize, rng))
            return;

        SplitNode(node.Left!, depth + 1, rng);
        SplitNode(node.Right!, depth + 1, rng);
    }

    private static void CarveRoom(TileMap map, Room room)
    {
        for (var x = room.X; x < room.X + room.Width; x++)
        for (var y = room.Y; y < room.Y + room.Height; y++)
            map.SetTile(x, y, Tile.Floor);
    }
}

public class DungeonResult
{
    public TileMap TileMap { get; set; } = null!;
    public List<Room> Rooms { get; set; } = new();
    public Vector2 SpawnPosition { get; set; }
    public BspNode RootNode { get; set; } = null!;
    public int UsedSeed { get; set; }
}
