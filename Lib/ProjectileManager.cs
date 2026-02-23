using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint2;

public class ProjectileManager : IDrawableObject, IUpdatableObject
{
    private readonly IMouseController mouse;
    private readonly Player player;
    private readonly List<IProjectile> projectiles = new();
    private readonly ProjectileDef def;

    public ProjectileManager(IMouseController mouse, Player player, ProjectileDef def)
    {
        this.mouse = mouse;
        this.player = player;
        this.def = def;
    }

    public void Reset()
    {
        projectiles.Clear();
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {

        if (mouse.IsLeftClick())
        {
            Vector2 direction = player.AimDirection;

            if (direction.LengthSquared() > 0.0001f)
            {
                direction.Normalize();
                Vector2 spawnPos = player.Center + direction * 12f;
                Vector2 initialVelocity = direction * def.LaunchSpeed;

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
        foreach (var p in projectiles) p.Draw(spriteBatch);
    }
}