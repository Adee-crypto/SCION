using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint2;

public class EnemyManager : IDrawableObject
{
    private readonly List<Enemy> enemies = new();

    public EnemyManager() {}

    public void Spawn(EnemyDef type, Vector2 spawnPos)
    {
        enemies.Add(new Enemy(type, spawnPos));
    }

    public void Reset()
    {
        enemies.Clear();
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects, Player player)
    {
        foreach (var e in enemies)
        {
            e.Update(gameTime, objects, player);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var e in enemies) e.Draw(spriteBatch);
    }
}