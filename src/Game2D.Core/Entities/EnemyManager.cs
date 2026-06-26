using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Core;

public class EnemyManager
{
    private readonly List<Enemy> _enemies = new();
    private Texture2D? _enemyTexture;
    private Random _rng = new();

    public IReadOnlyList<Enemy> Enemies => _enemies;
    public int Count => _enemies.Count;

    public void LoadContent(GraphicsDevice graphicsDevice, SpriteLibrary spriteLibrary)
    {
        _enemyTexture = spriteLibrary.LoadEnemySprite();
    }

    private AnimatedSprite CreateEnemyAnim()
    {
        var anim = SpriteLibrary.CreateEnemyAnim(_enemyTexture!);
        return anim;
    }

    public void SpawnEnemies(IReadOnlyList<Room> rooms, TileMap tileMap, int seed)
    {
        _enemies.Clear();
        _rng = new Random(seed);

        for (var i = 1; i < rooms.Count; i++)
        {
            var room = rooms[i];
            var count = _rng.Next(1, 4);

            for (var e = 0; e < count; e++)
            {
                var ex = _rng.Next(room.X + 1, room.X + room.Width - 1);
                var ey = _rng.Next(room.Y + 1, room.Y + room.Height - 1);

                if (!tileMap.IsWalkable(ex, ey))
                    continue;

                var pos = tileMap.TileToWorld(ex, ey);
                var enemy = new Enemy(pos, tileMap, _rng, speed: 50f + _rng.NextSingle() * 30f);

                if (_enemyTexture != null)
                    enemy.SetAnimatedSprite(CreateEnemyAnim());

                _enemies.Add(enemy);
            }
        }
    }

    public void Update(GameTime gameTime, Vector2 playerPos)
    {
        foreach (var enemy in _enemies)
        {
            enemy.Update(gameTime);
            enemy.UpdateAI(gameTime, playerPos);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var enemy in _enemies)
            enemy.Draw(spriteBatch);
    }

    public void UnloadContent()
    {
        _enemyTexture = null;
        _enemies.Clear();
    }

    public Enemy? CheckPlayerCollision(Rectangle playerBounds)
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.Bounds.Intersects(playerBounds))
                return enemy;
        }

        return null;
    }
}
