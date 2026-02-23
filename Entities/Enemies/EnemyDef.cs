using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2;

public class EnemyDef(string enemyID, Texture2D enemyTexture, float enemySpeed = 100f, float enemyGravity = 98f, float enemyPatrolDistance = 128f, float enemyViewDistance = 160f, float enemyAttackRange = 4f)
{
    public string Id { get; } = enemyID;
    public Texture2D Texture { get; } = enemyTexture;
    public float Gravity { get; } = enemyGravity;
    public float Speed { get; } = enemySpeed;
    public float PatrolDistance { get; } = enemyPatrolDistance;
    public float ViewDistance { get; } = enemyViewDistance;
    public float AttackRange { get; } = enemyAttackRange;
}