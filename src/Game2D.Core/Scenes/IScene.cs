using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Core;

public interface IScene
{
    Action<string>? OnSceneRequested { get; set; }
    void Initialize();
    void LoadContent(ContentManager content);
    void UnloadContent();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
