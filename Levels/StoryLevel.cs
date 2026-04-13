using Microsoft.Xna.Framework;
using Sprint2.Entities.Items;                   
using Sprint2.Entities.Players;
using Sprint2.Util;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Levels;

public sealed class StoryLevel : BaseLevel
{
    private readonly StoryLevelDef def;

    public StoryLevel(Player player, StoryLevelDef def) : base(player)
    {
        this.def = def;
        player.Collider.SetInitialPosition(def.PlayerSpawnPos);
        player.Collider.SetPosition(def.PlayerSpawnPos);
        Reset();
    }

    protected override void BuildLevel()
    {
        foreach (var platform in def.Platforms)
        {
            BlockManager.AddRectangleArray(platform);
        }

        foreach (var plant in def.Plants)
        {
            Plants.Add(plant(BlockManager));
        }

        EnemyManager.Spawn(Consts.BlockWidth * new Vector2(25, 10));

        Sword.Spawn(Consts.BlockWidth * new Vector2(25, 24));
    }

    protected override void UpdateLevelLogic(GameTime gameTime) { }
}