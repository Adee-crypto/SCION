using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
using Sprint2.Entities.Players;
using Sprint2.Entities.Projectiles;
using Sprint2.Extensions;
using System;
using System.Collections.Generic;

namespace Sprint2.Managers;

public class ProjectileManager(
    EnemyManager enemyManager,
    Player player,
    Func<bool> fireInput,                                               // e.g. () => MouseController.IsLeftClick()
    Func<ProjectileType, float, Vector2, Vector2, Projectile> factory) // e.g. (t,l,p,v) => new Projectile(level, t,l,p,v)
    : Extensions.IDrawable, IUpdatable
{
    private readonly EnemyManager enemyManager = enemyManager;
    private readonly Collider playerCollider = player.Collider;
    private readonly Func<bool> fireInput = fireInput;
    private readonly Func<ProjectileType, float, Vector2, Vector2, Projectile> factory = factory;
    private readonly List<IProjectile> projectiles = [];

    public void Reset() => projectiles.Clear();

    public void Spawn(ProjectileType type, float lifeTime, Vector2 initialPosition, Vector2 initialVelocity)
    {
        projectiles.Add(factory(type, lifeTime, initialPosition, initialVelocity));
    }

    public void Update(GameTime gameTime)
    {
        TryFireProjectile(player);

        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            projectiles[i].Update(gameTime);
            if (projectiles[i] is Projectile p)
                HandleProjectileHits(p, player);
            if (projectiles[i].IsDead)
                projectiles.RemoveAt(i);
        }
    }

    private void TryFireProjectile(Player player)
    {
        if (!fireInput() || player.Seeds.Count == 0 || player.Item != Item.Seed)
            return;

        Vector2 direction = player.AimDirection;
        if (direction.LengthSquared() <= 0.0001f)
            return;

        direction.Normalize();
        Projectile projectile = factory(player.ThrowSeed(), 5f, playerCollider.Center, direction * 300f);
        projectiles.Add(projectile);
        playerCollider.Momentum -= projectile.Collider.Momentum;
    }

    private void HandleProjectileHits(Projectile p, Player player)
    {
        if (p.IsDead) return;

        if (p.Type == ProjectileType.Void && p.Collider.Intersects(playerCollider))
        {
            p.Collider.KnockBack(playerCollider);
            player.TakeDamage(1);
            p.Kill();
            return;
        }

        if (p.Type != ProjectileType.Void)
            enemyManager.CheckProjectileHit(p);
    }

    public void Draw(SpriteBatch spriteBatch) => projectiles.ForEach(p => p.Draw(spriteBatch));
}