using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Util;
namespace Sprint2.Entities.Colliders;

public static class ColliderUtil
{
    //either here or somewhere else add different types per projectile type
    public static Dictionary<ColliderType, Func<Vector2, Vector2, Collider>> Presets {get;} = new()
    {
        { ColliderType.Player, (p, v) => new Collider(p, v, ColliderType.Player) {Mass = Tunables.PlayerMass.Value, Size = Consts.playerHitbox}},
        { ColliderType.Enemy, (p, v) => new Collider(p, v, ColliderType.Enemy) {Mass = Tunables.PlayerMass.Value, Size = Consts.playerHitbox}},
        { ColliderType.Projectile, (p, v) => new Collider(p, v, ColliderType.Projectile) {Mass = Tunables.ProjectileMass.Value, Gravity = Tunables.DefaultProjectileGravity.Value}},
    };
}