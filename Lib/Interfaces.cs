using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Sprint2.Entities;

namespace Interfaces;

public interface IDrawableObject
{
    void Draw(SpriteBatch spriteBatch);
}

public interface IUpdatable
{
    void Update(GameTime gameTime);
}

public interface IUpdatableObject
{
    void Update(GameTime gameTime, IEnumerable<Rectangle> objects);
}

public interface IPhysicsObject
{
    Vector2 Position { get; set; }
    Rectangle Hitbox { get; }
}

public interface IController
{
    bool IsPaused { get; set; } 
    void Update();
}

public interface IMouseController : IController
{
    bool IsLeftClick();

    bool IsRightClick();

    bool IsLeftClickHeld();

    bool IsRightClickHeld();
}

public interface IPlayer : IDrawableObject, IUpdatableObject, IPhysicsObject, IAim
{
    void Reset();

    void Move(int direction);

    void Jump();

    void BreakBlock();

    void PlantSeed();

    void Attack();

    void Damaged();

    void UpdateHealth(bool isDamaged, float time);

    void UpdateBreakBlock(float time);
}

public interface IAim
{
    Vector2 AimDirection { get; }
}

public interface IProjectile : IDrawableObject, IUpdatableObject, IPhysicsObject
{
    bool IsAlive { get; }
}

internal interface IEntitySprite : IUpdatable
{
    void SetFrames(PlayerState linkAction, Vector2 direction, Vector2 velocity, bool isDamaged);
}