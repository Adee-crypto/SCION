using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Players;
using Sprint2.Entities.Projectiles;
using Sprint2.Levels;
using Sprint2.Managers;
using Sprint2.Util;
using System;

namespace Sprint2.Entities.Enemies;

public enum EnemyState
{
    None,
    Attack,
    Dead
};

public class Enemy : Extensions.IDrawable
{
    private BaseLevel Level {get;set;}
    //States
    private readonly EnemySprite enemySprite;
    private EnemyState state;

    //health
    private int health;
    public int Health => health;

    //physics
    public Collider Collider { get; }
    private Vector2 direction;
    private bool isGrounded;
    private float movementX;
    private float attackCoolDownTimer;
    private float attackDurationTimer;

    public Enemy(BaseLevel level, Vector2 initialPosistion)
    {
        Level = level;
        enemySprite = new EnemySprite();
        Collider = new(initialPosistion, type:ColliderType.Enemy) {Size=Consts.playerHitbox};
        Reset();
    }

    public void Reset()
    {
        enemySprite.Reset();
        state = EnemyState.None;
        Collider.Reset();
        direction = new(1, 0);
        isGrounded = false;
        movementX = 0;
        attackCoolDownTimer = Tunables.EnemyAttackCoolDown.Value;
        attackDurationTimer = 0;
        health = 5;
    }

    public void TakeDamage(int damage)
    {
        if (state == EnemyState.Dead) return;
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            state = EnemyState.Dead;
        }
    }

    public void Move(int direction)
    {
        Collider.SetVelocityX(Tunables.EnemyXForce.Value * direction);
    }

    public void Jump()
    {
        if (isGrounded) Collider.Force += Vector2.UnitY * Tunables.PlayerYForce.Value;
    }

    private void UpdatePatrol()
    {
        if (state != EnemyState.None) return;
        if (Math.Abs(movementX) >= Tunables.EnemyPatrolDistance.Value)
        {
            movementX = 0;
            direction *= -1;
        }
        Move((int)direction.X);
    }

    public void Attack()
    {
        if (state != EnemyState.Dead) state = EnemyState.Attack;
    }

    private void UpdateAttack(float dt, Player player, ProjectileManager projectileManager)
    {
        if (attackDurationTimer > 0)
        {
            attackDurationTimer -= dt;
            Collider.SetVelocityX(0);
            return;
        }

        float distanceX = Collider.Center.X - player.Collider.Center.X;
        float distanceY = Collider.Center.Y - player.Collider.Center.Y;
        bool facing = direction.X > 0 && distanceX < 0 || direction.X < 0 && distanceX > 0;

        if (facing && attackCoolDownTimer >= Tunables.EnemyAttackCoolDown.Value && Math.Abs(distanceX) <= Tunables.EnemyRangeAttackDistance.Value && Math.Abs(distanceY) <= 4)
        {
            attackCoolDownTimer = 0;
            attackDurationTimer = Tunables.EnemyAttackDuration.Value;
            Attack();
            if (Math.Abs(distanceX) > Tunables.EnemyAttackDistance.Value) FireShot(projectileManager);
            else
            {
                Collider.KnockBack(player.Collider);   
                player.TakeDamage(1);
            }
        }
    }

    private void FireShot(ProjectileManager projectileManager)
    {
        Vector2 initialPosition = Collider.Center + Consts.playerHitbox * new Vector2(direction.X * 0.5f, -0.25f);
        Vector2 initialVelocity = new(direction.X * 100f, 0);
        projectileManager.Spawn(ProjectileType.Void, 5f, initialPosition, initialVelocity);
    }

    public void Update(GameTime gameTime)
    {
        if (state == EnemyState.Dead) return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        movementX += dt * Collider.Velocity.X;
        isGrounded = Collider.UpdateMovement(dt, Level.CollisionManager).isGrounded;

        attackCoolDownTimer += dt;
        UpdatePatrol();
        UpdateAttack(dt, Level.Player, Level.ProjectileManager);

        enemySprite.UpdateState(state, direction, Collider.Velocity);
        enemySprite.Update(gameTime);

        if (state != EnemyState.Dead) state = EnemyState.None;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (state == EnemyState.Dead) return;
        enemySprite.Draw(spriteBatch, Collider.Position);
    }
}