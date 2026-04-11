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
        foreach (var platform in def.Platforms) {
            BlockManager.AddRectangleArray(platform);
        }
        foreach (var plant in def.Plants) {
            Plants.Add(plant(BlockManager));
        }
        EnemyManager.Spawn(Consts.BlockWidth * new Vector2(40, 24));

        Sword = null;
        int fallbackPx = -1, fallbackPy = -1, fallbackPw = -1;
        foreach (var (type, px, py, pw, _) in def.Platforms)
        {
            if (type == BlockType.Stone)
            {
                Sword = SpawnOnPlatform(px, py, pw);
                break;
            }
            if (type == BlockType.CrackedStoneBrick && fallbackPx < 0) fallbackPx = px; fallbackPy = py; fallbackPw = pw;            
        }
        if (Sword == null && fallbackPx >= 0) Sword = SpawnOnPlatform(fallbackPx, fallbackPy, fallbackPw);
    }

    private static Sword SpawnOnPlatform(int gridX, int gridY, int gridW)
    {
        float centreX = (gridX + gridW / 2f) * Consts.BlockWidth + Consts.BlockWidth / 2f;
        float surfaceY = (gridY - 1) * Consts.BlockWidth + Consts.BlockWidth / 2f;
        return new Sword(new Vector2(centreX, surfaceY));
    }

    protected override void UpdateLevelLogic(GameTime gameTime) { }
}