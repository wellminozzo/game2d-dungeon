using Microsoft.Xna.Framework;

namespace Game2D.Core;

public class BspNode
{
    public Rectangle Bounds { get; }
    public BspNode? Left { get; private set; }
    public BspNode? Right { get; private set; }
    public Room? Room { get; set; }

    public BspNode(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public bool IsLeaf => Left == null && Right == null;

    public bool Split(int minSize, Random rng)
    {
        if (IsLeaf == false)
            return false;

        var splitH = Bounds.Width >= Bounds.Height && Bounds.Width > minSize * 2;
        var splitV = Bounds.Height >= Bounds.Width && Bounds.Height > minSize * 2;

        if (!splitH && !splitV)
            return false;

        if (splitH && splitV)
        {
            if (rng.Next(2) == 0)
                splitH = true;
            else
                splitV = true;
        }

        if (splitH)
        {
            var max = Bounds.Width - minSize;
            var split = rng.Next(minSize, max);

            Left = new BspNode(new Rectangle(Bounds.X, Bounds.Y, split, Bounds.Height));
            Right = new BspNode(new Rectangle(Bounds.X + split, Bounds.Y, Bounds.Width - split, Bounds.Height));
        }
        else
        {
            var max = Bounds.Height - minSize;
            var split = rng.Next(minSize, max);

            Left = new BspNode(new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, split));
            Right = new BspNode(new Rectangle(Bounds.X, Bounds.Y + split, Bounds.Width, Bounds.Height - split));
        }

        return true;
    }

    public void CreateRooms(int minRoomSize, int maxRoomSize, Random rng)
    {
        if (IsLeaf)
        {
            var roomW = rng.Next(minRoomSize, Math.Min(maxRoomSize, Bounds.Width - 2) + 1);
            var roomH = rng.Next(minRoomSize, Math.Min(maxRoomSize, Bounds.Height - 2) + 1);
            var roomX = Bounds.X + rng.Next(1, Bounds.Width - roomW);
            var roomY = Bounds.Y + rng.Next(1, Bounds.Height - roomH);

            Room = new Room
            {
                X = roomX,
                Y = roomY,
                Width = roomW,
                Height = roomH,
            };
        }
        else
        {
            Left?.CreateRooms(minRoomSize, maxRoomSize, rng);
            Right?.CreateRooms(minRoomSize, maxRoomSize, rng);
        }
    }

    public void GetLeaves(List<BspNode> leaves)
    {
        if (IsLeaf)
        {
            leaves.Add(this);
        }
        else
        {
            Left?.GetLeaves(leaves);
            Right?.GetLeaves(leaves);
        }
    }

    public void ConnectRooms(TileMap map, Random rng)
    {
        if (IsLeaf || Left == null || Right == null)
            return;

        Left.ConnectRooms(map, rng);
        Right.ConnectRooms(map, rng);

        var leftRoom = GetRoom(Left);
        var rightRoom = GetRoom(Right);

        if (leftRoom == null || rightRoom == null)
            return;

        var lr = leftRoom.Value;
        var rr = rightRoom.Value;

        var x1 = lr.CenterX;
        var y1 = lr.CenterY;
        var x2 = rr.CenterX;
        var y2 = rr.CenterY;

        if (rng.Next(2) == 0)
        {
            CarveHCorridor(map, x1, x2, y1);
            CarveVCorridor(map, y1, y2, x2);
        }
        else
        {
            CarveVCorridor(map, y1, y2, x1);
            CarveHCorridor(map, x1, x2, y2);
        }
    }

    private static Room? GetRoom(BspNode node)
    {
        if (node.Room != null)
            return node.Room;

        var left = node.Left != null ? GetRoom(node.Left) : null;
        if (left != null) return left;

        return node.Right != null ? GetRoom(node.Right) : null;
    }

    private static void CarveHCorridor(TileMap map, int x1, int x2, int y)
    {
        var start = Math.Min(x1, x2);
        var end = Math.Max(x1, x2);
        for (var x = start; x <= end; x++)
            if (y >= 0 && y < map.Height && x >= 0 && x < map.Width)
                map.SetTile(x, y, Tile.Floor);
    }

    private static void CarveVCorridor(TileMap map, int y1, int y2, int x)
    {
        var start = Math.Min(y1, y2);
        var end = Math.Max(y1, y2);
        for (var y = start; y <= end; y++)
            if (x >= 0 && x < map.Width && y >= 0 && y < map.Height)
                map.SetTile(x, y, Tile.Floor);
    }
}
