using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Enemies;
using Sprint2.Entities.Players;
using System.Collections.Generic;

namespace Sprint2.Managers;

public class EnemyManager : Extensions.IDrawable
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

    public void Update(GameTime gameTime, Player player, ProjectileManager projectileManager, CollisionManager collisionManager)
    {
        enemies.ForEach(e => e.Update(gameTime, player, projectileManager, collisionManager));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        enemies.ForEach(e => e.Draw(spriteBatch));
    }
}