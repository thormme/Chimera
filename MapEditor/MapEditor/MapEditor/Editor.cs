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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private InputManager mInput;
        private GuiManager mGUI;

        private MapEditor mMapEditor;

        private Camera mCamera;
        private AnimateModel mModel;

        public Editor()
        {

            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            mInput = new InputManager(Services, Window.Handle);
            mGUI = new GuiManager(Services);

            Components.Add(mInput);
            Components.Add(mGUI);
            mGUI.DrawOrder = 1000;
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

            GraphicsManager.CelShading = true;

            Viewport viewport = GraphicsDevice.Viewport;
            Screen mainScreen = new Screen(viewport.Width, viewport.Height);
            mGUI.Screen = mainScreen;

            mCamera = new Camera(viewport);
            mCamera.Position = new Vector3(0, 40, -100);
            mCamera.Target = new Vector3(0, 40, 0);

            mMapEditor = new MapEditor(mainScreen, mCamera, viewport);

            mModel = new AnimateModel("dude");
            mModel.PlayAnimation("Take 001");

            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsManager.LoadContent(Content, graphics.GraphicsDevice);
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

            GraphicsManager.Update(mCamera);

            mMapEditor.Update(gameTime);
            mModel.Update(gameTime);

            
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsManager.RenderToBackBuffer();

            mModel.Render(new Vector3(0, 0, 0));
            mMapEditor.Render(); ;

            base.Draw(gameTime);
        }
    }
}
