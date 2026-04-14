
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Players;
using Sprint2.Extensions;
using Sprint2.Levels;

namespace Sprint2.Managers;

public class LevelManager
{
    private ILevel current;
    public bool IsGameOver => current != null && current.IsOver;
    public LevelEndReason EndReason => current?.EndReason ?? LevelEndReason.None;
    public bool IsRunning => current != null;

    public void StartArcade(Player player)
    {
        current = new ArcadeLevel(player);
    }

    public void StartStory(Player player, int levelIndex)
    {
        var def = StoryLevelRegistry.Get(levelIndex);
        current = new StoryLevel(player, def);
    }

    public void Reset()
    {
        current?.Reset();
    }

    public void Update(GameTime gameTime) {
        current?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch) => current?.Draw(spriteBatch);
}