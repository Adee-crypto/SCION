using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For debug
using Sprint2.Entities.Projectiles;
using Sprint2.Util;
using Sprint2.Lib;
using System;
using System.Collections.Generic;
using Sprint2.Entities.Players;

namespace Sprint2.Entities.Enemies;

public enum State
{
    None,
    Attack,
    BreakBlock,
    Dead
};

public class Enemy : Animated, Interfaces.IDrawable, IPhysicsObject
{
    //Motion + Physics
    private Vector2 direction;
    private Vector2 initialPos;
    public Vector2 Position { get; set; }
    public Vector2 Velocity;
    private Vector2 center;
    //actually implement hitbox correctly
    // public Rectangle Hitbox => new((int)Position.X, (int)Position.Y, Consts.playerHitboxSize, Consts.playerHitboxSize);
    
    //States
    private EnemyDef def;
    private State state;
    private EnemySprite sprite = new();
    private bool isGrounded;
    private float PatrolMaxX;
    private float PatrolMinX;

    // TEST FIELDS
    private double shotCooldownLeft;
    private const double shootCooldown = 0.2;
    private bool firedAttackPrev;
    private const float ProjectileSpeed = 100f;
    // END TEST FIELDS

    public Enemy(EnemyDef type, Vector2 spawnPos)
    {
        initialPos = spawnPos;
        def = type;
        Reset();
    }

    public void Reset()
    {
        sprite.Reset();
        // enemySprite = new();
        Position = initialPos;
        center = Position + Consts.playerHitboxSize * Vector2.One * 0.5f;
        direction = new(1, 0);
        Velocity = Vector2.Zero;
        isGrounded = false;
        PatrolMaxX = initialPos.X + def.PatrolDistance;
        PatrolMinX = initialPos.X - def.PatrolDistance;
        shotCooldownLeft = 0;
        firedAttackPrev = false;
    }

    public Rectangle Hitbox => new((int)Position.X, (int)Position.Y, Consts.playerHitboxSize, Consts.playerHitboxSize);

    public void Move(int direction)
    {
        this.direction.X = direction;
        Velocity.X = def.Speed * direction;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            Velocity.Y = Consts.playerJumpSpeed;
        }
    }

    public void Attack()
    {
        state = State.Attack;
    }

    private void PatrolStep()
    {
        int facing = (direction.X >= 0) ? 1 : -1;
        Move(facing);

        if (Position.X >= PatrolMaxX)
        {
            Position = new(PatrolMaxX, Position.Y); // could be simplified if weren't for get;set; nonsense
            Move(-1);
        }
        else if (Position.X <= PatrolMinX)
        {
            Position = new(PatrolMinX, Position.Y); // could be simplified if weren't for get;set; nonsense
            Move(1);
        }
        state = State.None;
    }

    private void AttackStep(Player player, ProjectileManager projectileManager)
    {
        float enemyCenter = Position.X + Consts.playerHitboxSize * 0.5f;
        float playerCenter = player.Position.X + Consts.playerHitboxSize * 0.5f;

        float distance = playerCenter - enemyCenter;

        int facing = (distance >= 0) ? 1 : -1;
        direction.X = facing;

        float range = def.AttackRange;
        float targetCenter = playerCenter - facing * range;
        float difference = targetCenter - enemyCenter;

        const float buffer = 2f;

        if (MathF.Abs(difference) > buffer)
        {
            firedAttackPrev = false;
            state = State.None;
            float newSpeed = def.Speed;
            Velocity.X = newSpeed * MathF.Sign(difference);
            return;
        } 
        
        Velocity.X = 0;
        Attack();

        if (!firedAttackPrev && shotCooldownLeft <= 0)
        {
            FireShot(projectileManager, facing);
            firedAttackPrev = true;
            shotCooldownLeft = shootCooldown;
        }
    }

    private void ReturnStep()
    {
        if (Position.X > PatrolMaxX)
        {
            Move(-1);
        }
        else if (Position.X < PatrolMinX)
        {
            Move(1);
        }
        else
        {
            Velocity.X = 0;
        }
        state = State.None;
    }

    private bool CanSeePlayer(Player player)
    {
        float enemyX = center.X;
        float playerX = player.Center.X;

        float distance = playerX - enemyX;

        bool inFront = (direction.X >= 0 && distance > 0) || (direction.X < 0 && distance < 0);

        bool inView = MathF.Abs(distance) <= def.ViewDistance;

        return inFront && inView;
    }

    private void Decide(Player player, ProjectileManager projectileManager)
    {
        bool seesPlayer = CanSeePlayer(player);

        if (seesPlayer)
        {
            AttackStep(player, projectileManager);
            return;
        }

        firedAttackPrev = false;
        
        if (Position.X > PatrolMaxX || Position.X < PatrolMinX)
        {
            ReturnStep();
            return;
        }

        PatrolStep();
    }

    private void FireShot(ProjectileManager projectileManager, int facing)
    {
        Vector2 spawnPos = center + new Vector2(facing * 8, 0);
        Vector2 shotVelocity = new(facing * ProjectileSpeed, 0);
        ProjectileDef shotDef = new("VoidShot", 5, 100, 0);
        projectileManager.Spawn(shotDef, spawnPos, shotVelocity);
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects, Player player, ProjectileManager projectileManager)
    {
        // FOR TESTING
        shotCooldownLeft -= Time;
        if (shotCooldownLeft < 0) shotCooldownLeft = 0;
        // END TESTING

        Decide(player, projectileManager);

        Vector2 movement = Vector2.Zero;
        if (Velocity.X != 0) movement.X = Velocity.X * Time;
        if (Velocity.Y >= 0) isGrounded = Collisions.CheckGrounded(this, objects, ref movement);
        if (!isGrounded)
        {
            movement.Y = 0.5f * (2f * Velocity.Y + def.Gravity * Time) * Time;
            Velocity.Y += def.Gravity * Time;
        }
        else Velocity.Y = 0;
        Collisions.ManageCollision(this, objects, movement, ref Velocity);

        center = Position + Consts.playerHitboxSize * Vector2.One * 0.5f;
        
        sprite.UpdateState(state, direction, Velocity, false);
        sprite.Update(gameTime);

        Velocity.X = 0;
        if (state != State.Dead) state = State.None;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position);
    }
}