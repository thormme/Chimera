using BEPUphysics;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace finalProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Space mSpace;

        private Camera camera;
        private AnimateModel dude = null;
        private Terrain testLevel = null;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            mSpace = new Space();

            mSpace.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GraphicsManager.CelShading = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            GraphicsManager.LoadContent(this.Content, this.graphics.GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            UpdateCamera(gameTime);

            // TODO: Add your update logic here
            mSpace.Update();

            if (dude == null)
            {
                camera = new Camera(graphics.GraphicsDevice.Viewport);
                dude = new AnimateModel("dude");
                dude.PlayAnimation("Take 001");
            }

            if (dude.GetType() == typeof(AnimateModel))
            {
                dude.Update(gameTime);
            }

            if (testLevel == null)
            {
                testLevel = new Terrain("test_level");
            }

            GraphicsManager.Update(camera);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            testLevel.Render(new Vector3(0.0f, 0.0f, 0.0f));
            dude.Render(new Vector3(0.0f, 0.0f, 0.0f));

            base.Draw(gameTime);
        }

        private void UpdateCamera(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.Milliseconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camera.RotatePitch(time * 0.1f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camera.RotatePitch(time * -0.1f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camera.RotateYaw(time * -0.1f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camera.RotateYaw(time * 0.1f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                camera.MoveForward(time * 0.1f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                camera.MoveRight(time * -0.1f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                camera.MoveForward(time * -0.1f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                camera.MoveRight(time * 0.1f);
            }
        }
    }
}
