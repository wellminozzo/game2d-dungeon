using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Core;

public class SpriteLibrary
{
    private readonly ContentManager _content;
    private readonly GraphicsDevice _graphics;
    private bool _usePipeline;

    public bool IsUsingPipeline => _usePipeline;

    public SpriteLibrary(ContentManager content, GraphicsDevice graphics)
    {
        _content = content;
        _graphics = graphics;
    }

    public Texture2D LoadPlayerSprite()
    {
        var tex = TryLoad("Sprites/player/hero_walk");
        if (tex != null) { _usePipeline = true; return tex; }
        return SpriteGenerator.CreatePlayerTexture(_graphics);
    }

    public Texture2D LoadEnemySprite()
    {
        var tex = TryLoad("Sprites/enemies/slime_walk");
        if (tex != null) { _usePipeline = true; return tex; }
        return SpriteGenerator.CreateEnemyTexture(_graphics);
    }

    public Texture2D LoadTileset()
    {
        var tex = TryLoad("Sprites/dungeon_tiles");
        if (tex != null) { _usePipeline = true; return tex; }
        return SpriteGenerator.CreateTileset(_graphics);
    }

    private Texture2D? TryLoad(string assetName)
    {
        try
        {
            return _content.Load<Texture2D>(assetName);
        }
        catch
        {
            return null;
        }
    }

    public static AnimatedSprite CreatePlayerAnim(Texture2D texture)
    {
        var anim = new AnimatedSprite(texture);
        var fw = 24;
        var fh = 24;

        foreach (var dir in new[] { "down", "up", "left", "right" })
        {
            anim.AddAnimation($"idle_{dir}", new Animation($"idle_{dir}", new[]
            {
                new AnimationFrame(new Rectangle(0, 0, fw, fh), 0.5f),
            }));

            anim.AddAnimation($"walk_{dir}", new Animation($"walk_{dir}", new[]
            {
                new AnimationFrame(new Rectangle(0, 0, fw, fh), 0.15f),
                new AnimationFrame(new Rectangle(fw, 0, fw, fh), 0.15f),
            }));
        }

        return anim;
    }

    public static AnimatedSprite CreateEnemyAnim(Texture2D texture)
    {
        var anim = new AnimatedSprite(texture);
        anim.AddAnimation("walk", new Animation("walk", new[]
        {
            new AnimationFrame(new Rectangle(0, 0, 22, 22), 0.25f),
            new AnimationFrame(new Rectangle(22, 0, 22, 22), 0.25f),
        }));
        return anim;
    }
}
