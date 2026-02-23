using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Sprites;
using System;
using System.Collections.Generic;

namespace Sprint2;

public class Enemy : IDrawableObject, IPhysicsObject
{
    private PlayerUtil.PlayerState enemyAction;
    private PlayerSprite enemySprite;
    private Vector2 initialPos;
    private Vector2 position;
    private Vector2 center;
    private Vector2 direction;
    private Vector2 velocity;
    private EnemyDef def;
    private bool isGrounded;
    private float PatrolMaxX;
    private float PatrolMinX;

    public Enemy(EnemyDef type, Vector2 spawnPos)
    {
        initialPos = spawnPos;
        def = type;
        Reset();
    }

    public void Reset()
    {
        enemySprite = new PlayerSprite();
        position = initialPos;
        center = new Vector2(position.X + PlayerUtil.hitboxSize / 2f, position.Y + PlayerUtil.hitboxSize / 2f);
        direction = new Vector2(1, 0);
        velocity = Vector2.Zero;
        isGrounded = false;
        PatrolMaxX = initialPos.X + def.PatrolDistance;
        PatrolMinX = initialPos.X - def.PatrolDistance;
    }

    public Vector2 Position
    {
        get => position;
        set { position = value; enemySprite.Position = value; }
    }

    public Rectangle Hitbox => new((int)position.X, (int)position.Y, PlayerUtil.hitboxSize, PlayerUtil.hitboxSize);

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
            velocity.Y = PlayerUtil.jumpSpeed;
        }
    }

    public void Attack()
    {
        enemyAction = PlayerUtil.PlayerState.Attack;
    }

    private void PatrolStep()
    {
        int facing = (direction.X >= 0) ? 1 : -1;
        Move(facing);

        if (position.X >= PatrolMaxX)
        {
            position.X = PatrolMaxX;
            Move(-1);
        }
        else if (position.X <= PatrolMinX)
        {
            position.X = PatrolMinX;
            Move(1);
        }
    }

    private void AttackStep(Player player)
    {
        float distance = player.Position.X - position.X;
        float absDistance = MathF.Abs(distance);

        int facing = (distance >= 0) ? 1 : -1;
        direction.X = facing;

        if (absDistance <= def.AttackRange)
        {
            Attack();
            velocity.X = 0;
        } 
        else
        {
            velocity.X = def.Speed * 1.2f * facing;
        }
    }

    private void ReturnStep()
    {
        if (position.X > PatrolMaxX)
        {
            Move(-1);
        }
        else if (position.X < PatrolMinX)
        {
            Move(1);
        }
        else
        {
            velocity.X = 0;
        }
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

    private void Decide(Player player)
    {
        bool seesPlayer = CanSeePlayer(player);

        if (seesPlayer)
        {
            AttackStep(player);
            return;
        }
        
        if (position.X > PatrolMaxX || position.X < PatrolMinX)
        {
            ReturnStep();
            return;
        }

        PatrolStep();
    }

    public void Update(GameTime gameTime, IEnumerable<Rectangle> objects, Player player)
    {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Decide(player);

        Vector2 movement = Vector2.Zero;
        if (velocity.X != 0) movement.X = velocity.X * time;
        if (velocity.Y >= 0) isGrounded = Collisions.CheckGrounded(this, objects, ref movement);
        if (!isGrounded)
        {
            movement.Y = 0.5f * (2f * velocity.Y + def.Gravity * time) * time;
            velocity.Y += def.Gravity * time;
        }
        else velocity.Y = 0;
        Collisions.ManageCollision(this, objects, movement, ref velocity);

        enemySprite.Position = position;
        enemySprite.SetFrames(enemyAction, direction, velocity);
        enemySprite.Update(gameTime);

        center = new Vector2(position.X + PlayerUtil.hitboxSize / 2f, position.Y + PlayerUtil.hitboxSize / 2f);

        velocity.X = 0;
        enemyAction = PlayerUtil.PlayerState.None;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        enemySprite.Draw(spriteBatch);
    }
}