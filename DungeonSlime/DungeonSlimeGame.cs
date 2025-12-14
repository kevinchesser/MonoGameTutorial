using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace DungeonSlime
{
    public class DungeonSlimeGame : Core
    {
        private static readonly string GameTitle = "Dungeon Slime";
        private static readonly int WindowWidth = 1280;
        private static readonly int WindowHeight = 720;
        private static readonly bool FullscreenDefault = false;
        private Texture2D _logo;

        //Core systems
        public DungeonSlimeGame() : base(GameTitle, WindowWidth, WindowHeight, FullscreenDefault)
        {

        }

        //Game specific initializations
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize(); //Should never be removed - Graphics device is initialized
        }

        //Load textures, music, sound effects, misc game assets.
        //Called once during startup of the game - called from base.Initialize method
        //Anything dependant on content being loaded here should happen after base.Initialize()
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            _logo = Content.Load<Texture2D>("images/logo");
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);
            SpriteBatch.Begin();
            SpriteBatch.Draw(
                _logo,
                new Vector2(
                    (Window.ClientBounds.Width * 0.5f) - (_logo.Width * 0.5f),
                    (Window.ClientBounds.Height * 0.5f) - (_logo.Height * 0.5f)
                    ),
                 null,               //sourceRectangle -  	An optional region within the texture to be rendered in order to draw only a portion of the texture. Specifying null will render the entire texture.
                 Color.White,        //Color - The color mask (tint) to apply to the image drawn. Specifying Color.White will render the texture with no tint.
                 0.0f,               //Rotation -  	The amount of rotation, in radians, to apply to the texture when rendering. Specifying 0.0f will render the image with no rotation.
                 Vector2.Zero,       //Origin - The X and Y coordinates defining the position of the texture origin. This affects the texture's offset during rendering and serves as a reference point around which the texture is rotated and scaled.
                 1.0f,               //Scale - The scale factor applied to the image across the x- and y-axes. Specifying 1.0f will render the image at its original size with no scaling.
                 SpriteEffects.None, //Effects - A SpriteEffects enum value that specifies if the texture should be rendered flipped across the horizontal axis, the vertical axis, or both axes.
                 0.0f);              //LayerDepth - Specifies the depth at which the texture is rendered. Textures with a higher layer depth value are drawn on top of those with a lower layer depth value. Note: This value will only apply when using SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront.
            SpriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
