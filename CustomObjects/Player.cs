using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Sprites;
using System.Collections.Generic;

namespace Sprint2;

public class Player : IPlayer
{
    private LinkUtil.LinkAction linkAction; // Still, Attack, PlantSeed, BreakBlock
    private LinkSprite linkSprite;
    private Aimer aimer;
    private Vector2 center;

    private Vector2 position;
    private Vector2 velocity;
    private bool isGrounded;
    private bool isMoving;

    public Player()
    {
        linkAction = LinkUtil.LinkAction.Still;
        linkSprite = new LinkSprite();
        aimer = new Aimer(LinkUtil.arrowTexture, 10f);
        center = Vector2.Zero;
        position = new Vector2(16, 16);
        linkSprite.Position = position;
        velocity = new Vector2(LinkUtil.horizontalSpeed, 0);
        isGrounded = false;
        isMoving = false;
    }

    public Vector2 Position
    {
        get => position;
        set { position = value; linkSprite.Position = value; }
    }

    public Rectangle Hitbox => new((int)position.X, (int)position.Y, LinkUtil.hitboxSize, LinkUtil.hitboxSize);

    public void Move(int index)
    {
        isMoving = true;
        velocity = index switch
        {
            0 => new Vector2(LinkUtil.horizontalSpeed * -1f, velocity.Y), // left
            1 => new Vector2(LinkUtil.horizontalSpeed, velocity.Y), // right
            _ => new Vector2(0, 0), // never happen
        };
        if (velocity.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightRunning);
        if (velocity.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftRunning);
    }

    public void MoveLeft() => Move(0);

    public void MoveRight() => Move(1);

    public void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            velocity.Y = LinkUtil.jumpSpeed;
        }
    }

    public void BreakBlock() { }

    public void PlantSeed() { }

    public void Attack()
    {
        linkAction = LinkUtil.LinkAction.Attack;
        if (velocity.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightAttack);
        if (velocity.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftAttack);
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 movement = Vector2.Zero;
        if (isMoving) movement.X = velocity.X * time;
        isGrounded = Collisions.CheckGrounded(this, objects, ref movement);
        if (isGrounded && velocity.Y > 0)
            velocity.Y = 0;
        else
        {
            movement.Y = 0.5f * (2f * velocity.Y + LinkUtil.gravity * time) * time;
            velocity.Y += LinkUtil.gravity * time;
        }
        Collisions.ManageCollision(this, objects, movement, ref isGrounded, ref velocity);
        linkSprite.Position = position;

        if (!isMoving && isGrounded && linkAction == LinkUtil.LinkAction.Still)
        {
            if (velocity.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightFacing);
            if (velocity.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftFacing);
        }
        else if (!isGrounded && linkAction == LinkUtil.LinkAction.Still)
        {
            if (velocity.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightFalling);
            if (velocity.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftFalling);
        }
        linkSprite.Update(gameTime, objects);

        isMoving = false;
        linkAction = LinkUtil.LinkAction.Still;

        center = new Vector2(position.X + LinkUtil.hitboxSize / 2f, position.Y + LinkUtil.hitboxSize / 2f);
        aimer?.Update(center, Mouse.GetState());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        linkSprite.Draw(spriteBatch);
        aimer?.Draw(spriteBatch, center);
    }
}