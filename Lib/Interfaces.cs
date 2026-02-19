using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2;
using System;
using System.Collections.Generic;
using static Sprint2.Sprites.PlayerSprite;

namespace Interfaces;

public interface IDrawableObject
{
    public void Draw(SpriteBatch spriteBatch);
}

public interface IUpdatable
{
    public void Update(GameTime gameTime);
}

public interface IUpdatableObject
{
    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects);
}

public interface IPhysicsObject
{
    Vector2 Position { get; set; }
    Rectangle Hitbox { get; }
}

public interface IController
{
    public bool IsPaused { get; set; } 
    public void Update();
}

public interface IMouseController : IController
{
    public bool IsLeftClick();

    public bool IsRightClick();
}

public interface IPlayer : IDrawableObject, IUpdatableObject, IPhysicsObject
{
    public void Reset();

    public void Move(int index);

    public void MoveLeft();

    public void MoveRight();

    public void Jump();

    public void BreakBlock();

    public void PlantSeed();

    public void Attack();
}

internal interface ISprite : IDrawableObject, IUpdatable
{
    public void SetFrames(PlayerUtil.PlayerAction linkAction, Vector2 direction, Vector2 velocity);
}