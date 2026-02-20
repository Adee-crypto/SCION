using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace starseedPlant
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D texture;
        private IPlantable plant;

        private KeyboardState previousKeyboard;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("plant");

            plant = new VinePlant(texture, new Point(400, 100));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState currentKeyboard = Keyboard.GetState();

            // Detect single space press (edge detection)
            if (currentKeyboard.IsKeyDown(Keys.Space) &&
                previousKeyboard.IsKeyUp(Keys.Space))
            {
                plant.StartVerticalGrowth();
            }

            plant.Update(gameTime);

            previousKeyboard = currentKeyboard;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            plant.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

/*
 * You can change the line plant = new VinePlant(texture, new Point(400, 100));
 * to plant = new CactusPlant(texture, new Point(400, 100)); if you add the animation
 * */


