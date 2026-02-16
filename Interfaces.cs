using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Sprint2.Sprites.LinkSprite;

namespace Interfaces;

public interface IScreenObject
{
    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects);
    public void Draw(SpriteBatch spriteBatch);
}

public interface ICommand
{
    public void Execute(int index);
    public void Unexecute();
}

public interface IController
{
    public void Update();
}

public interface IEntityStateMachine : IScreenObject
{
    public void ChangeDirection(int index);
    public void Attack();
}

public interface IPlayer : IScreenObject
{
    public void ChangeDirection(int index);
    public void Attack();
}

internal interface ISprite : IScreenObject
{
    public void SetFrames(LinkAnimationState linkAnimationState);
}