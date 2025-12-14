using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace DungeonSlime
{
    public class DungeonSlimeGame : Core
    {
        private static readonly string GameTitle = "Dungeon Slime";
        private static readonly int WindowWidth = 1280;
        private static readonly int WindowHeight = 720;
        private static readonly bool FullscreenDefault = false;
        private TextureRegion _slime;
        private TextureRegion _bat;

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
            TextureAtlas textureAtlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
            _slime = textureAtlas.GetRegion("slime");
            _bat = textureAtlas.GetRegion("bat");
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

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Draw the slime texture region at a scale of 4.0
            _slime.Draw(SpriteBatch, Vector2.Zero, Color.White, 0.0f, Vector2.One, 4.0f, SpriteEffects.None, 0.0f);

            // Draw the bat texture region 10px to the right of the slime at a scale of 4.0
            _bat.Draw(SpriteBatch, new Vector2(_slime.Width * 4.0f + 10, 0), Color.White, 0.0f, Vector2.One, 4.0f, SpriteEffects.None, 1.0f);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
