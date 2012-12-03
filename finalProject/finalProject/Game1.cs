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
using finalProject.Parts;
using finalProject.Creatures;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities;
using System.Collections.Generic;

namespace finalProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static int NumParts = 10;

        private InputAction forward;
        private KeyInputAction celShading;
        private KeyInputAction mouseLock;
        private KeyInputAction pause = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.Pause);

        // DEBUG
        private ModelDrawer DebugModelDrawer;
        private KeyInputAction debugGraphics = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.F1);
        private KeyInputAction debug = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.OemTilde);
        bool debugMode = false;
        // END

        public static GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        private static List<GameState> mGameStates = new List<GameState>();
        private static List<GameState> mGameStateAddQueue = new List<GameState>();
        private static bool mPopQueued = false;

        public static ICamera Camera;
        Creature creature;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            forward = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.W);
            celShading = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.F2);
            mouseLock = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.Tab);

            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, CollisionRules.DefaultDynamicCollisionGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, CollisionRules.DefaultKinematicCollisionGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, Projectile.ProjectileGroup), CollisionRule.NoBroadPhase);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, Sensor.SensorGroup), CollisionRule.NoBroadPhase);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, TerrainPhysics.TerrainPhysicsGroup), CollisionRule.NoBroadPhase);
        }

        ~Game1()
        {
            forward.Destroy();
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
            GraphicsManager.CastingShadows = true;
            GraphicsManager.DebugVisualization = false;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            DebugModelDrawer = new InstancedModelDrawer(this);
            DebugModelDrawer.IsWireframe = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            World world = new World(DebugModelDrawer);

            GraphicsManager.LoadContent(this.Content, Graphics.GraphicsDevice, this.spriteBatch);
            CollisionMeshManager.LoadContent(this.Content);
            
            creature = new Rhino(new Vector3(0.0f, 1.0f, -20.0f));
            world.Add(creature);

            world.AddLevelFromFile("jump", new Vector3(0, -100, 0), new Quaternion(), new Vector3(8.0f, 0.25f, 8.0f));

            mGameStates.Add(world);
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
            // DEBUG STUFF
            if (debug.Active)
            {
                debugMode = !debugMode;
            }

            if (debugGraphics.Active)
            {
                GraphicsManager.DebugVisualization = (GraphicsManager.DebugVisualization) ? false : true;
            }
            // END

            if (celShading.Active)
            {
                GraphicsManager.CelShading = (GraphicsManager.CelShading == GraphicsManager.CelShaded.All) ? GraphicsManager.CelShaded.None : GraphicsManager.CelShaded.All;
            }

            if (mouseLock.Active)
            {
                //InputAction.IsMouseLocked = !InputAction.IsMouseLocked;
                if (mGameStates[mGameStates.Count - 1] is World)
                {
                    foreach (Entity entity in (mGameStates[mGameStates.Count - 1] as World).Space.Entities)
                    {
                        if (entity.Tag is PlayerCreature)
                        {
                            (entity.Tag as PlayerCreature).Damage(100, null);
                        }
                    }
                }
            }

            if (pause.Active)
            {
                PushState(new PauseState());
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            InputAction.Update();
            if (mGameStates.Count > 0)
            {
                mGameStates[mGameStates.Count - 1].Update(gameTime);
            }

            GraphicsManager.Update(Camera);
            DebugModelDrawer.Update();

            if (mPopQueued)
            {
                mPopQueued = false;
                mGameStates.RemoveAt(mGameStates.Count - 1);
            }
            foreach (GameState gameState in mGameStateAddQueue)
            {
                mGameStates.Add(gameState);
            }
            mGameStateAddQueue.Clear();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            if (mGameStates.Count > 0)
            {
                mGameStates[mGameStates.Count - 1].Render();
            }

            // DEBUG
            if (debugMode)
            {
                DebugModelDrawer.Draw(Game1.Camera.GetViewTransform(), Game1.Camera.GetProjectionTransform());
            }
            // END

            base.Draw(gameTime);
        }

        /// <summary>
        /// Queue the current state fro removal.
        /// </summary>
        /// <returns>Currently running state.</returns>
        public static GameState PopState()
        {
            mPopQueued = true;
            return mGameStates[mGameStates.Count - 1];
        }

        /// <summary>
        /// Queue a new state for addition.
        /// </summary>
        /// <param name="gameState">State to be added.</param>
        public static void PushState(GameState gameState)
        {
            mGameStateAddQueue.Add(gameState);
        }
    }
}
