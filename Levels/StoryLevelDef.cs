using Microsoft.Xna.Framework;
using Sprint2.Entities;

namespace Sprint2.Levels;

public sealed class StoryLevelDef
{
    public int Index { get; init; }
    public Vector2 PlayerSpawnPos { get; init; } = new Vector2(5, 5);
    public Platform[] Platforms { get; init; } = [];
}