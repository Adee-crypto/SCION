using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
using Sprint2.Entities.Players;
using Sprint2.Entities.Projectiles;
using System.Collections.Generic;

namespace Sprint2;

public class ProjectileManager(IMouseController mouse, Player player) : Interfaces.IDrawable, IUpdatableObject
{
    private readonly IMouseController mouse = mouse;
    private readonly Collider playerCollider = player.Collider;
    private readonly List<IProjectile> projectiles = [];

    public void Reset()
    {
        projectiles.Clear();
    }

    public void Spawn(ProjectileDef def, Vector2 spawnPos, Vector2 initialVelocity)
    {
        projectiles.Add(new Projectile(def, spawnPos, initialVelocity));
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

                Vector2 spawnPos = playerCollider.Center + direction * 12f;
                Vector2 initialVelocity = direction * def.LaunchSpeed + playerCollider.Velocity;

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