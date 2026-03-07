using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Util;
using System.Linq;

namespace Sprint2.Entities.Projectiles;

public class Projectile : IProjectile
{
    private readonly ProjectileSprite Sprite;
    public Collider Collider { get; }
    public bool IsGEMaxLifetime { get; private set; }

    public Projectile(ProjectileSprite Sprite, float gravity, float mass, Vector2 initialPosition, Vector2 initialVelocity, Vector2 size)
    {
        this.Sprite = Sprite;
        Collider = new(gravity, mass, initialPosition, initialVelocity, size);
        IsGEMaxLifetime = false;
    }

    public void Update(GameTime gameTime, CollisionManager collisionManager)
    {
        if (IsGEMaxLifetime) return;

        Sprite.UpdateFrameState(gameTime);
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Collider.SetPositionY(Collider.Position.Y + (Collider.Velocity.Y + 0.5f * Consts.playerGravity * dt) * dt);
        Collider.SetVelocityY(Collider.Velocity.Y + Collider.Gravity * dt);
        Collider.SetPositionX(Collider.Position.X + Collider.Velocity.X * dt);

        if (Sprite.Ticker.TickAge >= Sprite.MaxLifetimeSeconds || collisionManager.Objects.Any(o => o.Intersects(Collider.Hitbox))) Kill();

    }

    public void Kill() => IsGEMaxLifetime = true;

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsGEMaxLifetime)
            spriteBatch.Draw(Assets.BlockSpriteSheet, Collider.Position, Sprite.CurrentSourceRect, Color.White, Collider.Angle, Sprite.Origin, 1f, SpriteEffects.None, 0f);
    }
}