using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprint2;

public class Player : IPlayer
{
    private PlayerUtil.PlayerState playerState;
    private PlayerSprite playerSprite;
    private Aimer aimer;
    private Vector2 position;
    private Vector2 center;
    private Vector2 direction;
    private Vector2 velocity;
    private bool isGrounded;
    private int hp;
    private bool IsDamaged {get; set;}
    private float damageTimer;
    public bool IsBreakable {get; set;}
    private float breakTimer;
    public Vector2 AimDirection => aimer.Direction;
    public Vector2 Center => center;
    public List<Plant.Species> Seeds;

    public Player()
    {
        Reset();
    }

    public void Reset()
    {
        playerState = PlayerUtil.PlayerState.None;
        playerSprite = new PlayerSprite();
        aimer = new Aimer(10f);
        position = new Vector2(16, 16);
        center = new Vector2(position.X + PlayerUtil.hitboxSize / 2f, position.Y + PlayerUtil.hitboxSize / 2f);
        direction = new Vector2(1, 0);
        velocity = Vector2.Zero;
        isGrounded = false;
        hp = 5;
        IsDamaged = false;
        damageTimer = 0f;
        IsBreakable = false;
        breakTimer = 0f;
        Seeds = [.. Enum.GetValues<Plant.Species>().OrderBy(_ => Random.Shared.Next())]; //shuffles seed species order
    }

    public Vector2 Position
    {
        get => position;
        set { position = value; playerSprite.Position = value; }
    }

    public Rectangle Hitbox => new((int)position.X, (int)position.Y, PlayerUtil.hitboxSize, PlayerUtil.hitboxSize);

    public void Move(int direction)
    {
        if (playerState != PlayerUtil.PlayerState.Dead)
        {
            this.direction.X = direction;
            velocity.X = PlayerUtil.horizontalSpeed * direction;
        }
    }

    public void Jump()
    {
        if (isGrounded && playerState != PlayerUtil.PlayerState.Dead)
        {
            isGrounded = false;
            velocity.Y = PlayerUtil.jumpSpeed;
        }
    }

    public void BreakBlock() 
    {
        if (isGrounded && velocity.X == 0 && playerState != PlayerUtil.PlayerState.Dead) playerState = PlayerUtil.PlayerState.BreakBlock;
    }

    public void PlantSeed() { }

    public void Attack()
    {
        if (playerState != PlayerUtil.PlayerState.Dead) playerState = PlayerUtil.PlayerState.Attack;
    }

    public void Damaged()
    {
        IsDamaged = !IsDamaged; // Key.E to toggle for sprint2
    }

    public void UpdateHP(bool isDamaged, float time)
    {
        if (isDamaged)
        {
            damageTimer += time;
            if (damageTimer >= 1)
            {
                damageTimer = 0f;
                hp--;
            }
        }
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {
        if (playerState != PlayerUtil.PlayerState.Dead)
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

            playerSprite.Isdamaged = IsDamaged;
            playerSprite.Position = position;
            playerSprite.SetFrames(playerState, direction, velocity);
            playerSprite.Update(gameTime);

            center = new Vector2(position.X + PlayerUtil.hitboxSize / 2f, position.Y + PlayerUtil.hitboxSize / 2f);
            aimer?.Update(center, Mouse.GetState());

            if (playerState == PlayerUtil.PlayerState.BreakBlock)
            {
                breakTimer += time;
                if (breakTimer >= PlayerUtil.breakDuration)
                {
                    breakTimer = 0f;
                    IsBreakable = true;
                }
            }
            else breakTimer = 0f;

            velocity.X = 0;
            playerState = PlayerUtil.PlayerState.None;

            UpdateHP(IsDamaged, time);
            if (hp == 0)
            {
                playerState = PlayerUtil.PlayerState.Dead;
                playerSprite.SetFrames(playerState, direction, velocity);
                playerSprite.Update(gameTime);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //Draw seeds
        for (int i = 0; i < Seeds.Count; i++)
        {
            spriteBatch.Draw(PlantUtil.spritesheet, position + new Vector2(0, - (i+1)*16), PlantUtil.SeedSpriteRects[Seeds[i]], Color.White);
        }

        playerSprite.Draw(spriteBatch);
        aimer?.Draw(spriteBatch, center);
    }
}