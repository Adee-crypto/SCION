using Microsoft.Xna.Framework;
using Sprint2.Entities.Items;

using Sprint2.Entities.Players;
using Sprint2.Util;

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
        foreach (var platform in def.Platforms)
        {
            BlockManager.AddRectangleArray(platform);
        }

        foreach (var plant in def.Plants)
        {
            Plants.Add(plant(BlockManager));
        }

        EnemyManager.Spawn(Consts.BlockWidth * new Vector2(25, 10));

        Sword.Spawn(Consts.BlockWidth * new Vector2(30, 19));
    }
}