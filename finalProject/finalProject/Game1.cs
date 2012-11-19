using BEPUphysics;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysicsDrawer.Models;
using System;

namespace finalProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private InputAction debug;

        private bool debugMode;

        GraphicsDeviceManager graphics;
        public static ModelDrawer DebugModelDrawer;
        SpriteBatch spriteBatch;
        static public World World;

        private InanimateModel boxModel = null;

        private PlayerCreature mPlayer;
        private DummyCreature mDummy;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            World = new World();

            debug = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.OemTilde);

            debugMode = true;
        }

        ~Game1()
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
            // TODO: Add your initialization logic here
            GraphicsManager.CelShading = GraphicsManager.CelShaded.All;
            GraphicsManager.CastingShadows = true;
            GraphicsManager.DebugVisualization = false;
    
            base.Initialize();
        }
        PhysicsObject mp;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            DebugModelDrawer = new InstancedModelDrawer(this);
            DebugModelDrawer.IsWireframe = true;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            GraphicsManager.LoadContent(this.Content, this.graphics.GraphicsDevice, this.spriteBatch);
            CollisionMeshManager.LoadContent(this.Content);

            mPlayer = new PlayerCreature(GraphicsDevice.Viewport, new Vector3(0.0f));
            mDummy = new DummyCreature(new Vector3(15.0f, 0.0f, 15.0f));
            boxModel = new InanimateModel("box");

            World.Add(mPlayer);
            World.Add(mDummy);
            World.Add(mp = new PhysicsObject(boxModel, new Box(new Vector3(0.0f, -70.0f, 0.0f), 200.0f, 20.0f, 200.0f)));
            mp.Entity.BecomeKinematic();

            //World.Add(new TerrainPhysics("test_level", new Vector3(0.0f, -100.0f, 0.0f), new Quaternion(), new Vector3(1.0f)));

            //World.AddLevelFromFile("trial", new Vector3(-100, 0, 0), new Quaternion(), new Vector3(1));
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
            if (debug.Active)
            {
                debugMode = !debugMode;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            InputAction.Update();

            // TODO: Add your update logic here
            World.Update(gameTime);

            DebugModelDrawer.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsManager.BeginRendering();

            World.Render();
            //boxModel.Render(new Vector3(0.0f, 20.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), new Vector3(50.0f, 50.0f, 50.0f));

            GraphicsManager.FinishRendering();

            if (debugMode)
            {
                DebugModelDrawer.Draw((mPlayer.CreatureController as PlayerController).mCamera.ViewTransform, (mPlayer.CreatureController as PlayerController).mCamera.ProjectionTransform);
            }

            base.Draw(gameTime);
        }
    }
}
