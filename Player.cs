using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Interfaces;
using Sprint2.Sprites;
using static Sprint2.Sprites.LinkSprite;


namespace Sprint2;

public class Player : IPlayer
{
    private enum LinkMode
    {
        Still,
        Moving
    };
    private LinkMode linkMode;
    private Vector2 currentDirection;
    private Vector2 currentPosition;
    private LinkSprite linkSprite = new LinkSprite();
    private float speed;

    public Player()
    {
        linkMode = LinkMode.Still;
        currentDirection = new Vector2(LinkUtil.linkDefaultXDirection, 0);
        currentPosition = new Vector2(0, 0);
        speed = LinkUtil.linkSpeed;
    }

    public void ChangeDirection(int index)
    {
        Vector2 direction = (index%4) switch
        {
            0 => new Vector2(0, -1),// up
            1 => new Vector2(0, 1),// down
            2 => new Vector2(-1, 0),// left
            3 => new Vector2(1, 0),// right
            _ => new Vector2(0, 0),// never happen
        };

        // If stopped moving
        if (direction.X == 0 && direction.Y == 0)
        {
            linkMode = LinkMode.Still;
            if (currentDirection.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightFacing);
            if (currentDirection.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftFacing);
            if (currentDirection.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownFacing);
            if (currentDirection.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpFacing);
        }
        //If changing direciton
        else
        {
            currentDirection = direction;
            linkMode = LinkMode.Moving;
            if (direction.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightRunning);
            if (direction.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftRunning);
            if (direction.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownRunning);
            if (direction.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpRunning);
        }
    }

    public void Attack()
    {
        if (currentDirection.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightAttack);
        if (currentDirection.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftAttack);
        if (currentDirection.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownAttack);
        if (currentDirection.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpAttack);
    }

    public void Update(GameTime gameTime)
    {
        if (linkMode == LinkMode.Moving)
        {
            currentPosition += currentDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            linkSprite.Position = currentPosition;
        }
        linkSprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        linkSprite.Draw(spriteBatch);
    }
}