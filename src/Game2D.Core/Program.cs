if (args.Length > 0 && args[0] == "--export-sprites")
{
    var contentDir = Path.GetFullPath(Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Content", "Sprites"));

    Game2D.Core.SpriteExporter.ExportAll(contentDir);
    Console.WriteLine($"Sprites exported to {contentDir}");
    return;
}

using var game = new Game2D.Core.Game1();
game.Run();
