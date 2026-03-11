
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Entities.Players;
using Sprint2.Extensions;
using Sprint2.Levels;

namespace Sprint2.Managers;

public class LevelManager
{
    private ILevel current;
    private (int w, int h) screenSize;
    public bool IsGameOver => current != null && current.IsOver;
    public LevelEndReason EndReason => current?.EndReason ?? LevelEndReason.None;
    public bool IsRunning => current != null;

    public void Resize((int w, int h) size)
    {
        screenSize = size;
        current?.Resize(size);
    }

    public void StartArcade(Player player)
    {
        current = new ArcadeLevel(player);
        if (screenSize.w > 0 && screenSize.h > 0) current.Resize(screenSize);
    }

    public void StartStory(Player player, int levelIndex)
    {
        var def = StoryLevelRegistry.Get(levelIndex);
        current = new StoryLevel(player, def);

        if (screenSize.w > 0 && screenSize.h > 0) current.Resize(screenSize);
    }

    public void Reset()
    {
        current?.Reset();
        if (screenSize.w > 0 && screenSize.h > 0) current?.Resize(screenSize);
    }

    public void Update(GameTime gameTime) {
        current?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch) => current?.Draw(spriteBatch);
}