using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Plants;

namespace Sprint2.Util;

//Everything here should be set in Game1.LoadContent
public static class Assets
{
    //textures
    public static Texture2D PlayerTexture { get; set; }
    public static Texture2D ArrowTexture { get; set; }
    public static Texture2D BlockSpriteSheet { get; set; }
    public static Texture2D ButtonTexture { get; set; }
    public static Texture2D ResetTexture { get; set; }
    public static Texture2D VoidspawnTexture { get; set; }
    public static Texture2D PixelTexture { get; set; }

    public static SpriteFont UiFont { get; set; }
    
    //SFX
    public static SoundEffect GrassSound { get; set; }

    public static SoundEffect PlantGrowthSFX(Species species) => species switch
    {
        _ => GrassSound
    };
}