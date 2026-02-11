using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
class AnimNonMoveSprite(Texture2D spriteSheet, Rectangle[] data, Vector2 position) : ISprite {
    private int frame = 0;
    private int totalFrames = data.Length;
    private float fps = 10f;
    public Texture2D texture = spriteSheet;
    private Rectangle[] textureData = data;
    private Vector2 pos = position;

    //Add sprite to batch
    public void Draw(SpriteBatch batch)
    {
        Rectangle sourcerect = textureData[frame];
        //Draw centered on pos
        batch.Draw(texture, pos - new Vector2(textureData[frame].Width, textureData[frame].Height)/2, sourcerect, Color.White);
    }

    //Calculate new frame based on time
    public void Update(GameTime gameTime)
    {
        frame = (int)(gameTime.TotalGameTime.TotalSeconds * fps) % totalFrames;
    }
}