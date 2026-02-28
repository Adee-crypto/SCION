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

public class Enemy : Interfaces.IDrawable
{
    //Motion + Physics
    private Vector2 direction;
    public Collider Collider;
    
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
        Collider = new(spawnPos, Consts.playerHitboxSize * Vector2.One);
        def = type;
        Reset();
    }

    public void Reset()
    {
        sprite.Reset();
        Collider.Reset();
        // enemySprite = new();
        direction = new(1, 0);
        isGrounded = false;
        PatrolMaxX = Collider.InitialPos.X + def.PatrolDistance;
        PatrolMinX = Collider.InitialPos.X - def.PatrolDistance;
        shotCooldownLeft = 0;
        firedAttackPrev = false;
    }

    public void Move(int direction)
    {
        this.direction.X = direction;
        Collider.Velocity.X = def.Speed * direction;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            Collider.Velocity.Y = Consts.playerJumpSpeed;
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

        if (Collider.Position.X >= PatrolMaxX)
        {
            Collider.Position.X = PatrolMaxX;
            Move(-1);
        }
        else if (Collider.Position.X <= PatrolMinX)
        {
            Collider.Position.X = PatrolMinX;
            Move(1);
        }
        state = State.None;
    }

    private void AttackStep(Collider playerCollider, ProjectileManager projectileManager)
    {
        float distance = playerCollider.Center.X - Collider.Center.X;

        int facing = (distance >= 0) ? 1 : -1;
        direction.X = facing;

        float range = def.AttackRange;
        float difference = distance - facing * range;

        const float buffer = 2f;

        if (MathF.Abs(difference) > buffer)
        {
            firedAttackPrev = false;
            state = State.None;
            float newSpeed = def.Speed;
            Collider.Velocity.X = newSpeed * MathF.Sign(difference);
            return;
        } 
        
        Collider.Velocity.X = 0;
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
        if (Collider.Position.X > PatrolMaxX)
        {
            Move(-1);
        }
        else if (Collider.Position.X < PatrolMinX)
        {
            Move(1);
        }
        else
        {
            Collider.Position.X = 0;
        }
        state = State.None;
    }

    private bool CanSeePlayer(Player player)
    {
        float enemyX = Collider.Center.X;
        float playerX = player.Collider.Center.X;

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
            AttackStep(player.Collider, projectileManager);
            return;
        }

        firedAttackPrev = false;
        
        if (Collider.Position.X > PatrolMaxX || Collider.Position.X < PatrolMinX)
        {
            ReturnStep();
            return;
        }

        PatrolStep();
    }

    private void FireShot(ProjectileManager projectileManager, int facing)
    {
        Vector2 spawnPos = Collider.Center + new Vector2(facing * 8, 0);
        Vector2 shotVelocity = new(facing * ProjectileSpeed, 0);
        ProjectileDef shotDef = new("VoidShot", 5, 100, 0);
        projectileManager.Spawn(shotDef, spawnPos, shotVelocity);
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects, Player player, ProjectileManager projectileManager)
    {
        // FOR TESTING
        shotCooldownLeft -= sprite.Time;
        if (shotCooldownLeft < 0) shotCooldownLeft = 0;
        // END TESTING

        Decide(player, projectileManager);

        Vector2 movement = Vector2.Zero;
        if (Collider.Velocity.X != 0) movement.X = Collider.Velocity.X * sprite.Time;
        if (Collider.Velocity.Y >= 0) isGrounded = Collisions.CheckGrounded(Collider, objects, ref movement);
        if (!isGrounded)
        {
            movement.Y = 0.5f * (2f * Collider.Velocity.Y + def.Gravity * sprite.Time) * sprite.Time;
            Collider.Velocity.Y += def.Gravity * sprite.Time;
        }
        else Collider.Velocity.Y = 0;
        Collisions.ManageCollision(Collider, objects, movement);
        
        sprite.UpdateState(state, direction, Collider.Velocity, false);
        sprite.Update(gameTime);

        Collider.Velocity.X = 0;
        if (state != State.Dead) state = State.None;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Collider.Position);
    }
}