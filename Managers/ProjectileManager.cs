using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Controllers;
using Sprint2.Entities;
using Sprint2.Entities.Players;
using Sprint2.Entities.Projectiles;
using Sprint2.Extensions;
using Sprint2.Levels;
using Sprint2.Util;
using System.Collections.Generic;

namespace Sprint2.Managers;

public class ProjectileManager(BaseLevel level, Player player) : Extensions.IDrawable, IUpdatableObject
{
    private readonly BaseLevel level = level; //This is terrible for coupling idk how to fix
    private readonly Collider playerCollider = player.Collider;
    private readonly List<IProjectile> projectiles = [];

    public void Reset()
    {
        projectiles.Clear();
    }

    public void Spawn(ProjectileType type, float lifeTime, float gravity, float mass, Vector2 initialPosition, Vector2 initialVelocity, Vector2 size)
    {
        projectiles.Add(new Projectile(level, type, lifeTime, gravity, mass, initialPosition, initialVelocity, size));
    }

    public void Update(GameTime gameTime, CollisionManager collisionManager)
    {
        //Launch projectile
        if (MouseController.IsLeftClick() && player.Seeds.Count > 0)
        {
            Vector2 direction = player.AimDirection;

            if (direction.LengthSquared() > 0.0001f)
            {
                direction.Normalize();
                Vector2 initialPosition = playerCollider.Center + direction * 12f;
                Vector2 initialVelocity = direction * 300f;
                Projectile projectile = new(level, player.ThrowSeed(), 5f, Consts.playerProjectileGravity, Consts.projectileMass, initialPosition, initialVelocity, new(8, 8));
                projectiles.Add(projectile);
                //knockback
                player.Collider.Momentum -= projectile.Collider.Momentum;
            }
        }

        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            projectiles[i].Update(gameTime, collisionManager);
            if (projectiles[i].IsDead) projectiles.RemoveAt(i);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        projectiles.ForEach(p => p.Draw(spriteBatch));
    }
}