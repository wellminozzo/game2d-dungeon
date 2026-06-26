using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game2D.Core;

public class InputManager
{
    private KeyboardState _currentKbd;
    private KeyboardState _previousKbd;

    public Vector2 Direction
    {
        get
        {
            var dir = Vector2.Zero;
            if (_currentKbd.IsKeyDown(Keys.W) || _currentKbd.IsKeyDown(Keys.Up))    dir.Y -= 1;
            if (_currentKbd.IsKeyDown(Keys.S) || _currentKbd.IsKeyDown(Keys.Down))  dir.Y += 1;
            if (_currentKbd.IsKeyDown(Keys.A) || _currentKbd.IsKeyDown(Keys.Left))  dir.X -= 1;
            if (_currentKbd.IsKeyDown(Keys.D) || _currentKbd.IsKeyDown(Keys.Right)) dir.X += 1;

            if (dir != Vector2.Zero)
                dir.Normalize();

            return dir;
        }
    }

    public void Update()
    {
        _previousKbd = _currentKbd;
        _currentKbd = Keyboard.GetState();
    }

    public bool IsDown(Keys key) => _currentKbd.IsKeyDown(key);

    public bool IsPressed(Keys key) => _currentKbd.IsKeyDown(key) && _previousKbd.IsKeyUp(key);

    public bool IsReleased(Keys key) => _currentKbd.IsKeyUp(key) && _previousKbd.IsKeyDown(key);
}
