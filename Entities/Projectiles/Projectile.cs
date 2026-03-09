using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Projectiles;

public class Projectile(ProjectileSprite Sprite, float gravity, float mass, Vector2 initialPosition, Vector2 initialVelocity, Vector2 size) : IProjectile
{
    private ProjectileSprite Sprite { get; }= Sprite;
    public Collider Collider { get; } = new(gravity, mass, initialPosition, initialVelocity, size);
    public bool IsDead { get; private set; }

    public void Update(GameTime gameTime, CollisionManager collisionManager)
    {
        if (IsDead) return;

        Sprite.UpdateFrameState(gameTime);
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Collider.Update(dt, collisionManager);

        if (Sprite.Ticker.TickAge >= Sprite.MaxLifetimeSeconds) Kill();

        //ASSUMES PROJECTILES HAVE ZERO SIZE
        if (collisionManager.Blocks.Contains(((int) (Collider.Left / Consts.BlockWidth), (int) (Collider.Right / Consts.BlockWidth)))) {
            Kill();
        }

        //DOESNT HANDLE PROJECTILE-TO-ENTITY COLLISION YET

    }

    public void Kill() => IsDead = true;

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsDead)
            spriteBatch.Draw(Assets.BlockSpriteSheet, Collider.Position, Sprite.CurrentSourceRect, Color.White, Collider.Angle, Sprite.Origin, 1f, SpriteEffects.None, 0f);
    }
}