using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Sprint2.Sprites.LinkSprite;
using System;

namespace Interfaces;

public interface IScreenObject
{
    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects);
    public void Draw(SpriteBatch spriteBatch);
}

public interface IController
{
    public bool IsPaused { get; set; } 
    public void Update();
}

public interface IPlayer : IScreenObject
{
    public Vector2 Position { get; set; }
    public Rectangle Hitbox { get; }
    
    public void InitializeAimer(Texture2D aimerTexture);

    public void Move(int index);

    public void MoveLeft();

    public void MoveRight();

    public void Jump();

    public void BreakBlock();

    public void PlantSeed();

    public void Attack();
}

internal interface ISprite : IScreenObject
{
    public void SetFrames(LinkAnimationState linkAnimationState);
}