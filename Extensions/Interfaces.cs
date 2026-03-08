using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
using Sprint2.Managers;

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
    void Update(GameTime gameTime, CollisionManager collisionManager);
}

public interface IPhysicsObject
{
    Collider Collider { get; }
}

// public interface IController
// {
//     bool IsPaused { get; set; }
//     void Update();
// }

// public interface IMouseController : IController
// {
//     bool IsLeftClick();

//     bool IsRightClick();

//     bool IsLeftClickHeld();

//     bool IsRightClickHeld();
// }

public interface IPlayer : IDrawable, IUpdatableObject, IPhysicsObject, IAim
{
    void Move(int direction);
    void Jump();
    void Attack();
    void BreakBlock();
    void ToggleDamaged();
    void Reset();
    void UpdateHealth(bool isDamaged, float time);
    void UpdateBreakBlock(float time);
}

public interface IPlayerProvider
{
    IPlayer CurrentPlayer { get; }
}

public interface IAim
{
    Vector2 AimDirection { get; }
}

public interface IProjectile : IDrawable, IUpdatableObject, IPhysicsObject
{
    bool IsDead { get; }
}

public interface IResizableScreen
{
    void Resize((int w, int h) size);
}

public interface IResettableScreen
{
    void Reset();
}

public interface IScreen : IUpdatable, IDrawable
{
    void OnEnter();
    void OnExit();
}

public interface IPausableScreen
{
    bool IsPaused { get; }
    void TogglePause();
}

public interface ILevel : IUpdatable, IDrawable, IResizableScreen, IResettableScreen
{
    bool IsOver { get; }
    LevelEndReason EndReason { get; }
}

public enum LevelEndReason
{
    None,
    PlayerDied,
    Completed
}

// internal interface IEntitySprite : IUpdatable
// {
//     void SetFrames(PlayerState linkAction, Vector2 direction, Vector2 velocity, bool isDamaged);
// }