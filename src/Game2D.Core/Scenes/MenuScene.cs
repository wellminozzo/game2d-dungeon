using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2D.Core;

public class MenuScene : IScene
{
    private SpriteFont? _font;
    private string _statusText = "Press ENTER to start";
    private KeyboardState _previousKbd;

    public Action<string>? OnSceneRequested { get; set; }

    public void Initialize()
    {
        _previousKbd = new KeyboardState();
    }

    public void LoadContent(ContentManager content)
    {
        _font = content.Load<SpriteFont>("Fonts/Default");
    }

    public void UnloadContent()
    {
        _font = null;
    }

    public void Update(GameTime gameTime)
    {
        var kbd = Keyboard.GetState();

        if (kbd.IsKeyDown(Keys.Enter) && _previousKbd.IsKeyUp(Keys.Enter))
            OnSceneRequested?.Invoke("gameplay");

        _previousKbd = kbd;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_font == null) return;

        var viewport = spriteBatch.GraphicsDevice.Viewport;
        var text = "DUNGEON EXPLORER";
        var titleSize = _font.MeasureString(text);
        var titlePos = new Vector2(
            (viewport.Width - titleSize.X) / 2,
            viewport.Height / 3f);

        var statusSize = _font.MeasureString(_statusText);
        var statusPos = new Vector2(
            (viewport.Width - statusSize.X) / 2,
            viewport.Height / 2f);

        spriteBatch.Begin();
        spriteBatch.DrawString(_font, text, titlePos, Color.White);
        spriteBatch.DrawString(_font, _statusText, statusPos, Color.Gray);
        spriteBatch.End();
    }
}
