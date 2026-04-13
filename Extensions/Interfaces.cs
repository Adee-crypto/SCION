using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Entities;
using Sprint2.Entities.Projectiles;

namespace Sprint2.Extensions;

public interface IDrawable
{
    void Draw(SpriteBatch spriteBatch);
}

public interface IUpdatable
{
    void Update(GameTime gameTime);
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

public interface IPlayer : IDrawable, IPhysicsObject, IAim//, IUpdatable
{
    void Move(int direction);
    void TryJump();
    void Attack();
    void TryBreakBlock();
    void ToggleDamaged();
    void Reset();
    void UpdateHealth(bool isDamaged, float time);
    void UpdateBreakBlock(float time);
    void ChangeItem(int num);
}

public interface IPlayerProvider
{
    IPlayer CurrentPlayer { get; }
}

public interface IAim
{
    Vector2 AimDirection { get; }
}

public interface IProjectile : IDrawable, IUpdatable, IPhysicsObject
{
    ProjectileType Type { get; }
    bool IsDead { get; }
    void Kill();
}

public interface IResizable
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

public interface IOverlay : IUpdatable, IDrawable
{
    void OnOpen();
    void OnClose();
}

public interface IPausableScreen
{
    bool IsPaused { get; }
    void TogglePause();
}

public interface ISettingsPanel : IDrawable, IResizable
{
    void Update();
    void BuildPanel();
}

public interface ISettingsRow
{
    int Height { get;}

    void SetBounds(Rectangle bounds);

    void Update(
        MouseState mouse, 
        MouseState previousMouse, 
        KeyboardState keyboard, 
        KeyboardState previousKeyboard
    );

    void Draw(SpriteBatch spriteBatch);
}

public interface ILevel : IUpdatable, IDrawable, IResizable, IResettableScreen
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

public enum SettingsTab
{
    General,
    Graphics,
    Controls,
    Dev
}

public interface ITunableFloat
{
    float DefaultValue { get; }
    float Value { get; set; }
    void Reset();
}

public interface ITunableInt
{
    int DefaultValue { get; }
    int Value { get; set; }
    void Reset();
}

// internal interface IEntitySprite : IUpdatable
// {
//     void SetFrames(PlayerState linkAction, Vector2 direction, Vector2 velocity, bool isDamaged);
// }