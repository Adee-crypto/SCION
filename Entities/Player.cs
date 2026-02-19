using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Sprites;
using System.Collections.Generic;

namespace Sprint2;

public class Player : IPlayer
{
    private PlayerUtil.PlayerAction PlayerAction;
    private PlayerSprite playerSprite;
    private Aimer aimer;
    private Vector2 center;

    private Vector2 position;
    private Vector2 velocity;
    private Vector2 direction;
    private bool isGrounded;

    public Player()
    {
        Reset();
    }

    public Vector2 Position
    {
        get => position;
        set { position = value; playerSprite.Position = value; }
    }

    public Rectangle Hitbox => new((int)position.X, (int)position.Y, PlayerUtil.hitboxSize, PlayerUtil.hitboxSize);

    public void Reset()
    {
        PlayerAction = PlayerUtil.PlayerAction.None;
        playerSprite = new PlayerSprite();
        aimer = new Aimer(10f);
        center = Vector2.Zero;
        position = new Vector2(16, 16);
        velocity = Vector2.Zero;
        direction = new Vector2(1, 0);
        isGrounded = false;
    }

    public void Move(int direction)
    {
        this.direction.X = direction;
        velocity.X = PlayerUtil.horizontalSpeed * direction;
    }

    public void MoveLeft() => Move(-1);

    public void MoveRight() => Move(1);

    public void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            velocity.Y = PlayerUtil.jumpSpeed;
        }
    }

    public void BreakBlock() { }

    public void PlantSeed() { }

    public void Attack()
    {
        PlayerAction = PlayerUtil.PlayerAction.Attack;
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 movement = Vector2.Zero;
        if (velocity.X != 0) movement.X = velocity.X * time;
        isGrounded = Collisions.CheckGrounded(this, objects, ref movement);
        if (isGrounded && velocity.Y >= 0) 
            velocity.Y = 0;        
        else
        {
            movement.Y = 0.5f * (2f * velocity.Y + PlayerUtil.gravity * time) * time;
            velocity.Y += PlayerUtil.gravity * time;
        }
        Collisions.ManageCollision(this, objects, movement, ref isGrounded, ref velocity);

        playerSprite.Position = position;
        playerSprite.SetFrames(PlayerAction, direction, velocity);
        playerSprite.Update(gameTime);

        velocity.X = 0;
        PlayerAction = PlayerUtil.PlayerAction.None;

        center = new Vector2(position.X + PlayerUtil.hitboxSize / 2f, position.Y + PlayerUtil.hitboxSize / 2f);
        aimer?.Update(center, Mouse.GetState());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        playerSprite.Draw(spriteBatch);
        aimer?.Draw(spriteBatch, center);
    }
}