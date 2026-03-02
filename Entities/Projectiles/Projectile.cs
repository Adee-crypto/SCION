using Sprint2.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using System.Collections.Generic;
using System.Linq;
using Sprint2.Managers;

namespace Sprint2.Entities.Projectiles;

public class Projectile : IProjectile
{
    private readonly ProjectileDef def;
    public Collider Collider { get; }  //change this from Vector2.Zero

    public bool IsAlive { get; private set; } = true;

    public Projectile(ProjectileDef def, Vector2 initialPosition, Vector2 initialMomentum)
    {
        this.def = def;
        Collider = new(initialPosition, Vector2.Zero);
        Collider.SetMomentum(initialMomentum);
    }

    public void Update(GameTime gameTime, CollisionManager collisionManager) {
        if (!IsAlive) return;

        def.UpdateFrameState(gameTime);

        if (def.Ticker.TickAge >= def.MaxLifetimeSeconds) { //kill if too old
            Kill();
            return;
        }

        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
        //update position and momentum
        Collider.SetMomentumY(Collider.Momentum.Y + Collider.Mass * def.Gravity * dt);
        Collider.SetPosition(Collider.Position + Collider.Velocity * dt);

        if (collisionManager.Objects.Any(o => o.Intersects(Collider.Hitbox))) Kill();  //kill on collision
    }

    public void Kill() => IsAlive = false;

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsAlive)
            spriteBatch.Draw(Assets.PlantSpritesheet, Collider.Position, def.CurrentSourceRect, Color.White, Collider.Angle, def.Origin, 1f, SpriteEffects.None, 0f);
    }
}