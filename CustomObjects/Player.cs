using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Interfaces;
using Sprint2.Sprites;
using static Sprint2.Sprites.LinkSprite;
using System.Collections.Generic;


namespace Sprint2;

public class Player((int, int) position)// : IPlayer UNCOMMENT THIS
{
    private enum LinkMode {Still, Moving, Attack};
    private LinkMode linkMode = LinkMode.Still;
    private Vector2 direction = new Vector2(LinkUtil.linkDefaultXDirection, 0);
    private Vector2 position = PlantUtil.cellWidth * new Vector2(position.Item1, position.Item2);
    private Vector2 velocity;
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
        get => position;
        set
        {
            position = value;
            linkSprite.Position = value;
        }
    }

    public Rectangle Hitbox => new((int)position.X, (int)position.Y, HitboxSize, HitboxSize);

    //make these nicer/customized
    //e.g. make down break the block below you, make up jump, etc.
    public void up()
    {
        Move(0);
    }
    public void down()
    {
        Move(1);
    }
    public void left()
    {
        Move(2);
    }
    public void right()
    {
        Move(3);
    }

    public void Move(int index)
    {
        linkMode = LinkMode.Moving;
        direction = index switch
        {
            0 => new Vector2(0, -1),// up
            1 => new Vector2(0, 1),// down
            2 => new Vector2(-1, 0),// left
            3 => new Vector2(1, 0),// right
            _ => new Vector2(0, 0),// never happen
        };
        velocity = LinkUtil.walkSpeed * direction;

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
            if (direction.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightRunning);
            if (direction.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftRunning);
            if (direction.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownRunning);
            if (direction.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpRunning);
        // }
    }

    public void Attack()
    {
        linkMode = LinkMode.Attack;
        if (direction.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightAttack);
        if (direction.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftAttack);
        if (direction.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownAttack);
        if (direction.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpAttack);
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
                if (direction.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightFacing);
                if (direction.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftFacing);
                if (direction.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownFacing);
                if (direction.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpFacing);
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

    public void Draw(SpriteBatch spriteBatch) {
        linkSprite.Draw(spriteBatch);
    }
}