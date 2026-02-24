using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
using Sprint2.Entities.Projectiles;
using System.Collections.Generic;

namespace Sprint2;

public class ProjectileManager(IMouseController mouse, Player player) : IDrawableObject, IUpdatableObject
{
    private readonly IMouseController mouse = mouse;
    private readonly Player player = player;
    private readonly List<IProjectile> projectiles = [];

    public void Reset()
    {
        projectiles.Clear();
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {

        if (mouse.IsLeftClick() && player.Seeds.Count > 0)
        {
            Vector2 direction = player.AimDirection;

            if (direction.LengthSquared() > 0.0001f)
            {
                direction.Normalize();

                string seedSpecies = player.ThrowSeed();
                ProjectileDef def = new(seedSpecies);

                Vector2 spawnPos = player.Center + direction * 12f;
                Vector2 initialVelocity = direction * def.LaunchSpeed + player.velocity;

                projectiles.Add(new Projectile(def, spawnPos, initialVelocity));
            }
        }

        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            projectiles[i].Update(gameTime, objects);
            if (!projectiles[i].IsAlive) projectiles.RemoveAt(i);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        projectiles.ForEach(p => p.Draw(spriteBatch));
    }
}