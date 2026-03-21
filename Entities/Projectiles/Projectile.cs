using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Colliders;
using Sprint2.Extensions;
using Sprint2.Levels;
using Sprint2.Util;

namespace Sprint2.Entities.Projectiles;

public enum ProjectileType
{
    Void,
    Grass,
    Apple,
    Pineapple,
    Sandbox,
}

public class Projectile : IProjectile
{
    private readonly BaseLevel level; //this is terrible for coupling but idk
    public ProjectileType Type { get; }
    private ProjectileSprite Sprite { get; }
    public Collider Collider { get; }
    public bool IsDead { get; private set; }

    public Projectile(BaseLevel level, ProjectileType type, float lifeTime, Vector2 initialPosition, Vector2 initialVelocity)
    {
        this.level = level; //this is terrible for coupling but idk
        Type = type;
        Sprite = new(type, lifeTime);
        Collider = ColliderUtil.Presets[ColliderType.Projectile](initialPosition, initialVelocity);

        //add a lookup somewhere to change collider gravity based on projectile type
        if (type == ProjectileType.Void) {
            Collider.Gravity = 0;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (IsDead) return;
        if (Sprite.Ticker.TickAge >= Sprite.MaxLifetimeSeconds) Kill();

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Sprite.UpdateFrameState(gameTime);
        var prevCoords = Funcs.GridCoords(Collider.Position);
        var coords = Collider.UpdateMovement(dt, level.CollisionManager).collisionCoords;

        //ASSUMES PROJECTILES HAVE ZERO SIZE FOR NOW
        // System.Console.WriteLine(Collider.Position);
        if (coords is not null) {
            if (ProjectileUtil.ProjectileToPlant.ContainsKey(Type)) { //eventually change this to check for type of collider too
                level.TrySow(Type, prevCoords, coords.Value);
            }
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