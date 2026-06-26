using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2D.Core;

public class GameplayScene : IScene
{
    private InputManager _input = null!;
    private TileMap _tileMap = null!;
    private Camera2D _camera = null!;
    private MapRenderer _mapRenderer = null!;
    private Player _player = null!;
    private EnemyManager _enemyManager = null!;
    private SpriteLibrary _spriteLibrary = null!;
    private SpriteFont? _font;
    private GraphicsDevice? _graphicsDevice;
    private string _debugText = "";
    private KeyboardState _previousKbd;
    private int _dungeonSeed;

    public Action<string>? OnSceneRequested { get; set; }

    public void Initialize()
    {
        _input = new InputManager();
        _camera = new Camera2D();
        _mapRenderer = new MapRenderer();
        _enemyManager = new EnemyManager();

        GenerateDungeon();
        _previousKbd = new KeyboardState();
    }

    private void GenerateDungeon()
    {
        _dungeonSeed = Environment.TickCount;
        var gen = new DungeonGenerator
        {
            MapWidth = 60,
            MapHeight = 40,
            MinLeafSize = 8,
            MinRoomSize = 4,
            MaxRoomSize = 10,
            MaxDepth = 5,
            Seed = _dungeonSeed,
        };

        var result = gen.Generate();
        _tileMap = result.TileMap;

        _player = new Player(result.SpawnPosition, _input, _tileMap);

        if (_spriteLibrary != null)
            SetupPlayerSprite();

        _enemyManager.SpawnEnemies(result.Rooms, _tileMap, result.UsedSeed);
    }

    private void SetupPlayerSprite()
    {
        var texture = _spriteLibrary.LoadPlayerSprite();
        var anim = SpriteLibrary.CreatePlayerAnim(texture);
        _player.SetAnimatedSprite(anim);
    }

    public void LoadContent(ContentManager content)
    {
        _graphicsDevice = ((IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService))!).GraphicsDevice;

        _spriteLibrary = new SpriteLibrary(content, _graphicsDevice);
        _font = content.Load<SpriteFont>("Fonts/Default");
        _mapRenderer.LoadContent(_graphicsDevice, _spriteLibrary);
        _enemyManager.LoadContent(_graphicsDevice, _spriteLibrary);

        SetupPlayerSprite();
    }

    public void UnloadContent()
    {
        _font = null;
        _mapRenderer.UnloadContent();
        _enemyManager.UnloadContent();
    }

    public void Update(GameTime gameTime)
    {
        var kbd = Keyboard.GetState();

        if (kbd.IsKeyDown(Keys.Escape) && _previousKbd.IsKeyUp(Keys.Escape))
            OnSceneRequested?.Invoke("menu");

        if (kbd.IsKeyDown(Keys.R) && _previousKbd.IsKeyUp(Keys.R))
            GenerateDungeon();

        _input.Update();
        _player.Update(gameTime);
        _enemyManager.Update(gameTime, _player.Position);

        if (_graphicsDevice != null)
        {
            _camera.ViewportWidth = _graphicsDevice.Viewport.Width;
            _camera.ViewportHeight = _graphicsDevice.Viewport.Height;
        }

        _camera.MapPixelWidth = _tileMap.PixelWidth;
        _camera.MapPixelHeight = _tileMap.PixelHeight;
        _camera.Follow(_player.Position, 0.08f);

        var tilePos = _tileMap.WorldToTile(_player.Position);
        var enemyStatus = _enemyManager.Count > 0 ? "active" : "cleared";
        _debugText = $"Tile: {tilePos.X},{tilePos.Y}  Enemies: {_enemyManager.Count} ({enemyStatus}) | Press R: new map | ESC: menu";

        _previousKbd = kbd;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_graphicsDevice == null || _font == null) return;

        _graphicsDevice.Clear(new Color(15, 15, 20));

        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            null, null, null, null, null,
            _camera.TransformMatrix);

        _mapRenderer.Draw(spriteBatch, _tileMap, _camera);
        _enemyManager.Draw(spriteBatch);
        _player.Draw(spriteBatch);

        spriteBatch.End();

        spriteBatch.Begin();
        spriteBatch.DrawString(_font, _debugText, new Vector2(8, 8), Color.LightGray);

        var spriteMode = _spriteLibrary.IsUsingPipeline ? "Content Pipeline" : "Generated (code)";
        var hint = _spriteLibrary.IsUsingPipeline ? "Replace PNGs in Content/Sprites/" : "Run tools/GenerateSprites then uncomment Content.mgcb";
        spriteBatch.DrawString(_font, $"Sprites: {spriteMode}", new Vector2(8, 26), new Color(100, 100, 100));
        spriteBatch.End();
    }
}
