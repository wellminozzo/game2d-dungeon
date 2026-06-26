using Game2D.Library;

var solutionDir = Path.GetFullPath(Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", ".."));

var contentDir = Path.Combine(solutionDir, "src", "Game2D.Core", "Content", "Sprites");

Directory.CreateDirectory(Path.Combine(contentDir, "player"));
Directory.CreateDirectory(Path.Combine(contentDir, "enemies"));

WritePng(Path.Combine(contentDir, "player", "hero_walk.png"), 48, 24, PngWriter.CreatePlayerPixels());
WritePng(Path.Combine(contentDir, "enemies", "slime_walk.png"), 44, 22, PngWriter.CreateEnemyPixels());
WritePng(Path.Combine(contentDir, "dungeon_tiles.png"), 96, 32, PngWriter.CreateTilesetPixels());

Console.WriteLine($"Sprites generated in: {contentDir}");
Console.WriteLine("Run 'dotnet build' to process them through the Content Pipeline.");

static void WritePng(string path, int w, int h, byte[] pixels)
{
    if (File.Exists(path))
    {
        Console.WriteLine($"  Skipped (exists): {Path.GetFileName(path)}");
        return;
    }

    PngWriter.Write(path, w, h, pixels);
    Console.WriteLine($"  Generated: {Path.GetFileName(path)}");
}
