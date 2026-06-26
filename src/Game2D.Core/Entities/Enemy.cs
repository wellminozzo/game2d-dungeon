using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Core;

public enum EnemyState
{
    Patrol,
    Chase,
}

public class Enemy : Entity
{
    private readonly TileMap _tileMap;
    private readonly float _speed;
    private readonly float _detectionRadius;
    private Vector2 _patrolTarget;
    private float _patrolWaitTimer;
    private readonly Random _rng;
    private AnimatedSprite _anim = null!;

    public EnemyState State { get; private set; }
    public int Damage { get; set; } = 1;
    public float AttackCooldown { get; set; } = 0.5f;
    public float AttackTimer { get; set; }

    public Enemy(Vector2 position, TileMap tileMap, Random? rng = null, float speed = 60f, float detectionRadius = 120f)
        : base(position, new Vector2(22, 22))
    {
        _tileMap = tileMap;
        _speed = speed;
        _detectionRadius = detectionRadius;
        _rng = rng ?? new Random();
        _patrolTarget = position;
        State = EnemyState.Patrol;
    }

    public void SetAnimatedSprite(AnimatedSprite anim)
    {
        _anim = anim;
        _anim.Play("walk");
        _anim.Origin = new Vector2(11, 11);
    }

    public override void Update(GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (AttackTimer > 0)
            AttackTimer -= dt;

        _anim?.Update(gameTime);
    }

    public void UpdateAI(GameTime gameTime, Vector2 playerPos)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var distToPlayer = Vector2.Distance(Position, playerPos);

        if (distToPlayer < _detectionRadius)
        {
            State = EnemyState.Chase;
        }
        else if (State == EnemyState.Chase && distToPlayer > _detectionRadius * 1.5f)
        {
            State = EnemyState.Patrol;
            PickNewPatrolTarget();
        }

        if (_anim != null)
            _anim.Tint = State == EnemyState.Chase
                ? new Color(220, 100, 80)
                : new Color(180, 80, 60);

        switch (State)
        {
            case EnemyState.Patrol:
                UpdatePatrol(dt);
                break;
            case EnemyState.Chase:
                UpdateChase(dt, playerPos);
                break;
        }
    }

    private void UpdatePatrol(float dt)
    {
        if (_patrolWaitTimer > 0)
        {
            _patrolWaitTimer -= dt;
            return;
        }

        var dir = _patrolTarget - Position;
        if (dir.LengthSquared() < 16f)
        {
            _patrolWaitTimer = _rng.NextSingle() * 2f + 0.5f;
            PickNewPatrolTarget();
            return;
        }

        dir.Normalize();
        TryMove(dir * _speed * dt);
    }

    private void UpdateChase(float dt, Vector2 playerPos)
    {
        var dir = playerPos - Position;
        if (dir.LengthSquared() < 400f)
            return;

        dir.Normalize();
        TryMove(dir * _speed * dt);
    }

    private void TryMove(Vector2 movement)
    {
        var ts = _tileMap.TileSize;

        var newX = Position.X + movement.X;
        var rectX = new Rectangle(
            (int)(newX - Size.X / 2f),
            (int)(Position.Y - Size.Y / 2f),
            (int)Size.X,
            (int)Size.Y);

        if (!CollidesWithUnwalkable(rectX, ts))
            Position = new Vector2(newX, Position.Y);

        var newY = Position.Y + movement.Y;
        var rectY = new Rectangle(
            (int)(Position.X - Size.X / 2f),
            (int)(newY - Size.Y / 2f),
            (int)Size.X,
            (int)Size.Y);

        if (!CollidesWithUnwalkable(rectY, ts))
            Position = new Vector2(Position.X, newY);

        var halfSize = Size / 2f;
        Position = new Vector2(
            MathHelper.Clamp(Position.X, halfSize.X, _tileMap.PixelWidth - halfSize.X),
            MathHelper.Clamp(Position.Y, halfSize.Y, _tileMap.PixelHeight - halfSize.Y));
    }

    private bool CollidesWithUnwalkable(Rectangle rect, int tileSize)
    {
        var minTile = new Point(rect.Left / tileSize, rect.Top / tileSize);
        var maxTile = new Point((rect.Right - 1) / tileSize, (rect.Bottom - 1) / tileSize);

        for (var x = minTile.X; x <= maxTile.X; x++)
        for (var y = minTile.Y; y <= maxTile.Y; y++)
        {
            if (!_tileMap.IsWalkable(x, y))
                return true;
        }

        return false;
    }

    private void PickNewPatrolTarget()
    {
        var ts = _tileMap.TileSize;
        for (var i = 0; i < 20; i++)
        {
            var tx = (int)(Position.X / ts) + _rng.Next(-5, 6);
            var ty = (int)(Position.Y / ts) + _rng.Next(-5, 6);

            if (_tileMap.IsWalkable(tx, ty))
            {
                _patrolTarget = new Vector2(tx * ts + ts / 2f, ty * ts + ts / 2f);
                return;
            }
        }

        _patrolTarget = Position;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _anim?.Draw(spriteBatch, Position);
    }
}
