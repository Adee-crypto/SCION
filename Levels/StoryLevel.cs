using Microsoft.Xna.Framework;              
using Sprint2.Entities.Players;
using Sprint2.Entities.Plants;        //  Added for Species
using Sprint2.Util;                   //  Added for PlantUtil
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Levels;

public sealed class StoryLevel : BaseLevel
{
    private readonly StoryLevelDef def;

    public StoryLevel(StoryLevelDef def, Player player) : this(def, player, def.PlayerSpawnPos){}
    public StoryLevel(StoryLevelDef def, Player player, Vector2 playerPos) : base(player)
    {
        this.def = def;
        player.Collider.SetInitialPosition(def.PlayerSpawnPos);
        Reset();
        if (playerPos != Vector2.Zero) {
            player.Collider.SetPosition(playerPos);
        }
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
    }
}