
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
    public bool Frozen { get; set; }

    public void StartArcade(Player player)
    {
        current = new ArcadeLevel(player);
    }

    public void StartStory((int, int) levelCoords, Player player, Vector2 playerCoords)
    {
        current = new StoryLevel(StoryLevelRegistry.Get(levelCoords), player, playerCoords);
    }

    public void Reset()
    {
        current?.Reset();
    }

    public void Update(GameTime gameTime) {
        if (!Frozen) current?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch) => current?.Draw(spriteBatch);
}