using Interfaces;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprint2;

public class Projectile(ProjectileDef type, Vector2 initialPosition, Vector2 initialVelocity) : IProjectile
{
    private readonly ProjectileDef def = type;
    public Vector2 Position { get; set; } = initialPosition;
    private Vector2 velocity = initialVelocity;
    private float angle = MathF.Atan2(initialVelocity.Y, initialVelocity.X);

    private float age = 0f;
    public bool IsAlive { get; private set; } = true;

    public Rectangle Hitbox => new((int)(Position.X - def.Size.X / 2f), (int)(Position.Y - def.Size.Y / 2f), (int)def.Size.X, (int)def.Size.Y);

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {
        if (!IsAlive) return;

        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        age += time;
        if (age >= def.MaxLifetimeSeconds)
        {
            IsAlive = false;
            return;
        }

        velocity += new Vector2(0f, def.Gravity) * time;
        Position += velocity * time;

        if (velocity.LengthSquared() > 0.0001f)
        {
            angle = MathF.Atan2(velocity.Y, velocity.X);
        }

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
        spriteBatch.Draw(def.Texture, Position, null, Color.White, angle, def.Origin, 1f, SpriteEffects.None, 0f);
    }
}