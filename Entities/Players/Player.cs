using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Projectiles;
using Sprint2.Extensions;
using Sprint2.HUD;
using Sprint2.Managers;
using Sprint2.Util;
using System;
using System.Collections.Generic;

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
    public Collider Collider { get; } = new(Consts.playerGravity, Consts.playerMass, Vector2.One * Consts.BlockWidth, Vector2.Zero, Consts.playerHitbox);
    private Vector2 direction;

    //States
    private State playerState;
    public bool IsDead => playerState == State.Dead;
    private PlayerSprite playerSprite;
    private bool isGrounded;
    private bool IsDamaged { get; set; }
    public bool IsBreakable { get; set; }
    private float damageTimer;
    private float breakTimer;

    //Stats
    private readonly int maxHealth = 5;
    public int MaxHealth => maxHealth;
    private int health;
    public int Health => health;

    //Aiming
    private Aimer aimer;
    public Vector2 AimDirection => aimer.Direction;

    //Inventory
    public List<ProjectileType> Seeds { get; set; } = [];
    private const int maximumSeedsDrawable = 5;

    public Player()
    {
        Reset();
    }

    public void Reset()
    {
        Collider.Reset();
        playerState = State.None;
        playerSprite = new();
        direction = new(1, 0);
        isGrounded = false;
        health = MaxHealth;
        IsDamaged = false;
        damageTimer = 0f;
        IsBreakable = false;
        breakTimer = 0f;
        aimer = new(10f);
        Seeds.Clear();
        for (int i = 0; i < 5; i++) //prob change this
        {
            GetSeed();
        }
    }

    public void Move(int direction)
    {
        if (isGrounded || this.direction.X != direction)
        {
            this.direction.X = direction;
            Collider.SetVelocityX(Consts.playerXSpeed * direction);
        }
        else if (Math.Abs(Collider.Velocity.X) < Consts.playerXSpeed)
        {
            Collider.SetVelocityX(Consts.playerXSpeed * direction);
        }
    }

    public void Jump()
    {
        if (isGrounded) Collider.SetVelocityY(Consts.playerJumpSpeed);
    }   

    public void BreakBlock()
    {
        if (isGrounded && Collider.Velocity.X == 0) playerState = State.BreakBlock;
    }

    public void GetSeed() //This is pretty inefficient
    {
        Seeds.Add(Plant.SpeciesToProjectile[Random.Shared.GetItems(Enum.GetValues<Species>(), 1)[0]]);
    }

    public ProjectileType ThrowSeed()
    {
        ProjectileType seedSpecies = Seeds[0];
        Seeds.RemoveAt(0);
        return seedSpecies;
    }

    public void ToggleDamaged() => IsDamaged = !IsDamaged;

    public void Attack()
    {
        if (playerState != State.Dead) playerState = State.Attack;
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

    public void UpdateReset()
    {
        if (isGrounded) Collider.SetVelocityX(0);
        else
        {
            if (Math.Abs(Collider.Velocity.X) <= 1.5f) Collider.SetVelocityX(0);
            else if (Collider.Velocity.X < 0) Collider.SetVelocityX(Collider.Velocity.X + 1.5f);
            else Collider.SetVelocityX(Collider.Velocity.X - 1.5f);
        }
        if (playerState != State.Dead) playerState = State.None;
    }

    public void Update(GameTime gameTime, CollisionManager collisionManager)
    {
        if (playerState == State.Dead) return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        this.isGrounded = Collider.Update(dt, collisionManager).isGrounded;
        UpdateBreakBlock(dt);
        UpdateHealth(IsDamaged, dt);

        aimer?.Update(Collider.Center, Mouse.GetState());
        playerSprite.UpdateState(playerState, direction, Collider.Velocity, IsDamaged);
        playerSprite.Update(gameTime);

        UpdateReset();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //Draw seed in inventory
        int drawableAmount = Math.Min(Seeds.Count, maximumSeedsDrawable);
        for (int i = 0; i < drawableAmount; i++)
        {
            spriteBatch.Draw(Assets.BlockSpriteSheet, Collider.Position - Consts.BlockWidth * new Vector2(0, i + 1), SourceRects.ProjectileSourceRects[Seeds[i]][0], Color.White);
        }

        playerSprite.Draw(spriteBatch, Collider.Position);
        string text = $"{Seeds.Count}";
        spriteBatch.DrawString(Assets.UiFont, text, Collider.Position + new Vector2(1, 18), Color.Black, 0f, new(), 0.75f, SpriteEffects.None, 0f);
        aimer?.Draw(spriteBatch, Collider.Center);
    }
}