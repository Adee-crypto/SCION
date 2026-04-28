using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Colliders;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Util;
using System;

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
    public ProjectileType Type { get; }
    private ProjectileSprite Sprite { get; }
    public Collider Collider { get; }
    public bool IsDead { get; private set; }

    private readonly CollisionManager collisionManager;
    private readonly Action<ProjectileType, (int, int)> onSow;    // called when a plant projectile hits a block
    private readonly Action<(int, int)> onInfect;                 // called when a void projectile hits a non-void block

    public Projectile(
        CollisionManager collisionManager,
        ProjectileType type,
        float lifeTime,
        Vector2 initialPosition,
        Vector2 initialVelocity,
        Action<ProjectileType, (int, int)> onSow,
        Action<(int, int)> onInfect)
    {
        this.collisionManager = collisionManager;
        this.onSow = onSow;
        this.onInfect = onInfect;
        Type = type;
        Sprite = new(type, lifeTime);
        Collider = ColliderUtil.Presets[ColliderType.Projectile](initialPosition, initialVelocity);

        if (type == ProjectileType.Void)
            Collider.Gravity = 0;
    }

    public void Update(GameTime gameTime)
    {
        if (IsDead) return;
        if (Sprite.Ticker.TickAge >= Sprite.MaxLifetimeSeconds) Kill();

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Sprite.UpdateFrame(gameTime);
        var prevCoords = Funcs.GridCoords(Collider.Position);
        var coords = Collider.Update(collisionManager, dt);

        //ASSUMES PROJECTILES HAVE ZERO SIZE FOR NOW
        if (coords is not null)
        {
            if (ProjectileUtil.ProjectileToPlant.ContainsKey(Type))
                onSow(Type, prevCoords);
            else if (Type == ProjectileType.Void)
                onInfect(coords.Value);
            Kill();
        }
    }

    public void Kill() => IsDead = true;
    
    public void Draw(SpriteBatch spriteBatch)
    {
    if (!IsDead)
        spriteBatch.Draw(
            Assets.BallTexture,
            Collider.Position,
            null,
            GetProjectileColor(Type),
            Collider.Angle,
            new Vector2(8, 8),
            1f,
            SpriteEffects.None,
            0f
        );
    }

    // 👇 PUT THE METHOD RIGHT HERE (inside the class, before the final closing brace)
    public static Color GetProjectileColor(ProjectileType type)
    {
        return type switch
        {
            ProjectileType.Grass => Color.Brown,
            ProjectileType.Apple => Color.Green,
            ProjectileType.Pineapple => Color.Purple,
            ProjectileType.Sandbox => Color.Red,
            ProjectileType.Void => Color.White,
            _ => Color.White
        };
    }

}