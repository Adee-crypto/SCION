using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Entities.Plants;
using Sprint2.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprint2.Entities;

public enum PlayerState
{
    None,
    Attack,
    PlantSeed,
    BreakBlock,
    Dead
};

public class Player : IPlayer
{
    private PlayerState playerState;
    private PlayerSprite playerSprite;
    private Vector2 position;
    private Vector2 direction;
    public Vector2 velocity;
    private bool isGrounded;
    private int health;
    private float damageTimer;
    private float breakTimer;
    private Aimer aimer;
    public Vector2 AimDirection => aimer.Direction;
    public List<Species> Seeds;

    private bool IsDamaged { get; set; }
    public bool IsBreakable { get; set; }
    public Vector2 Center
    {
        get => position + Consts.playerHitboxSize * Vector2.One / 2f;
    }

    public Vector2 Position
    {
        get => position;
        set { position = value; playerSprite.Position = value; }
    }

    public Rectangle Hitbox
    {
        get => new((int)position.X, (int)position.Y, Consts.playerHitboxSize, Consts.playerHitboxSize);
    }

    public Player()
    {
        Reset();
    }

    public void Reset()
    {
        playerState = PlayerState.None;
        playerSprite = new PlayerSprite();
        position = new Vector2(16, 16);
        direction = new Vector2(1, 0);
        velocity = Vector2.Zero;
        isGrounded = false;
        health = 5;
        IsDamaged = false;
        damageTimer = 0f;
        IsBreakable = false;
        breakTimer = 0f;
        aimer = new Aimer(10f);
        Seeds = [.. Enum.GetValues<Species>().OrderBy(_ => Random.Shared.Next())]; //shuffles seed species order
    }

    public void Move(int direction)
    {
        if (playerState != PlayerState.Dead)
        {
            this.direction.X = direction;
            velocity.X = Consts.playerXSpeed * direction;
        }
    }

    public void Jump()
    {
        if (isGrounded && playerState != PlayerState.Dead)
        {
            isGrounded = false;
            velocity.Y = Consts.playerJumpSpeed;
        }
    }

    public void BreakBlock()
    {
        if (isGrounded && velocity.X == 0 && playerState != PlayerState.Dead)
        {
            playerState = PlayerState.BreakBlock;
        }
    }

    //Add random seed to inventory
    public void GetSeed()
    {
        Seeds.Add(Random.Shared.GetItems(Enum.GetValues<Species>(), 1)[0]);
    }

    public string ThrowSeed()
    {
        //ADD SAFEGUARD FOR EMPTY SEED LIST
        string seedSpecies = Seeds[0].ToString() + " seed";
        Seeds.RemoveAt(0);
        return seedSpecies;
    }

    public void PlantSeed() { }

    public void Attack()
    {
        if (playerState != PlayerState.Dead) playerState = PlayerState.Attack;
    }

    public void Damaged()
    {
        IsDamaged = !IsDamaged;
    }

    public void UpdateMovement(float time, IEnumerable<Rectangle> objects)
    {
        Vector2 movement = Vector2.Zero;
        if (velocity.X != 0) movement.X = velocity.X * time;
        if (velocity.Y >= 0) isGrounded = Collisions.CheckGrounded(this, objects, ref movement);
        if (!isGrounded)
        {
            movement.Y = 0.5f * (2f * velocity.Y + Consts.playerGravity * time) * time;
            velocity.Y += Consts.playerGravity * time;
        }
        else velocity.Y = 0;
        Collisions.ManageCollision(this, objects, movement, ref velocity);
        playerSprite.Position = position;
    }

    public void UpdateBreakBlock(float time)
    {
        if (playerState == PlayerState.BreakBlock)
        {
            breakTimer += time;
            if (breakTimer >= Consts.breakDuration)
            {
                breakTimer = 0f;
                IsBreakable = true;
            }
        }
        else breakTimer = 0f;
    }

    public void UpdateHealth(bool isDamaged, float time)
    {
        if (isDamaged)
        {
            damageTimer += time;
            if (damageTimer >= 1)
            {
                damageTimer = 0f;
                health--;
            }
        }
        if (health == 0) playerState = PlayerState.Dead;
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        UpdateMovement(time, objects);
        UpdateBreakBlock(time);
        UpdateHealth(IsDamaged, time);

        aimer?.Update(Center, Mouse.GetState());
        playerSprite.SetFrames(playerState, direction, velocity, IsDamaged);
        playerSprite.Update(gameTime);

        velocity.X = 0;
        if (playerState != PlayerState.Dead) playerState = PlayerState.None;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //Draw seed in inventory
        for (int i = 0; i < Seeds.Count; i++)
        {
            spriteBatch.Draw(Assets.plantSpritesheet, position + new Vector2(0, -(i + 1) * 16), SourceRects.SeedSourceRects[Seeds[i]][0], Color.White);
        }

        playerSprite.Draw(spriteBatch, Assets.playerTexture);
        aimer?.Draw(spriteBatch, Center);
    }
}