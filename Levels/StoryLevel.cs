using Microsoft.Xna.Framework;
using Sprint2.Entities.Enemies;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Players;
using Sprint2.Util;

namespace Sprint2.Levels;

public sealed class StoryLevel : BaseLevel
{
    private readonly StoryLevelDef def;
    private readonly EnemyDef rangedEnemy;

    public StoryLevel(Player player, StoryLevelDef def) : base(player)
    {
        this.def = def;

        rangedEnemy = new EnemyDef("Void Spawn", Assets.VoidspawnTexture, 100, 98, 96, 128, 96);

        Reset();
    }

    protected override void BuildLevel()
    {
        foreach (var platform in def.Platforms) {
            Platforms.Add(platform(this));
        }
        Plants.Add(new ApplePlant(this, (20, 20)));
        EnemyManager.Spawn(rangedEnemy, Consts.BlockWidth * new Vector2(40, 24));
    }

    protected override void UpdateLevelLogic(GameTime gameTime)
    {

    }
}