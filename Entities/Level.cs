using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Controllers;
using Sprint2.Entities;
using Sprint2.Entities.Enemies;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Players;
using Sprint2.Managers;
using Sprint2.UI;
using Sprint2.Util;
using Sprint2.Screens;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sprint2;

public class Level
{
    private bool isPaused;
    private Player Player;
    private List<Platform> platforms;
    private ProjectileManager projectileManager;
    private CollisionManager collisionManager;
    private EnemyManager enemyManager;
    private EnemyDef rangedEnemy;
    //private ScreenManager screenManager; // Not yet finished

    //for testing
    public Plant testPlant { get; private set; }

    public Level(Player player)
    {
        //specific objects (prob will all be deleted and added to level, maybe not player tho)
        Player = player;
        
        //managers
        //screenManager = new ScreenManager(); // Not yet finished
        projectileManager = new(Player);
        collisionManager = new();
        enemyManager = new();
        rangedEnemy = new("Void Spawn", Assets.VoidspawnTexture, 100f, 98f, 96f, 128f, 96f);
        
        Reset();
    }

    public void Reset() {
        Player.Reset();
        enemyManager.Reset();
        projectileManager.Reset();
        collisionManager.Reset();
        enemyManager.Spawn(rangedEnemy, 16 * new Vector2(40, 24));
        testPlant = new ApplePlant((20, 20)); //POTENTIALLY ADD RESET TO PLANT
        platforms = [new(Platform.Type.stonebrick, 0, 16 * 25, 50, 1)];
    }

    public void TogglePause() => isPaused = !isPaused;

    public void Update(GameTime gameTime, (int w, int h) screenSize)
    {
        //screenManager.Update(gameTime); // Not yet finished

        collisionManager.Reset();

        collisionManager.Objects.AddRange(testPlant.GetPlantObjects());
        collisionManager.Objects.AddRange(platforms.Select(p => p.Bounds));

        projectileManager.Update(gameTime, collisionManager); //MUST BE CALLED BEFORE PLAYER UPDATE TO GET VELOCITY
        Player.Update(gameTime, collisionManager);
        enemyManager.Update(gameTime, Player, projectileManager, collisionManager);

        if (Player.IsBreakable)
        {
            if (testPlant.TryRemoveCellBelow(new Vector2(Player.Collider.Hitbox.Center.X, Player.Collider.Hitbox.Bottom)))
            {
                Player.GetSeed();
            }
            Player.IsBreakable = false;
        }

        testPlant.Update(gameTime);

        if (Player.Collider.Position.Y > screenSize.h) Reset();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //screenManager.Draw(spriteBatch); // Not yet finished
        testPlant.Draw(spriteBatch);
        projectileManager.Draw(spriteBatch);
        enemyManager.Draw(spriteBatch);
        platforms.ForEach(p => p.Draw(spriteBatch));
        Player.Draw(spriteBatch);
    }
}