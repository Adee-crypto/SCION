using Microsoft.Xna.Framework;
using Sprint2.Entities.Enemies;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Players;
using Sprint2.Util;

namespace Sprint2.Levels;

public sealed class StoryLevel : BaseLevel
{
    private readonly StoryLevelDef def;

    public StoryLevel(Player player, StoryLevelDef def) : base(player)
    {
        this.def = def;
        player.Collider.InitialPosition = def.PlayerSpawnPos;
        player.Collider.SetPosition(def.PlayerSpawnPos);
        Reset();
    }

    protected override void BuildLevel()
    {
        foreach (var platform in def.Platforms) {
            Platforms.Add(platform(this));
        }
        foreach (var plant in def.Plants) {
            Plants.Add(plant(this));
        }
        EnemyManager.Spawn(Consts.BlockWidth * new Vector2(40, 24));
    }

    protected override void UpdateLevelLogic(GameTime gameTime)
    {
        
    }
}