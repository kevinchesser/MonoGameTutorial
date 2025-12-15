using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
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
        private AnimatedSprite _slime;
        private AnimatedSprite _bat;

        private Vector2 _slimePosition;

        private const float MOVEMENT_SPEED = 5.0f;

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

            _slime = textureAtlas.CreateAnimatedSprite("slime-animation");
            _slime.Scale = new Vector2(4.0f, 4.0f);
            _bat = textureAtlas.CreateAnimatedSprite("bat-animation");
            _bat.Scale = new Vector2(4.0f, 4.0f);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _slime.Update(gameTime);
            _bat.Update(gameTime);

            //Check for keyboard input and handle it.
            CheckKeyboardInput();

            //Check for gamepad input and handle it
            CheckGamePadInput();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Draw slime sprite
            _slime.Draw(SpriteBatch, _slimePosition);

            //Draw bat sprite to the right of the slime
            _bat.Draw(SpriteBatch, new Vector2(_slime.Width + 10, 0));

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void CheckKeyboardInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            float speed = MOVEMENT_SPEED;
            if(keyboardState.IsKeyDown(Keys.Space))
            {
                speed *= 1.5f;
            }

            if(keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                _slimePosition.Y -= speed;
            }

            if(keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                _slimePosition.Y += speed;
            }

            if(keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                _slimePosition.X -= speed;
            }

            if(keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                _slimePosition.X += speed;
            }
        }

        private void CheckGamePadInput()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            // If the A button is held down, the movement speed increases by 1.5
            // and the gamepad vibrates as feedback to the player.
            float speed = MOVEMENT_SPEED;
            if(gamePadState.IsButtonDown(Buttons.A))
            {
                speed *= 1.5f;
                GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
            }
            else
            {
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
            }

            // Check thumbstick first since it has priority over which gamepad input
            // is movement.  It has priority since the thumbstick values provide a
            // more granular analog value that can be used for movement.
            if(gamePadState.ThumbSticks.Left != Vector2.Zero)
            {
                _slimePosition.X += gamePadState.ThumbSticks.Left.X * speed;
                _slimePosition.Y -= gamePadState.ThumbSticks.Left.Y * speed;
            }
            else
            {
                // If DPadUp is down, move the slime up on the screen.
                if(gamePadState.IsButtonDown(Buttons.DPadUp))
                {
                    _slimePosition.Y -= speed;
                }

                // If DPadDown is down, move the slime down on the screen.
                if(gamePadState.IsButtonDown(Buttons.DPadDown))
                {
                    _slimePosition.Y += speed;
                }

                // If DPapLeft is down, move the slime left on the screen.
                if(gamePadState.IsButtonDown(Buttons.DPadLeft))
                {
                    _slimePosition.X -= speed;
                }

                // If DPadRight is down, move the slime right on the screen.
                if(gamePadState.IsButtonDown(Buttons.DPadRight))
                {
                    _slimePosition.X += speed;
                }
            }
        }
    }
}
