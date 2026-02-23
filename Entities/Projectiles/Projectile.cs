using Interfaces;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using System;

namespace Sprint2.Entities.Projectiles;

public class Projectile(ProjectileDef def, Vector2 initialPosition, Vector2 initialVelocity) : IProjectile
{
    private readonly ProjectileDef def = def;
    public Vector2 Position { get; set; } = initialPosition;
    private Vector2 velocity = initialVelocity;

    private float angle => MathF.Atan2(velocity.Y, velocity.X);

    public bool IsAlive { get; private set; } = true;

    //This is kind of broken currently, so I have 0x0 hitbox
    public Rectangle Hitbox => new ((int)Position.X, (int)Position.Y, 0, 0);//new((Position - def.HitBox / 2f).ToPoint(), def.HitBox.ToPoint());

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {
        if (!IsAlive) return;

        def.Update(gameTime);
        if (def.age >= def.MaxLifetimeSeconds)
        {
            IsAlive = false;
            return;
        }

        velocity += new Vector2(0f, def.Gravity) * def.time;
        Position += velocity * def.time;

        // if (velocity.LengthSquared() > 0.0001f) I hope having this commented doesnt break anything
        // {
        //     angle = MathF.Atan2(velocity.Y, velocity.X);
        // }

        Rectangle hitbox = Hitbox;
        foreach (var o in objects)
        {
            if (o.Intersects(hitbox))
            {
                IsAlive = false;
                return;
            }
        }
    }

    public void Kill() => IsAlive = false;

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsAlive) return;
        spriteBatch.Draw(Assets.plantSpritesheet, Position, def.SourceRect, Color.White, angle, def.Origin, 1f, SpriteEffects.None, 0f);
    }
}