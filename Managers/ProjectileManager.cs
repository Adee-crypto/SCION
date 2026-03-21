using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Controllers;
using Sprint2.Entities;
using Sprint2.Entities.Players;
using Sprint2.Entities.Projectiles;
using Sprint2.Extensions;
using Sprint2.Levels;
using System.Collections.Generic;

namespace Sprint2.Managers;

public class ProjectileManager(BaseLevel level, Player player) : Extensions.IDrawable, IUpdatable
{
    private readonly BaseLevel level = level; //This is terrible for coupling idk how to fix
    private readonly Collider playerCollider = player.Collider;
    private readonly List<IProjectile> projectiles = [];

    public void Reset()
    {
        projectiles.Clear();
    }

    public void Spawn(ProjectileType type, float lifeTime, Vector2 initialPosition, Vector2 initialVelocity)
    {
        projectiles.Add(new Projectile(level, type, lifeTime, initialPosition, initialVelocity));
    }

    public void Update(GameTime gameTime)
    {
        //Launch projectile
        if (MouseController.IsLeftClick() && player.Seeds.Count > 0)
        {
            Vector2 direction = player.AimDirection;

            if (direction.LengthSquared() > 0.0001f)
            {
                direction.Normalize();
                Vector2 initialPosition = playerCollider.Center;// + Consts.playerHitbox * new Vector2(direction.X * 0.5f, -0.25f);
                Vector2 initialVelocity = direction * 300f;
                Projectile projectile = new(level, player.ThrowSeed(), 5f, initialPosition, initialVelocity);
                projectiles.Add(projectile);
                player.Collider.Momentum -= projectile.Collider.Momentum;
            }
        }

        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            projectiles[i].Update(gameTime);

            if (projectiles[i] is Projectile p && !p.IsDead && p.Type == ProjectileType.Void && p.Collider.Intersects(playerCollider))
            {
                p.Collider.KnockBack(playerCollider);
                player.TakeDamage(1);
                p.Kill();
            }

            if (projectiles[i].IsDead) projectiles.RemoveAt(i);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        projectiles.ForEach(p => p.Draw(spriteBatch));
    }
}