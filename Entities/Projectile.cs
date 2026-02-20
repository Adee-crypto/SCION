using Interfaces;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprint2;

public class Projectile : IProjectile
{
    private readonly ProjectileDef def;
    private Vector2 position;
    private Vector2 velocity;
    private float angle;

    private float age;
    public bool IsAlive { get; private set; } = true;

    public Projectile(ProjectileDef type, Vector2 initialPosition, Vector2 initialVelocity)
    {
        def = type;
        position = initialPosition;
        velocity = initialVelocity;
        age = 0f;

        angle = MathF.Atan2(initialVelocity.Y, initialVelocity.X);
    }

    public Vector2 Position
    {
        get => position;
        set => position = value;
    }

    public Rectangle Hitbox => new Rectangle((int)(position.X - def.Size.X / 2f), (int)(position.Y - def.Size.Y / 2f), (int)def.Size.X, (int)def.Size.Y);

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
        position += velocity * time;

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
        spriteBatch.Draw(def.Texture, position, null, Color.White, angle, def.Origin, 1f, SpriteEffects.None, 0f);
    }
}