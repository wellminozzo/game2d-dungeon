using Game2D.Core;

namespace Game2D.Tests;

public class EnemyTests
{
    [Fact]
    public void Constructor_StartsWithPatrolState()
    {
        var map = new TileMap(20, 20);
        map.Fill(Tile.Floor);
        var enemy = new Enemy(new Microsoft.Xna.Framework.Vector2(100, 100), map);

        Assert.Equal(EnemyState.Patrol, enemy.State);
    }

    [Fact]
    public void UpdateAI_PlayerInRange_ChangesToChase()
    {
        var map = new TileMap(20, 20);
        map.Fill(Tile.Floor);
        var enemy = new Enemy(
            new Microsoft.Xna.Framework.Vector2(100, 100),
            map,
            detectionRadius: 150f);

        var gameTime = new Microsoft.Xna.Framework.GameTime();
        enemy.UpdateAI(gameTime, new Microsoft.Xna.Framework.Vector2(100, 50));

        Assert.Equal(EnemyState.Chase, enemy.State);
    }

    [Fact]
    public void UpdateAI_PlayerFarAway_StaysInPatrol()
    {
        var map = new TileMap(20, 20);
        map.Fill(Tile.Floor);
        var enemy = new Enemy(
            new Microsoft.Xna.Framework.Vector2(100, 100),
            map,
            detectionRadius: 50f);

        var gameTime = new Microsoft.Xna.Framework.GameTime();
        enemy.UpdateAI(gameTime, new Microsoft.Xna.Framework.Vector2(500, 500));

        Assert.Equal(EnemyState.Patrol, enemy.State);
    }

    [Fact]
    public void UpdateAI_PlayerLeavesRange_ReturnsToPatrol()
    {
        var map = new TileMap(20, 20);
        map.Fill(Tile.Floor);
        var enemy = new Enemy(
            new Microsoft.Xna.Framework.Vector2(100, 100),
            map,
            detectionRadius: 80f);

        var gameTime = new Microsoft.Xna.Framework.GameTime();
        enemy.UpdateAI(gameTime, new Microsoft.Xna.Framework.Vector2(100, 50));
        Assert.Equal(EnemyState.Chase, enemy.State);

        enemy.UpdateAI(gameTime, new Microsoft.Xna.Framework.Vector2(500, 500));
        Assert.Equal(EnemyState.Patrol, enemy.State);
    }

    [Fact]
    public void UpdateAI_ChaseMode_MovesTowardPlayer()
    {
        var map = new TileMap(20, 20);
        map.Fill(Tile.Floor);
        var startPos = new Microsoft.Xna.Framework.Vector2(200, 200);
        var enemy = new Enemy(startPos, map, detectionRadius: 300f, speed: 100f);

        var playerPos = new Microsoft.Xna.Framework.Vector2(300, 200);
        var gameTime = new Microsoft.Xna.Framework.GameTime(
            TimeSpan.Zero,
            TimeSpan.FromSeconds(0.1));

        enemy.UpdateAI(gameTime, playerPos);

        Assert.True(enemy.Position.X > startPos.X);
    }
}
