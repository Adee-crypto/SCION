using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Interfaces;
using Sprint2.Sprites;
using static Sprint2.Sprites.LinkSprite;
using System.Collections.Generic;


namespace Sprint2;

public class Player : IPlayer
{
    private enum LinkMode {Still, Moving, Attack};
    private LinkMode linkMode;
    private Vector2 currentDirection;
    private Vector2 currentPosition;
    private LinkSprite linkSprite = new LinkSprite();
    private float speed;
    private const float Gravity = 1800f;
    private const float MaxFallSpeed = 3600f;
    private const int HitboxSize = 16;
    private float velocityY = 0f;
    private bool isOnPlatform;


    public Player()
    {
        linkMode = LinkMode.Still;
        currentDirection = new Vector2(LinkUtil.linkDefaultXDirection, 0);
        currentPosition = new Vector2(0, 0);
        speed = LinkUtil.linkSpeed;
    }

    public Vector2 Position
    {
        get => currentPosition;
        set
        {
            currentPosition = value;
            linkSprite.Position = value;
        }
    }

    public Rectangle Hitbox => new((int)currentPosition.X, (int)currentPosition.Y, HitboxSize, HitboxSize);

    //make these nicer/customized
    public void up()
    {
        ChangeDirection(0);
    }

    public void down()
    {
        ChangeDirection(1);
    }
    public void left()
    {
        ChangeDirection(2);
    }
    public void right()
    {
        ChangeDirection(3);
    }

    public void ChangeDirection(int index)
    {
        linkMode = LinkMode.Moving;
        Vector2 direction = index switch
        {
            0 => new Vector2(0, -1),// up
            1 => new Vector2(0, 1),// down
            2 => new Vector2(-1, 0),// left
            3 => new Vector2(1, 0),// right
            _ => new Vector2(0, 0),// never happen
        };

        // If stopped moving
        // if (direction.X == 0 && direction.Y == 0)
        // {
        //     linkMode = LinkMode.Still;
        //     if (currentDirection.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightFacing);
        //     if (currentDirection.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftFacing);
        //     if (currentDirection.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownFacing);
        //     if (currentDirection.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpFacing);
        // }
        //If changing direciton
        // else
        // {
            currentDirection = direction;
            if (direction.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightRunning);
            if (direction.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftRunning);
            if (direction.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownRunning);
            if (direction.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpRunning);
        // }
    }

    public void Attack()
    {
        linkMode = LinkMode.Attack;
        if (currentDirection.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightAttack);
        if (currentDirection.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftAttack);
        if (currentDirection.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownAttack);
        if (currentDirection.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpAttack);
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 horizontalMove = Vector2.Zero;
        if (linkMode == LinkMode.Moving)
        {
            horizontalMove = new Vector2(currentDirection.X, 0) * speed * time;
            Collisions.ManageCollision(this, objects, horizontalMove);
        } else {
            if (linkMode != LinkMode.Attack) {
                if (currentDirection.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightFacing);
                if (currentDirection.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftFacing);
                if (currentDirection.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownFacing);
                if (currentDirection.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpFacing);
            }
            Collisions.ManageCollision(this, objects, Vector2.Zero);
        }

        velocityY += Gravity * time;
        if (velocityY > MaxFallSpeed) velocityY = MaxFallSpeed;

        isOnPlatform = false;

        float oldY = currentPosition.Y;
        currentPosition.Y += velocityY * time;

        linkSprite.Position = currentPosition;

        Rectangle newRect = Hitbox;
        Rectangle oldRect = new Rectangle(newRect.X, (int)oldY, newRect.Width, newRect.Height);

        foreach (Rectangle platform in objects)
        {
            if (!newRect.Intersects(platform)) continue;

            bool falling = velocityY > 0;
            bool wasAbove = oldRect.Bottom <= platform.Top;

            if (falling && wasAbove)
            {
                currentPosition.Y = platform.Top - newRect.Height;
                velocityY = 0;
                isOnPlatform = true;

                linkSprite.Position = currentPosition;
                newRect = Hitbox;
            }
        }

        linkSprite.Update(gameTime, objects);
        linkMode = LinkMode.Still;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        linkSprite.Draw(spriteBatch);
    }
}