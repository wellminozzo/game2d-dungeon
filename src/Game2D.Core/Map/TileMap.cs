using Microsoft.Xna.Framework;

namespace Game2D.Core;

public class TileMap
{
    private readonly Tile[,] _tiles;
    public int Width { get; }
    public int Height { get; }
    public int TileSize { get; } = 32;
    public int PixelWidth => Width * TileSize;
    public int PixelHeight => Height * TileSize;

    public TileMap(int width, int height)
    {
        Width = width;
        Height = height;
        _tiles = new Tile[width, height];
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return Tile.Wall;
        return _tiles[x, y];
    }

    public void SetTile(int x, int y, Tile tile)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            _tiles[x, y] = tile;
    }

    public bool IsWalkable(int x, int y)
    {
        return GetTile(x, y).IsWalkable;
    }

    public Point WorldToTile(Vector2 worldPos)
    {
        return new Point((int)(worldPos.X / TileSize), (int)(worldPos.Y / TileSize));
    }

    public Vector2 TileToWorld(int tileX, int tileY)
    {
        return new Vector2(tileX * TileSize + TileSize / 2f, tileY * TileSize + TileSize / 2f);
    }

    public Rectangle GetTileRect(int x, int y)
    {
        return new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
    }

    public void Fill(Tile tile)
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            _tiles[x, y] = tile;
    }

    public void DrawRoom(int x, int y, int roomW, int roomH)
    {
        for (var tx = x; tx < x + roomW; tx++)
        for (var ty = y; ty < y + roomH; ty++)
            SetTile(tx, ty, Tile.Floor);

        for (var tx = x; tx < x + roomW; tx++)
        {
            SetTile(tx, y, Tile.Wall);
            SetTile(tx, y + roomH - 1, Tile.Wall);
        }

        for (var ty = y; ty < y + roomH; ty++)
        {
            SetTile(x, ty, Tile.Wall);
            SetTile(x + roomW - 1, ty, Tile.Wall);
        }
    }
}
