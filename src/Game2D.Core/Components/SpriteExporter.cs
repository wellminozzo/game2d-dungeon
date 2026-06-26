using Game2D.Library;

namespace Game2D.Core;

public static class SpriteExporter
{
    public static void ExportAll(string? targetDir = null)
    {
        var spriteDir = targetDir ?? FindContentSpriteDir();

        var dirs = new[]
        {
            Path.Combine(spriteDir, "player"),
            Path.Combine(spriteDir, "enemies"),
        };

        foreach (var d in dirs)
            Directory.CreateDirectory(d);

        Export(spriteDir, "player", "hero_walk.png", 48, 24, PngWriter.CreatePlayerPixels());
        Export(spriteDir, "enemies", "slime_walk.png", 44, 22, PngWriter.CreateEnemyPixels());
        Export(spriteDir, null, "dungeon_tiles.png", 96, 32, PngWriter.CreateTilesetPixels());
    }

    private static void Export(string spriteDir, string? subdir, string filename, int w, int h, byte[] pixels)
    {
        var path = subdir != null
            ? Path.Combine(spriteDir, subdir, filename)
            : Path.Combine(spriteDir, filename);

        if (File.Exists(path))
            return;

        PngWriter.Write(path, w, h, pixels);
        Console.WriteLine($"Generated: {path}");
    }

    private static string FindContentSpriteDir()
    {
        var dir = AppDomain.CurrentDomain.BaseDirectory;
        for (var i = 0; i < 6; i++)
        {
            var candidate = Path.Combine(dir, "Content", "Sprites");
            if (Directory.Exists(candidate) || File.Exists(Path.Combine(dir, "Game2D.Core.csproj")))
                return candidate;

            var parent = Directory.GetParent(dir);
            if (parent == null) break;
            dir = parent.FullName;
        }

        return Path.Combine(dir, "Content", "Sprites");
    }
}
