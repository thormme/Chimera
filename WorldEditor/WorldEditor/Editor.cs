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

namespace WorldEditor.Dialogs
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Editor : GameDeviceControl
    {
        private SpriteBatch mSpriteBatch = null;

        private FPSCamera mCamera = null;
        private MapEditor mWorldEditor = null;
        private ContentManager Content = null;

        public Editor()
        {
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

            Content = new ContentManager(Services, "Content");

            GraphicsManager.CelShading = GraphicsManager.CelShaded.All;
            GraphicsManager.Outlining = GraphicsManager.Outlines.All;
            GraphicsManager.CastingShadows = true;
            GraphicsManager.EnableScreenPicking = true;
            GraphicsManager.WireframeRendering = GraphicsManager.Wireframe.Both;
            GraphicsManager.DebugVisualization = false;

            mCamera = new FPSCamera(GraphicsDevice.Viewport);
            mCamera.Position = new Vector3(28.0f, 25.0f, 29.0f);
            mCamera.Target = new Vector3(27.5f, 24.3f, 28.5f);
            mCamera.FarPlaneDistance = 1500;
            mCamera.NearPlaneDistance = 0.2f;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsManager.Initialize(GraphicsDevice, mSpriteBatch);
            Console.WriteLine(Content.RootDirectory);
            AssetLibrary.LoadContent(GraphicsDevice, Content);
            CollisionMeshManager.LoadContent(GraphicsDevice, Content);
            mWorldEditor = new MapEditor(GraphicsDevice, mCamera, Content, this, Parent as EditorForm);

            ResizedWindow(null, null);
            this.Resize += ResizedWindow;
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
            GraphicsManager.Update(mCamera, gameTime);
            mWorldEditor.Update(gameTime, Form.ActiveForm == this.Parent);

            if (mWorldEditor.Closed)
            {
                //Exit();
            }
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
        }

        protected void ResizedWindow(object sender, EventArgs e)
        {
            var safeWidth = Math.Max(this.Width, 1);
            var safeHeight = Math.Max(this.Height, 1);

            var newViewport = new Viewport(
                0,
                0, 
                safeWidth,
                safeHeight) { MinDepth = 0.0f, MaxDepth = 1.0f };

            var presentationParams = GraphicsDevice.PresentationParameters;
            presentationParams.BackBufferWidth = safeWidth;
            presentationParams.BackBufferHeight = safeHeight;
            presentationParams.DeviceWindowHandle = this.Handle;
            GraphicsDevice.Reset(presentationParams);

            GraphicsDevice.Viewport = newViewport;

            GraphicsManager.OverrideViewport = (Viewport?)newViewport;

            GraphicsManager.CreateBuffers();
            if (mCamera != null)
            {
                (mCamera as FPSCamera).Viewport = newViewport;
            }
        }
    }
}
