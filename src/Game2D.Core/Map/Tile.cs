namespace Game2D.Core;

public enum TileType
{
    Void = 0,
    Floor,
    Wall,
    Door,
    Water,
}

public struct Tile
{
    public TileType Type;
    public bool IsWalkable;
    public int TextureIndex;

    public static Tile Create(TileType type, bool walkable, int textureIndex = 0)
    {
        return new Tile
        {
            Type = type,
            IsWalkable = walkable,
            TextureIndex = textureIndex,
        };
    }

    public static readonly Tile Void = new() { Type = TileType.Void, IsWalkable = false, TextureIndex = 0 };
    public static readonly Tile Floor = new() { Type = TileType.Floor, IsWalkable = true, TextureIndex = 1 };
    public static readonly Tile Wall = new() { Type = TileType.Wall, IsWalkable = false, TextureIndex = 2 };
}
