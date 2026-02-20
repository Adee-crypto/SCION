using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Sprites;
using System.Collections.Generic;

namespace Sprint2;

public class Player : IPlayer
{
    private PlayerUtil.PlayerAction playerAction;
    private PlayerSprite playerSprite;
    private Aimer aimer;
    private Vector2 position;
    private Vector2 center;
    private Vector2 direction;
    private Vector2 velocity;
    private bool isGrounded;

    /// 

    private float breakTimer = 0f;
    private const float breakDuration = 2f;
    private bool isBreakabke = false;
    public bool IsBreakable
    {
        get => isBreakabke;
        set { isBreakabke = value;}
    }


    ///

    public Player()
    {
        Reset();
    }

    public void Reset()
    {
        playerAction = PlayerUtil.PlayerAction.None;
        playerSprite = new PlayerSprite();
        aimer = new Aimer(10f);
        position = new Vector2(16, 16);
        center = new Vector2(position.X + PlayerUtil.hitboxSize / 2f, position.Y + PlayerUtil.hitboxSize / 2f);
        direction = new Vector2(1, 0);
        velocity = Vector2.Zero;
        isGrounded = false;
    }

    public Vector2 Position
    {
        get => position;
        set { position = value; playerSprite.Position = value; }
    }

    public Rectangle Hitbox => new((int)position.X, (int)position.Y, PlayerUtil.hitboxSize, PlayerUtil.hitboxSize);

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

    public void BreakBlock() 
    {
        playerAction = PlayerUtil.PlayerAction.BreakBlock;
    }

    public void PlantSeed() { }

    public void Attack()
    {
        playerAction = PlayerUtil.PlayerAction.Attack;
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 movement = Vector2.Zero;
        if (velocity.X != 0) movement.X = velocity.X * time;
        if (velocity.Y >= 0) isGrounded = Collisions.CheckGrounded(this, objects, ref movement);
        if (!isGrounded)
        {
            movement.Y = 0.5f * (2f * velocity.Y + PlayerUtil.gravity * time) * time;
            velocity.Y += PlayerUtil.gravity * time;
        }
        else velocity.Y = 0;
        Collisions.ManageCollision(this, objects, movement, ref velocity);

        playerSprite.Position = position;
        playerSprite.SetFrames(playerAction, direction, velocity);
        playerSprite.Update(gameTime);

        velocity.X = 0;

        ///

        if (playerAction == PlayerUtil.PlayerAction.BreakBlock)
        {
            breakTimer += time;
            if (breakTimer >= breakDuration)
            {
                breakTimer = 0f;
                isBreakabke = true;
            }
        }
        else breakTimer = 0f;


        ///

        playerAction = PlayerUtil.PlayerAction.None;

        center = new Vector2(position.X + PlayerUtil.hitboxSize / 2f, position.Y + PlayerUtil.hitboxSize / 2f);
        aimer?.Update(center, Mouse.GetState());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        playerSprite.Draw(spriteBatch);
        aimer?.Draw(spriteBatch, center);
    }
}