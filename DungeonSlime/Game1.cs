using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DungeonSlime
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Core systems
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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
            GraphicsDevice.Clear(Color.Yellow);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
