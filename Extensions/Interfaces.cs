using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
using System.Collections.Generic;

namespace Sprint2.Extensions;

public interface IDrawable
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
    Collider Collider {get;}
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

public interface IPlayer : IDrawable, IUpdatableObject, IPhysicsObject, IAim
{
    void Reset();

    void Move(int direction);

    void Jump();

    void BreakBlock();

    void Attack();

    void ToggleDamaged();

    void UpdateHealth(bool isDamaged, float time);

    void UpdateBreakBlock(float time);
}

public interface IAim
{
    Vector2 AimDirection { get; }
}

public interface IProjectile : IDrawable, IUpdatableObject, IPhysicsObject
{
    bool IsAlive { get; }
}

// internal interface IEntitySprite : IUpdatable
// {
//     void SetFrames(PlayerState linkAction, Vector2 direction, Vector2 velocity, bool isDamaged);
// }