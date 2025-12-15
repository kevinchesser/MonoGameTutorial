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
        private Sprite _slime;
        private Sprite _bat;

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
            //Create texture atlas from xml config
            TextureAtlas textureAtlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

            _slime = textureAtlas.CreateSprite("slime");
            _slime.Scale = new Vector2(4.0f, 4.0f);
            _bat = textureAtlas.CreateSprite("bat");
            _bat.Scale = new Vector2(4.0f, 4.0f);
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

            //Draw slime sprite
            _slime.Draw(SpriteBatch, Vector2.Zero);

            //Draw bat sprite to the right of the slime
            _bat.Draw(SpriteBatch, new Vector2(_slime.Width + 10, 0));

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
