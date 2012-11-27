using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Nuclex.Input;
using Nuclex.UserInterface;
using GameConstructLibrary;
using GraphicsLibrary;
using BEPUphysics;

namespace MapEditor
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Editor : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager mGraphics;
        SpriteBatch mSpriteBatch;

        private FPSCamera mCamera;
        private Dialog mReminder;

        public Editor()
        {

            Content.RootDirectory = "Content";
            mGraphics = new GraphicsDeviceManager(this);
            
            mGraphics.PreferredBackBufferWidth = 1024;
            mGraphics.PreferredBackBufferHeight = 640;

            GameMapEditor.Input = new InputManager(Services, Window.Handle);
            GameMapEditor.GUI = new GuiManager(Services); ;

            Components.Add(GameMapEditor.Input);
            Components.Add(GameMapEditor.GUI);
            GameMapEditor.GUI.DrawOrder = 1000;
            IsMouseVisible = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();

            GraphicsManager.CelShading = GraphicsManager.CelShaded.All;
            GraphicsManager.CastingShadows = true;

            GameMapEditor.Viewport = GraphicsDevice.Viewport;

            GameMapEditor.Screen = new Screen(GameMapEditor.Viewport.Width, GameMapEditor.Viewport.Height);
            GameMapEditor.GUI.Screen = GameMapEditor.Screen;

            mCamera = new FPSCamera(GameMapEditor.Viewport);
            mCamera.Position = new Vector3(0, 140, -100);
            mCamera.Target = new Vector3(0, 100, 0);

            GameMapEditor.Camera = mCamera;

            GameMapEditor.Initialize();


            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsManager.LoadContent(Content, mGraphics.GraphicsDevice, mSpriteBatch);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            GraphicsManager.Update(mCamera);

            GameMapEditor.Update(gameTime);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsManager.BeginRendering();
            GameMapEditor.Render();
            GraphicsManager.FinishRendering();

            GameMapEditor.RenderBox(mGraphics.GraphicsDevice, mSpriteBatch);

            base.Draw(gameTime);
        }
    }
}
