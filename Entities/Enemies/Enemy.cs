using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For debug
using Sprint2.Entities.Projectiles;
using Sprint2.Util;
using Sprint2.Lib;
using System;
using System.Collections.Generic;

namespace Sprint2.Entities.Enemies;

public class Enemy : Animated, Interfaces.IDrawable, IPhysicsObject
{
    private PlayerState enemyAction;
    private PlayerSprite enemySprite;
    private Vector2 initialPos;
    public Vector2 Position { get; set; }
    private Vector2 center;
    private Vector2 direction;
    private Vector2 velocity;
    private EnemyDef def;
    private bool isGrounded;
    private float PatrolMaxX;
    private float PatrolMinX;

    // TEST FIELDS
    private State currentState = State.RightFacing;
    private Color color = Color.White;
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
        ResetFrameState(SourceRects.EnemySourceRects[currentState]);
        enemySprite = new PlayerSprite();
        Position = initialPos;
        center = Position + Consts.playerHitboxSize * Vector2.One * 0.5f;
        direction = new Vector2(1, 0);
        velocity = Vector2.Zero;
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
        velocity.X = def.Speed * direction;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            velocity.Y = Consts.playerJumpSpeed;
        }
    }

    public void Attack()
    {
        enemyAction = PlayerState.Attack;
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
        enemyAction = PlayerState.None;
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
            enemyAction = PlayerState.None;
            float newSpeed = def.Speed;
            velocity.X = newSpeed * MathF.Sign(difference);
            return;
        } 
        
        velocity.X = 0;
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
            velocity.X = 0;
        }
        enemyAction = PlayerState.None;
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
        Vector2 spawnPos = new(center.X + (facing * 8), center.Y);
        Vector2 shotVelocity = new(facing * ProjectileSpeed, 0);
        ProjectileDef shotDef = new("VoidShot", 5, 100, 0);
        projectileManager.Spawn(shotDef, spawnPos, shotVelocity);
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects, Player player, ProjectileManager projectileManager)
    {
        UpdateFrameState(gameTime);
        // FOR TESTING

        shotCooldownLeft -= Time;
        if (shotCooldownLeft < 0) shotCooldownLeft = 0;
        // END TESTING

        Decide(player, projectileManager);

        Vector2 movement = Vector2.Zero;
        if (velocity.X != 0) movement.X = velocity.X * Time;
        if (velocity.Y >= 0) isGrounded = Collisions.CheckGrounded(this, objects, ref movement);
        if (!isGrounded)
        {
            movement.Y = 0.5f * (2f * velocity.Y + def.Gravity * Time) * Time;
            velocity.Y += def.Gravity * Time;
        }
        else velocity.Y = 0;
        Collisions.ManageCollision(this, objects, movement, ref velocity);

        enemySprite.SetFrames(enemyAction, direction, velocity, false);
        enemySprite.Update(gameTime);

        center = Position + Consts.playerHitboxSize * Vector2.One * 0.5f;
        
        SetFrames(enemyAction, direction, velocity);
        if (enemyAction != PlayerState.Dead) enemyAction = PlayerState.None;
        velocity.X = 0;
    }

    public void SetFrames(PlayerState enemyAction, Vector2 direction, Vector2 velocity)
    {
        State newState;

        if (enemyAction == PlayerState.None)
        {
            if (velocity.X != 0)
                newState = direction.X == 1 ? State.RightRunning : State.LeftRunning;
            else
                newState = direction.X == 1 ? State.RightFacing : State.LeftFacing;
        }
        else if (enemyAction == PlayerState.Dead)
        {
            newState = State.Dead;
        }
        else if (enemyAction == PlayerState.Attack)
        {
            newState = direction.X == 1 ? State.RightAttack : State.LeftAttack;
        }
        else
        {
            newState = State.RightFacing;
        }

        if (currentState != newState)
        {
            currentState = newState;
            ResetFrameState(SourceRects.EnemySourceRects[currentState]);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(def.Texture, Position, CurrentSourceRect, color);
    }
}