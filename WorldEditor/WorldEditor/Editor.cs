#define MAP_EDITOR

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
using GraphicsLibrary;
using GameConstructLibrary;
using WorldEditor.Dialogs;
using System.Windows.Forms;

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

        public Editor()
        {
            IsMouseVisible = true;

            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Form windowForm = (Form)Form.FromHandle(Window.Handle);
            windowForm.ControlBox = false;

            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += ResizedWindow;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GraphicsManager.CelShading = GraphicsManager.CelShaded.All;
            GraphicsManager.Outlining = GraphicsManager.Outlines.All;
            GraphicsManager.CastingShadows = true;

            mCamera = new FPSCamera(GraphicsDevice.Viewport);
            mCamera.Position = new Vector3(0, 1400, 1000);
            mCamera.Target = new Vector3(0, 100, 0);
            mCamera.FarPlaneDistance = 3000;

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
            mWorldEditor = new WorldEditor(GraphicsDevice.Viewport, mCamera, Content);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

            GraphicsManager.Update(mCamera, gameTime);
            mWorldEditor.Update(gameTime, Form.ActiveForm == Form.FromHandle(Window.Handle));

            if (mWorldEditor.Closed)
            {
                Exit();
            }

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
            mWorldEditor.Draw();
            GraphicsManager.FinishRendering();

            base.Draw(gameTime);
        }

        protected void ResizedWindow(object sender, EventArgs e)
        {
            Viewport vp = this.GraphicsDevice.Viewport;

            vp.Width = this.Window.ClientBounds.Width;
            vp.Height = this.Window.ClientBounds.Height;

            this.GraphicsDevice.Viewport = vp;
            GraphicsManager.CreateBuffers();
            this.mCamera.Viewport = vp;
            this.mWorldEditor.Entity.Viewport = vp;
        }
    }
}
