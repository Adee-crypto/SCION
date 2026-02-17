using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Sprites;
using System.Collections.Generic;


namespace Sprint2;

public class Player : IPlayer
{
    private enum LinkMode {Still, Moving, Attack};
    private const int HitboxSize = 16;

    private LinkMode linkMode;
    private LinkSprite linkSprite;

    private Vector2 position;
    private Vector2 velocity;
    private bool isGrounded;

    public Player()
    {
        linkMode = LinkMode.Still;
        linkSprite = new LinkSprite();
        position = new Vector2(20, 0);
        linkSprite.Position = position;
        velocity = new Vector2(LinkUtil.horizontalSpeed, 0);
        isGrounded = false;
    }

    public Vector2 Position
    {
        get => position;
        set { position = value; linkSprite.Position = value; }
    }

    public Rectangle Hitbox => new((int)position.X, (int)position.Y, HitboxSize, HitboxSize);

    public void Move(int index)
    {
        linkMode = LinkMode.Moving;
        velocity = index switch
        {
            0 => new Vector2(LinkUtil.horizontalSpeed * -1f, velocity.Y), // left
            1 => new Vector2(LinkUtil.horizontalSpeed, velocity.Y), // right
            _ => new Vector2(0, 0), // never happen
        };
        if (velocity.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightRunning);
        if (velocity.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftRunning);
    }

    public void MoveLeft()
    {
        Move(0);
    }
    public void MoveRight()
    {
        Move(1);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.Y = LinkUtil.jumpSpeed;
            isGrounded = false;
        }
    }

    public void BreakBlock()
    {
        //
    }

    public void PlantSeed()
    {
        // 
    }

    public void Attack()
    {
        linkMode = LinkMode.Attack;
        if (velocity.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightAttack);
        if (velocity.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftAttack);
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (linkMode == LinkMode.Moving)
        {
            Vector2 horizontalMove = new Vector2(velocity.X, 0) * LinkUtil.horizontalSpeed * time;
            Collisions.ManageCollision(this, objects, horizontalMove);
        }
        else
        {
            if (linkMode != LinkMode.Attack)
            {
                if (velocity.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightFacing);
                if (velocity.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftFacing);
            }
            Collisions.ManageCollision(this, objects, Vector2.Zero);
        }

        float oldY = position.Y;
        velocity.Y += LinkUtil.gravity * time;
        position.Y += 0.5f * velocity.Y * time;
        linkSprite.Position = position;

        Rectangle newRect = Hitbox;
        Rectangle oldRect = new Rectangle(newRect.X, (int)oldY, newRect.Width, newRect.Height);

        foreach (Rectangle platform in objects)
        {
            if (!newRect.Intersects(platform)) continue;
            if (oldRect.Bottom <= platform.Top)
            {
                isGrounded = true;
                position.Y = platform.Top - newRect.Height;
                velocity.Y = 0;
            }
            else if (oldRect.Top >= platform.Bottom)
            {
                position.Y = platform.Bottom;
                velocity.Y = 0;
            }
            linkSprite.Position = position;
            newRect = Hitbox;
        }

        linkSprite.Update(gameTime, objects);
        linkMode = LinkMode.Still;
    }

    public void Draw(SpriteBatch spriteBatch) {
        linkSprite.Draw(spriteBatch);
    }
}