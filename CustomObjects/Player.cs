using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Sprites;
using System.Collections.Generic;


namespace Sprint2;

public class Player// : IPlayer UNCOMMENT THIS
{
    private enum LinkMode {Still, Moving, Attack};
    private const int HitboxSize = 16;

    private LinkMode linkMode;
    private LinkSprite linkSprite;

    private Vector2 position;
    private Vector2 velocity;

    public Player()
    {
        linkMode = LinkMode.Still;
        linkSprite = new LinkSprite();
        position = new Vector2(20, 0);
        linkSprite.Position = position;
        velocity = new Vector2(LinkUtil.horizontalSpeed, 0);
    }

    public Vector2 Position
    {
        get => position;
        set { position = value; linkSprite.Position = value; }
    }

    public Rectangle Hitbox => new((int)position.X, (int)position.Y, HitboxSize, HitboxSize);

    //make these nicer/customized
    //e.g. make down break the block below you, make up jump, etc.
    public void left()
    {
        Move(0);
    }
    public void right()
    {
        Move(1);
    }

    public void jump()
    {
        //
    }

    public void breakBlock()
    {
        //
    }

    public void plantSeed()
    {
        // 
    }

    public void Move(int index)
    {
        linkMode = LinkMode.Moving;
        velocity = index switch
        {
            0 => new Vector2(LinkUtil.horizontalSpeed * -1, velocity.Y), // left
            1 => new Vector2(LinkUtil.horizontalSpeed, velocity.Y), // right
            _ => new Vector2(0, 0), // never happen
        };
        if (velocity.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightRunning);
        if (velocity.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftRunning);
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

        Vector2 horizontalMove = Vector2.Zero;
        if (linkMode == LinkMode.Moving)
        {
            horizontalMove = new Vector2(velocity.X, 0) * LinkUtil.horizontalSpeed * time;
            Collisions.ManageCollision(this, objects, horizontalMove);
        } else {
            if (linkMode != LinkMode.Attack) {
                if (velocity.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightFacing);
                if (velocity.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftFacing);
            }
            Collisions.ManageCollision(this, objects, Vector2.Zero);
        }

        velocity.Y += LinkUtil.gravity * time;
        if (velocity.Y > LinkUtil.maxFallSpeed) velocity.Y = LinkUtil.maxFallSpeed;

        float oldY = position.Y;
        position.Y += 0.5f * velocity.Y * time;

        linkSprite.Position = position;

        Rectangle newRect = Hitbox;
        Rectangle oldRect = new Rectangle(newRect.X, (int)oldY, newRect.Width, newRect.Height);

        foreach (Rectangle platform in objects)
        {
            if (!newRect.Intersects(platform)) continue;

            bool falling = velocity.Y > 0;
            bool wasAbove = oldRect.Bottom <= platform.Top;

            if (falling && wasAbove)
            {
                position.Y = platform.Top - newRect.Height;
                velocity.Y = 0;

                linkSprite.Position = position;
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