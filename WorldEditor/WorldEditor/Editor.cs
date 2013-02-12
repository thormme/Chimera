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
using GraphicsLibrary;
using GameConstructLibrary;
using WorldEditor.Dialogs;

namespace WorldEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Editor : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager mGraphics = null;
        private SpriteBatch mSpriteBatch = null;

        private FPSCamera mCamera = null;
        private WorldEditor mWorldEditor = null;

        private InputManager mInputManager = null;
        private GuiManager mGUIManager = null;

        public Editor()
        {
            IsMouseVisible = true;

            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GraphicsManager.CelShading = GraphicsManager.CelShaded.Models;
            GraphicsManager.CastingShadows = true;

            mCamera = new FPSCamera(GraphicsDevice.Viewport);
            mCamera.Position = new Vector3(0, 140, -100);
            mCamera.Target = new Vector3(0, 100, 0);

            base.Initialize();
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

            mInputManager = new InputManager(Services, Window.Handle);
            mGUIManager = new GuiManager(Services);
            mGUIManager.DrawOrder = 1000;

            Screen screen = new Screen(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            mGUIManager.Screen = screen;
            mWorldEditor = new WorldEditor(screen, mInputManager, mGUIManager, mCamera);

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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            GraphicsManager.Update(mCamera, gameTime);
            mWorldEditor.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsManager.BeginRendering();
            //WorldEditor.Draw();
            GraphicsManager.FinishRendering();

            base.Draw(gameTime);
        }
    }
}
