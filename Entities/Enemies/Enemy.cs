using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Players;
using Sprint2.Entities.Projectiles;
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
    //States
    private EnemySprite enemySprite;
    private EnemyState state;

    //physcis
    public Collider Collider { get; }
    private Vector2 direction;
    private bool isGrounded;
    private float movementX;
    private float attackCoolDownTimer;
    private float attackDurationTimer;

    public Enemy(Vector2 initialPosistion)
    {
        enemySprite = new EnemySprite();
        Collider = new(initialPosistion) {Size=Consts.playerHitbox};
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
        attackCoolDownTimer = Consts.enemyAttackCoolDown;
        attackDurationTimer = 0;
    }

    public void Move(int direction)
    {
        Collider.SetVelocityX(Consts.enemyXForce * direction);
    }

    public void Jump()
    {
        if (isGrounded) Collider.SetVelocityY(Consts.playerYForce);
    }

    private void UpdatePatrol()
    {
        if (state != EnemyState.None) return;
        if (Math.Abs(movementX) >= Consts.enemyPatrolDistance)
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

        if (facing && attackCoolDownTimer >= Consts.enemyAttackCoolDown && Math.Abs(distanceX) <= Consts.enemyRangeAttackDistance && Math.Abs(distanceY) <= 4)
        {
            attackCoolDownTimer = 0;
            attackDurationTimer = Consts.enemyAttackDuration;
            Attack();
            if (Math.Abs(distanceX) > Consts.enemyAttackDistance) FireShot(projectileManager);
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

    public void Update(GameTime gameTime, Player player, ProjectileManager projectileManager, CollisionManager collisionManager)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        movementX += dt * Collider.Velocity.X;
        isGrounded = Collider.UpdateMovement(dt, collisionManager).isGrounded;

        attackCoolDownTimer += dt;
        UpdatePatrol();
        UpdateAttack(dt, player, projectileManager);

        enemySprite.UpdateState(state, direction, Collider.Velocity, false);
        enemySprite.Update(gameTime);

        if (state != EnemyState.Dead) state = EnemyState.None;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        enemySprite.Draw(spriteBatch, Collider.Position);
    }
}