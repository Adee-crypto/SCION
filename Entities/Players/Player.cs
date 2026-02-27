using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Entities.Plants;
using Sprint2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sprint2.Entities.Players;

public enum State
{
    None,
    Attack,
    BreakBlock,
    Dead
};

public class Player : IPlayer
{
    //Motion + Physics
    private Vector2 direction;
    public Vector2 Position { get; set; }
    private Vector2 velocity;
    public Vector2 Velocity => velocity;
    public Vector2 Center => Position + Consts.playerHitboxSize * Vector2.One / 2f;
    public Rectangle Hitbox => new((int)Position.X, (int)Position.Y, Consts.playerHitboxSize, Consts.playerHitboxSize);
    
    //States
    private State playerState;
    private PlayerSprite playerSprite;
    private bool isGrounded;
    private bool IsDamaged { get; set; }
    public bool IsBreakable { get; set; }
    private int health;
    private float damageTimer;
    private float breakTimer;
    
    //Aiming
    private Aimer aimer;
    public Vector2 AimDirection => aimer.Direction;
    
    //Inventory
    public List<Species> Seeds {get; set;}
    private const int maximumSeedsDrawable = 5;

    public Player() => Reset();

    public void Reset()
    {
        playerState = State.None;
        playerSprite = new();
        Position = new(16, 16);
        direction = new(1, 0);
        velocity = Vector2.Zero;
        isGrounded = false;
        health = 5;
        IsDamaged = false;
        damageTimer = 0f;
        IsBreakable = false;
        breakTimer = 0f;
        aimer = new(10f);
        Seeds = [.. Enum.GetValues<Species>().OrderBy(_ => Random.Shared.Next())]; //shuffles seed species order
    }

    public void Move(int direction)
    {
        if (playerState != State.Dead)
        {
            this.direction.X = direction;
            velocity.X = Consts.playerXSpeed * direction;
        }
    }

    public void Jump()
    {
        if (isGrounded && playerState != State.Dead)
        {
            isGrounded = false;
            velocity.Y = Consts.playerJumpSpeed;
        }
    }

    public void BreakBlock()
    {
        if (isGrounded && velocity.X == 0 && playerState != State.Dead)
        {
            playerState = State.BreakBlock;
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

    public void ToggleDamaged() => IsDamaged = !IsDamaged;

    public void Attack()
    {
        if (playerState != State.Dead) playerState = State.Attack;
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
    }

    public void UpdateBreakBlock(float time)
    {
        if (playerState == State.BreakBlock)
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
        if (health == 0) playerState = State.Dead;
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
    {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        UpdateMovement(time, objects);
        UpdateBreakBlock(time);
        UpdateHealth(IsDamaged, time);

        aimer?.Update(Center, Mouse.GetState());
        playerSprite.UpdateState(playerState, direction, velocity, IsDamaged);
        playerSprite.Update(gameTime);

        velocity.X = 0;
        if (playerState != State.Dead) playerState = State.None;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //Draw seed in inventory
        int drawableAmount = Math.Min(Seeds.Count, maximumSeedsDrawable);
        for (int i = 0; i < drawableAmount; i++)
        {
            spriteBatch.Draw(Assets.PlantSpritesheet, Position + new Vector2(0, -(i + 1) * 16), SourceRects.SeedSourceRects[Seeds[i]][0], Color.White);
        }
        
        playerSprite.Draw(spriteBatch, Position);
        string text = $"{Seeds.Count}";
        spriteBatch.DrawString(Assets.UiFont, text, Position + new Vector2(1, 18), Color.Black, 0f, new Vector2(0, 0), 0.75f, SpriteEffects.None, 0f);
        aimer?.Draw(spriteBatch, Center);
    }
}