using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Entities.Colliders;
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

public enum Item
{
    Sword,
    Seed
}

public class Player : IPlayer
{
    //Motion + Physics
    public Collider Collider { get; } = ColliderUtil.Presets[ColliderType.Player](new(), new());
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
    private readonly int maxHealth = Tunables.PlayerMaxHealth.Value;
    public int MaxHealth => maxHealth;
    private int health;
    public int Health => health;

    //Aiming
    private Aimer aimer;
    public Vector2 AimDirection => aimer.Direction;

    //Inventory
    public List<ProjectileType> Seeds { get; set; } = [];
    private const int maximumSeedsDrawable = 5;
    private Item item;

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
        for (int i = 0; i < 5; i++) GetRandomSeed(); //prob change this
        item = Item.Seed;
    }

    public void ChangeItem(int num)
    {
        if (num == 1) item = Item.Sword;
        else item = Item.Seed;
    }

    public void TakeDamage(int damage) // could define damage
    {
        IsDamaged = true;
        health -= damage;
    }

    public void Move(int direction)
    {
        float xForce;
        if (isGrounded) {
            xForce = Tunables.PlayerGroundXForce.Value * (Tunables.PlayerTargetWalkVelocity.Value - Math.Abs(Collider.Velocity.X));
        } else {
            xForce = Tunables.PlayerAirXForce.Value;
        }
        Collider.Force += Vector2.UnitX * direction * xForce;
        this.direction.X = direction;
    }

    public void TryJump() {
        if (isGrounded) Collider.Force += Vector2.UnitY * Tunables.PlayerYForce.Value;
    }   

    public void TryBreakBlock() {
        if (isGrounded && Collider.Velocity.X == 0) playerState = State.BreakBlock;
    }

    public void GetRandomSeed() => GetSeed(PlantUtil.RandomSpecies());
    
    public void GetSeed(Species type) => Seeds.Add(PlantUtil.SpeciesToProjectile[type]);

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
            if (breakTimer >= Tunables.BreakDuration.Value)
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

    public void Update(GameTime gameTime, CollisionManager collisionManager)
    {
        if (playerState == State.Dead) return;
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        isGrounded = Collider.UpdateMovement(dt, collisionManager).isGrounded;
        UpdateBreakBlock(dt);
        UpdateHealth(IsDamaged, dt);

        if (item == Item.Seed) aimer.Update(Collider.Center, Mouse.GetState());
        playerSprite.Update(gameTime, playerState, direction, Collider.Velocity, IsDamaged);

        IsDamaged = false;
        Collider.UpdatePlayerVelocity(isGrounded, dt);
        if (playerState != State.Dead) playerState = State.None;
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
        if (item == Item.Seed) aimer?.Draw(spriteBatch, Collider.Center);
    }
}