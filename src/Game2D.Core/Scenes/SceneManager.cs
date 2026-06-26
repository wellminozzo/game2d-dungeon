using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Core;

public class SceneManager
{
    private readonly Dictionary<string, IScene> _scenes = new();
    private IScene? _currentScene;
    private string? _currentSceneName;
    private ContentManager? _content;

    public string? CurrentSceneName => _currentSceneName;

    public void AddScene(string name, IScene scene)
    {
        scene.OnSceneRequested = target =>
        {
            if (_scenes.ContainsKey(target))
                SetScene(target);
        };

        _scenes[name] = scene;
    }

    public void SetScene(string name)
    {
        if (!_scenes.ContainsKey(name))
            return;

        _currentScene?.UnloadContent();
        _currentScene = _scenes[name];
        _currentSceneName = name;
        _currentScene.Initialize();

        if (_content != null)
            _currentScene.LoadContent(_content);
    }

    public void LoadContent(ContentManager content)
    {
        _content = content;
        _currentScene?.LoadContent(content);
    }

    public void Update(GameTime gameTime)
    {
        _currentScene?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _currentScene?.Draw(spriteBatch);
    }
}
