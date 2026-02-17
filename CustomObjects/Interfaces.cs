using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2;
using Sprint2.Sprites;
using System.Collections.Generic;
using static Sprint2.Sprites.LinkSprite;

namespace Interfaces;

public interface IScreenObject
{
    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects);
    public void Draw(SpriteBatch spriteBatch);
}

public interface IController
{
    public void Update();
}

public interface IPlayer : IScreenObject
{
    public void Move(int index);

    public void Jump();

    public void BreakBlock();

    public void PlantSeed();

    public void Attack();
}

internal interface ISprite : IScreenObject
{
    public void SetFrames(LinkAnimationState linkAnimationState);
}