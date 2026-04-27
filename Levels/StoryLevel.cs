using Microsoft.Xna.Framework;
using Sprint2.Entities.Items;
using Sprint2.Entities.Players;
using Sprint2.Entities.Plants;        //  Added for Species
using Sprint2.Util;                   //  Added for PlantUtil
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
        // Add all blocks using the existing Add method
        foreach (var (type, x, y) in def.Blocks)
        {
            BlockManager.Add((x, y), type);
        }

        // Add all plants
        foreach (var (species, x, y) in def.Plants)
        {
            Plants.Add(PlantUtil.SpeciesToPlantInit[species](BlockManager, (x, y)));
        }

        // Special spawns
        EnemyManager.Spawn(Consts.BlockWidth * new Vector2(25, 10));
        Sword.Spawn(Consts.BlockWidth * new Vector2(30, 19));
    }

    protected override void UpdateLevelLogic(GameTime gameTime) { }
}