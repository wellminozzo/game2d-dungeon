using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Core;

public class MapRenderer
{
    private Texture2D? _tileset;
    private readonly Rectangle[] _sourceRects;

    public MapRenderer()
    {
        _sourceRects = new Rectangle[]
        {
            new(0, 0, 32, 32),     // Void
            new(32, 0, 32, 32),    // Floor
            new(64, 0, 32, 32),    // Wall
        };
    }

    public void LoadContent(GraphicsDevice graphicsDevice, SpriteLibrary spriteLibrary)
    {
        _tileset = spriteLibrary.LoadTileset();
    }

    public void Draw(SpriteBatch spriteBatch, TileMap tileMap, Camera2D camera)
    {
        if (_tileset == null) return;

        var visible = camera.VisibleArea;
        var ts = tileMap.TileSize;

        var startX = Math.Max(0, visible.Left / ts);
        var startY = Math.Max(0, visible.Top / ts);
        var endX = Math.Min(tileMap.Width - 1, visible.Right / ts);
        var endY = Math.Min(tileMap.Height - 1, visible.Bottom / ts);

        for (var x = startX; x <= endX; x++)
        for (var y = startY; y <= endY; y++)
        {
            var tile = tileMap.GetTile(x, y);
            var idx = (int)tile.Type;

            if (idx >= _sourceRects.Length || idx == 0)
                continue;

            var dest = tileMap.GetTileRect(x, y);
            var src = _sourceRects[idx];
            spriteBatch.Draw(_tileset, dest, src, Color.White);
        }
    }

    public void UnloadContent()
    {
        _tileset = null;
    }
}
