using System;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
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

        private Vector2 _batPosition;
        private Vector2 _batVelocity;

        //Core systems
        public DungeonSlimeGame() : base(GameTitle, WindowWidth, WindowHeight, FullscreenDefault)
        {

        }

        //Game specific initializations
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize(); //Should never be removed - Graphics device is initialized

            _batPosition = new Vector2(_slime.Width + 10, 0);
            AssignRandomBatVelocity();
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

            // Create a bounding rectangle for the screen.
            Rectangle screenBounds = new Rectangle(
                0,
                0,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight
            );

            // Creating a bounding circle for the slime
            Circle slimeBounds = new Circle(
                (int)(_slimePosition.X + (_slime.Width * 0.5f)),
                (int)(_slimePosition.Y + (_slime.Height * 0.5f)),
                (int)(_slime.Width * 0.5f)
            );

            // Use distance based checks to determine if the slime is within the
            // bounds of the game screen, and if it is outside that screen edge,
            // move it back inside.
            if(slimeBounds.Left < screenBounds.Left)
            {
                _slimePosition.X = screenBounds.Left;
            }
            else if(slimeBounds.Right > screenBounds.Right)
            {
                _slimePosition.X = screenBounds.Right - _slime.Width;
            }

            if(slimeBounds.Top < screenBounds.Top)
            {
                _slimePosition.Y = screenBounds.Top;
            }
            else if(slimeBounds.Bottom > screenBounds.Bottom)
            {
                _slimePosition.Y = screenBounds.Bottom - _slime.Height;
            }

            // Calculate the new position of the bat based on the velocity.
            Vector2 newBatPosition = _batPosition + _batVelocity;

            // Create a bounding circle for the bat.
            Circle batBounds = new Circle(
                (int)(newBatPosition.X + (_bat.Width * 0.5f)),
                (int)(newBatPosition.Y + (_bat.Height * 0.5f)),
                (int)(_bat.Width * 0.5f)
            );

            Vector2 normal = Vector2.Zero;

            // Use distance based checks to determine if the bat is within the
            // bounds of the game screen, and if it is outside that screen edge,
            // reflect it about the screen edge normal.
            if(batBounds.Left < screenBounds.Left)
            {
                normal.X = Vector2.UnitX.X;
                newBatPosition.X = screenBounds.Left;
            }
            else if(batBounds.Right > screenBounds.Right)
            {
                normal.X = -Vector2.UnitX.X;
                newBatPosition.X = screenBounds.Right - _bat.Width;
            }

            if(batBounds.Top < screenBounds.Top)
            {
                normal.Y = Vector2.UnitY.Y;
                newBatPosition.Y = screenBounds.Top;
            }
            else if(batBounds.Bottom > screenBounds.Bottom)
            {
                normal.Y = -Vector2.UnitY.Y;
                newBatPosition.Y = screenBounds.Bottom - _bat.Height;
            }

            // If the normal is anything but Vector2.Zero, this means the bat had
            // moved outside the screen edge so we should reflect it about the
            // normal.
            if(normal != Vector2.Zero)
            {
                normal.Normalize();
                _batVelocity = Vector2.Reflect(_batVelocity, normal);
            }

            _batPosition = newBatPosition;

            if(slimeBounds.Intersects(batBounds))
            {
                // Divide the width and height of the screen into equal columns and
                // rows based on the width and height of the bat.
                int totalColumns = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_bat.Width;
                int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_bat.Height;

                // Choose a random row and column based on the total number of each
                int column = Random.Shared.Next(0, totalColumns);
                int row = Random.Shared.Next(0, totalRows);

                // Change the bat position by setting the x and y values equal to
                // the column and row multiplied by the width and height.
                _batPosition = new Vector2(column * _bat.Width, row * _bat.Height);

                // Assign a new random velocity to the bat
                AssignRandomBatVelocity();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Draw slime sprite
            _slime.Draw(SpriteBatch, _slimePosition);

            //Draw bat sprite to the right of the slime
            _bat.Draw(SpriteBatch, _batPosition);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void AssignRandomBatVelocity()
        {
            // Generate a random angle.
            float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

            // Convert angle to a direction vector.
            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);
            Vector2 direction = new Vector2(x, y);

            // Multiply the direction vector by the movement speed.
            _batVelocity = direction * MOVEMENT_SPEED;
        }

        private void CheckKeyboardInput()
        {
            float speed = MOVEMENT_SPEED;
            if(Input.Keyboard.IsKeyDown(Keys.Space))
            {
                speed *= 1.5f;
            }

            if(Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
            {
                _slimePosition.Y -= speed;
            }

            if(Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
            {
                _slimePosition.Y += speed;
            }

            if(Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
            {
                _slimePosition.X -= speed;
            }

            if(Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
            {
                _slimePosition.X += speed;
            }
        }

        private void CheckGamePadInput()
        {
            GamePadInfo gamePadOne = Input.GamePads[(int)PlayerIndex.One];

            // If the A button is held down, the movement speed increases by 1.5
            // and the gamepad vibrates as feedback to the player.
            float speed = MOVEMENT_SPEED;
            if(gamePadOne.IsButtonDown(Buttons.A))
            {
                speed *= 1.5f;
                gamePadOne.SetVibration(1.0f, TimeSpan.FromSeconds(1));
            }
            else
            {
                gamePadOne.StopVibration();
            }

            // Check thumbstick first since it has priority over which gamepad input
            // is movement.  It has priority since the thumbstick values provide a
            // more granular analog value that can be used for movement.
            if(gamePadOne.LeftThumbStick != Vector2.Zero)
            {
                _slimePosition.X += gamePadOne.LeftThumbStick.X * speed;
                _slimePosition.Y -= gamePadOne.LeftThumbStick.Y * speed;
            }
            else
            {
                // If DPadUp is down, move the slime up on the screen.
                if(gamePadOne.IsButtonDown(Buttons.DPadUp))
                {
                    _slimePosition.Y -= speed;
                }

                // If DPadDown is down, move the slime down on the screen.
                if(gamePadOne.IsButtonDown(Buttons.DPadDown))
                {
                    _slimePosition.Y += speed;
                }

                // If DPapLeft is down, move the slime left on the screen.
                if(gamePadOne.IsButtonDown(Buttons.DPadLeft))
                {
                    _slimePosition.X -= speed;
                }

                // If DPadRight is down, move the slime right on the screen.
                if(gamePadOne.IsButtonDown(Buttons.DPadRight))
                {
                    _slimePosition.X += speed;
                }
            }
        }
    }
}
