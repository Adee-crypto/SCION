using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Enemies;
using Sprint2.Entities.Players;
using Sprint2.Levels;
using System.Collections.Generic;

namespace Sprint2.Managers;

public class EnemyManager(BaseLevel level) : Extensions.IDrawable
{
    private readonly BaseLevel level = level;
    private readonly List<Enemy> enemies = [];

    public void Spawn(Vector2 initialPosition)
    {
        enemies.Add(new(level, initialPosition));
    }

    public void Reset()
    {
        enemies.Clear();
    }

    public void Update(GameTime gameTime)
    {
        enemies.ForEach(e => e.Update(gameTime));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        enemies.ForEach(e => e.Draw(spriteBatch));
    }
}