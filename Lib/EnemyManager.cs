using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
using Sprint2.Entities.Enemies;
using System.Collections.Generic;

namespace Sprint2;

public class EnemyManager : IDrawableObject
{
    private readonly List<Enemy> enemies = [];

    public EnemyManager() { }

    public void Spawn(EnemyDef type, Vector2 spawnPos)
    {
        enemies.Add(new(type, spawnPos));
    }

    public void Reset()
    {
        enemies.Clear();
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects, Player player, ProjectileManager projectileManager)
    {
        enemies.ForEach(e => e.Update(gameTime, objects, player, projectileManager));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        enemies.ForEach(e => e.Draw(spriteBatch));
    }
}