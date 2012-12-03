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

namespace finalProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static int NumParts = 10;

        private InputAction forward;
        private InputAction debug;
        private KeyInputAction debugGraphics;
        private KeyInputAction celShading;
        private KeyInputAction mouseLock;

        private bool debugMode;

        public static GraphicsDeviceManager Graphics;
        private ModelDrawer DebugModelDrawer;
        SpriteBatch spriteBatch;
        private World World;

        public static ICamera Camera;
        Creature creature;
        TerrainPhysics terrain;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            forward = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.W);
            debug = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.OemTilde);
            debugGraphics = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.F1);
            celShading = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.F2);
            mouseLock = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.Tab);

            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, CollisionRules.DefaultDynamicCollisionGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, CollisionRules.DefaultKinematicCollisionGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, Projectile.ProjectileGroup), CollisionRule.NoBroadPhase);

            debugMode = false;
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

            World = new World(DebugModelDrawer);

            GraphicsManager.LoadContent(this.Content, Graphics.GraphicsDevice, this.spriteBatch);
            CollisionMeshManager.LoadContent(this.Content);

            //terrain = new TerrainPhysics("default", new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(), new Vector3(2.5f));
            //World.Add(terrain);

            Vector3 position = new Vector3(-100, -100, 200);

            creature = new Kangaroo(position + new Vector3(20.0f, 1.0f, -20.0f));
            World.Add(creature);

            creature = new Kangaroo(position + new Vector3(10.0f, 1.0f, -20.0f));
            World.Add(creature);

            creature = new Bear(position + new Vector3(0.0f, 1.0f, -20.0f));
            World.Add(creature);

            creature = new Bear(position + new Vector3(0.0f, 1.0f, -10.0f));
            World.Add(creature);

            World.AddLevelFromFile("corner", new Vector3(0, -100, 0), new Quaternion(), new Vector3(8.0f, 0.25f, 8.0f));
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

            if (debugGraphics.Active)
            {
                GraphicsManager.DebugVisualization = (GraphicsManager.DebugVisualization) ? false : true;
            }

            if (celShading.Active)
            {
                GraphicsManager.CelShading = (GraphicsManager.CelShading == GraphicsManager.CelShaded.All) ? GraphicsManager.CelShaded.None : GraphicsManager.CelShaded.All;
            }

            if (mouseLock.Active)
            {
                InputAction.IsMouseLocked = !InputAction.IsMouseLocked;
                foreach (Entity entity in World.Space.Entities)
                {
                    if (entity.Tag is PlayerCreature)
                    {
                        (entity.Tag as PlayerCreature).Damage(100, null);
                    }
                }
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            InputAction.Update();
            World.Update(gameTime);

            GraphicsManager.Update(Camera);
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
           
            GraphicsManager.FinishRendering();

            if (debugMode)
            {
                DebugModelDrawer.Draw(Camera.GetViewTransform(), Camera.GetProjectionTransform());
            }

            base.Draw(gameTime);
        }

        internal PlayerCreature player { get; set; }
    }
}
