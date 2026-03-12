using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Plants;
using Sprint2.Extensions;
using Sprint2.Levels;
using Sprint2.Managers;
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

public class Projectile(BaseLevel level, ProjectileType type, float lifeTime, float gravity, float mass, Vector2 initialPosition, Vector2 initialVelocity, Vector2 size) : IProjectile
{
    public static Dictionary<ProjectileType, Func<BaseLevel, (int, int), Plant>> ProjectileToPlant { get; } = new() {
        { ProjectileType.Grass, (c, r) => new GrassPlant(c, r) },
        { ProjectileType.Apple, (c, r) => new ApplePlant(c, r) },
        { ProjectileType.Pineapple, (c, r) => new PineapplePlant(c, r) },
        { ProjectileType.Sandbox, (c, r) => new SandboxPlant(c, r) },
    };

    private readonly BaseLevel level = level; //this is terrible for coupling but idk
    public ProjectileType Type { get; } = type;
    private ProjectileSprite Sprite { get; }= new(type, lifeTime);
    public Collider Collider { get; } = new(gravity, mass, initialPosition, initialVelocity, size);
    public bool IsDead { get; private set; }

    public void Update(GameTime gameTime, CollisionManager collisionManager)
    {
        if (IsDead) return;
        if (Sprite.Ticker.TickAge >= Sprite.MaxLifetimeSeconds) Kill();

        Sprite.UpdateFrameState(gameTime);
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //ASSUMES PROJECTILES HAVE ZERO SIZE FOR NOW
        (bool isCollision, var _) = Collider.Update(dt, collisionManager);
        if (isCollision) {
            if (ProjectileToPlant.ContainsKey(Type)) { //eventually change this to check for type of collider too
                level.TrySow(Type, Funcs.GridCoords(Collider.Position));
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