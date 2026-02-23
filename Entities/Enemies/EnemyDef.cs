using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2;

public class EnemyDef
{
    public string Id { get; }
    public Texture2D Texture { get; }
    public float Gravity { get; }
    public float Speed { get; }
    public float PatrolDistance { get; }
    public float ViewDistance { get; }
    public float AttackRange { get; }

    public EnemyDef(string enemyID, Texture2D enemyTexture, float enemySpeed = 100f, float enemyGravity = 98f, float enemyPatrolDistance = 128f, float enemyViewDistance = 160f, float enemyAttackRange = 4f)
    {
        Id = enemyID;
        Texture = enemyTexture;
        Speed = enemySpeed;
        Gravity = enemyGravity;
        PatrolDistance = enemyPatrolDistance;
        ViewDistance = enemyViewDistance;
        AttackRange = enemyAttackRange;
    }
}