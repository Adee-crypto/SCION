using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprint2.Entities.Projectiles;

public class Projectile(ProjectileDef def, Vector2 initialPosition, Vector2 initialVelocity) : IProjectile
{
    private readonly ProjectileDef def = def;
    public Collider Collider { get; } = new(initialPosition, Vector2.Zero) {Velocity = initialVelocity};  //change this from Vector2.Zero

    public bool IsAlive { get; private set; } = true;

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects) {
        if (!IsAlive) return;

        def.UpdateFrameState(gameTime);

        if (def.Age >= def.MaxLifetimeSeconds) { //kill if too old
            Kill();
            return;
        }
        //update position and velocity
        Collider.Velocity += new Vector2(0f, def.Gravity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Collider.Position += Collider.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        // if (velocity.LengthSquared() > 0.0001f) I hope having this commented doesnt break anything
        //     angle = MathF.Atan2(velocity.Y, velocity.X);
        if (objects.Any(o => o.Intersects(Collider.Hitbox))) Kill();  //kill on collision
    }

    public void Kill() => IsAlive = false;

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsAlive)
            spriteBatch.Draw(Assets.PlantSpritesheet, Collider.Position, def.CurrentSourceRect, Color.White, Collider.Angle, def.Origin, 1f, SpriteEffects.None, 0f);
    }
}