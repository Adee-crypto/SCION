using Sprint2.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
using Sprint2.Entities.Players;
using Sprint2.Entities.Projectiles;
using System;
using System.Collections.Generic;
using Sprint2.Controllers;

namespace Sprint2.Managers;

public class ProjectileManager(Player player) : Extensions.IDrawable, IUpdatableObject
{
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

    public void Update(GameTime gameTime, CollisionManager collisionManager)
    {

        if (MouseController.IsLeftClick() && player.Seeds.Count > 0)
        {
            Vector2 direction = player.AimDirection;

            if (direction.LengthSquared() > 0.0001f)
            {
                direction.Normalize();

                string seedSpecies = player.ThrowSeed();
                ProjectileDef def = new(seedSpecies);

                Vector2 spawnPos = playerCollider.Center + direction * 12f;
                Vector2 initialVelocity;
                if (Vector2.Dot(direction, playerCollider.Velocity) > 0) { initialVelocity = direction * def.LaunchSpeed + playerCollider.Velocity; }
                else initialVelocity = direction * def.LaunchSpeed;

                projectiles.Add(new Projectile(def, spawnPos, initialVelocity));
            }
        }

        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            projectiles[i].Update(gameTime, collisionManager);
            if (!projectiles[i].IsAlive) projectiles.RemoveAt(i);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        projectiles.ForEach(p => p.Draw(spriteBatch));
    }
}