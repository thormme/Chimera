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
using finalProject.Parts;
using finalProject.Creatures;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities;
using System.Collections.Generic;
using finalProject.Projectiles;
using GameConstructLibrary.Menu;
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using finalProject.Menus;

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
        private InputAction pause = new CombinedInputAction(
                new InputAction[]
                {
                    new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.Pause),
                    new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.Escape),
                    new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Buttons.Start)
                },
                InputAction.ButtonAction.Down
            );

        // DEBUG
        private ModelDrawer DebugModelDrawer;
        private KeyInputAction debugGraphics = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.F1);
        private KeyInputAction debug = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.OemTilde);
        private KeyInputAction cheat = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.Tab);
        bool debugMode = false;
        // END

        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch spriteBatch;
        private SpriteFont font;
        public static List<GameTip> Tips;

        private static List<IGameState> mGameStates = new List<IGameState>();
        private static List<IGameState> mGameStateAddQueue = new List<IGameState>();
        private static int mNumPopQueued = 0;

        public static PlayerCreature Player;
        public static ICamera Camera;

        private Sprite mSprite = new Sprite("test_tex");

        public Game1()
        {

            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.PreferMultiSampling = true;
            //Graphics.ToggleFullScreen();

            Content.RootDirectory = "Content";

            forward = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.W);
            celShading = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.F2);


            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, CollisionRules.DefaultDynamicCollisionGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, CollisionRules.DefaultKinematicCollisionGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, Projectile.ProjectileGroup), CollisionRule.NoBroadPhase);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, Sensor.SensorGroup), CollisionRule.NoBroadPhase);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, TerrainPhysics.TerrainPhysicsGroup), CollisionRule.NoBroadPhase);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Sensor.SensorGroup, InvisibleWall.InvisibleWallGroup), CollisionRule.NoBroadPhase);

            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Projectile.SensorProjectileGroup, CollisionRules.DefaultDynamicCollisionGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Projectile.SensorProjectileGroup, CollisionRules.DefaultKinematicCollisionGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Projectile.SensorProjectileGroup, Projectile.ProjectileGroup), CollisionRule.NoBroadPhase);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Projectile.SensorProjectileGroup, Sensor.SensorGroup), CollisionRule.NoBroadPhase);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Projectile.SensorProjectileGroup, Projectile.SensorProjectileGroup), CollisionRule.NoBroadPhase);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(Projectile.SensorProjectileGroup, InvisibleWall.InvisibleWallGroup), CollisionRule.NoBroadPhase);
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

            Tips = new List<GameTip>();

            IntPtr ptr = this.Window.Handle;
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(ptr);
            form.Size = new System.Drawing.Size(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

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
            font = Content.Load<SpriteFont>("font");
            try
            {
                GraphicsManager.LoadContent(this.Content, Graphics.GraphicsDevice, spriteBatch);
                CollisionMeshManager.LoadContent(this.Content);

                ExitToMenu();
            }
            catch (Exception e) 
            {
                TextWriter tw = new StreamWriter("log.txt");
                tw.WriteLine(e.Message);
                tw.WriteLine(e.StackTrace);
                tw.Close();
                throw e;
            }
        }

        public void ExitToMenu()
        {
            mNumPopQueued = mGameStates.Count;
            mGameStateAddQueue.Clear();

            GameMenu menu = new Menus.MainMenu(this, DebugModelDrawer);

            PushState(menu);
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
                GraphicsManager.CelShading = (GraphicsManager.CelShading == GraphicsManager.CelShaded.All) ? GraphicsManager.CelShaded.Models : GraphicsManager.CelShaded.All;
            }

            if (cheat.Active)
            {
                if (mGameStates.Count > 0 && mGameStates[mGameStates.Count - 1] is World)
                {
                    foreach (Entity entity in (mGameStates[mGameStates.Count - 1] as World).Space.Entities)
                    {
                        PlayerCreature player = entity.Tag as PlayerCreature;
                        if (player != null)
                        {
                            player.Damage(100, null);
                            //player.Position = player.SpawnOrigin;
                            int i = 0;
                            //player.AddPart(new RhinoHead(), i++);
                            player.AddPart(new TestingLegs(), i++);
                            player.AddPart(new TestingWings(), i++);
                            //player.AddPart(new TestingWings(), i++);
                            //player.AddPart(new TestingLegs(), i++);
                            player.AddPart(new KangarooLegs(), i++);
                            //player.AddPart(new CheetahLegs(), i++);
                            //player.AddPart(new CheetahLegs(), i++);
                            //player.AddPart(new FrilledLizardHead(), i++);
                            //player.AddPart(new PenguinLimbs(), i++);

                            //(mGameStates[mGameStates.Count - 1] as World).Add(new Turtle(player.Position + 30.0f * player.Forward + Vector3.Up * 5.0f, null));
                            //(mGameStates[mGameStates.Count - 1] as World).Add(new Turtle(player.Position + 32.0f * player.Forward + Vector3.Up * 5.0f, null));
                            //(mGameStates[mGameStates.Count - 1] as World).Add(new Turtle(player.Position + 28.0f * player.Forward + Vector3.Up * 5.0f, null));
                            //(mGameStates[mGameStates.Count - 1] as World).Add(new Turtle(player.Position + 26.0f * player.Forward + Vector3.Up * 5.0f, null));
                            //(mGameStates[mGameStates.Count - 1] as World).Add(new Turtle(player.Position + 24.0f * player.Forward + Vector3.Up * 5.0f, null));
                            //(mGameStates[mGameStates.Count - 1] as World).Add(new Turtle(player.Position + 22.0f * player.Forward + Vector3.Up * 5.0f, null));
                            //(mGameStates[mGameStates.Count - 1] as World).Add(new Turtle(player.Position + 20.0f * player.Forward + Vector3.Up * 5.0f, null));
                            //(mGameStates[mGameStates.Count - 1] as World).Add(new Turtle(player.Position + 18.0f * player.Forward + Vector3.Up * 5.0f, null));
                            //(mGameStates[mGameStates.Count - 1] as World).Add(new Turtle(player.Position + 16.0f * player.Forward + Vector3.Up * 5.0f, null));
                            //(mGameStates[mGameStates.Count - 1] as World).Add(new Turtle(player.Position + 14.0f * player.Forward + Vector3.Up * 5.0f, null));

                            //(mGameStates[mGameStates.Count - 1] as World).Add(new PhysicsProp("rock1light", player.Position + 14.0f * player.Forward + Vector3.Up * 10.0f, Quaternion.Identity, new Vector3(100.0f), 10.0f));
                        }
                    }
                }
            }

            IsMouseVisible = !InputAction.IsMouseLocked;

            FinalProject.ChaseCamera camera = Camera as FinalProject.ChaseCamera;

            if (mGameStates.Count > 0 && mGameStates[mGameStates.Count - 1] is PauseState && pause.Active)
            {
                PopState();
                if (mGameStates.Count >= 2 && mGameStates[mGameStates.Count - 2] is World)
                {
                    InputAction.IsMouseLocked = true;
                }
            }
            else if (pause.Active)
            {
                PushState(new PauseState(this));
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

            InputAction.Update();
            if (mGameStates.Count > 0)
            {
                mGameStates[mGameStates.Count - 1].Update(gameTime);
            }

            if (Camera != null)
            {
                GraphicsManager.Update(Camera, gameTime);
            }
            DebugModelDrawer.Update();

            while (mNumPopQueued > 0)
            {
                mNumPopQueued--;
                mGameStates.RemoveAt(mGameStates.Count - 1);
            }
            foreach (IGameState gameState in mGameStateAddQueue)
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

            Game1.Graphics.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
            GraphicsManager.BeginRendering();

            /*if (mGameStates.Count > 0)
            {
                mGameStates[mGameStates.Count - 1].Render();
            }*/
            foreach (IGameState state in mGameStates)
            {
                state.Render();
            }

            GraphicsManager.FinishRendering();

            RenderTips(gameTime);

            // DEBUG
            if (debugMode && Camera != null)
            {
                DebugModelDrawer.Draw(Game1.Camera.GetViewTransform(), Game1.Camera.GetProjectionTransform());
            }
            // END

            base.Draw(gameTime);
        }

        private void RenderTips(GameTime gameTime)
        {
            int width = Graphics.PreferredBackBufferWidth;
            int height = Graphics.PreferredBackBufferHeight;

            List<GameTip> expiredTips = new List<GameTip>();
            float tipHeight = height - 60.0f;
            spriteBatch.Begin();
            foreach (GameTip tip in Tips)
            {
                tipHeight -= font.LineSpacing * tip.Tips.Length;
                float lineHeight = 0.0f;
                foreach (string line in tip.Tips)
                {
                    spriteBatch.DrawString(font, line, new Vector2(20.0f, tipHeight + lineHeight), Microsoft.Xna.Framework.Color.White);
                    lineHeight += font.LineSpacing;
                }
                
                tipHeight -= font.LineSpacing;
                tip.Time -= (float)(gameTime.ElapsedGameTime.TotalSeconds);
                if (tip.Time <= 0)
                {
                    expiredTips.Add(tip);
                }
            }
            foreach (GameTip expired in expiredTips)
            {
                Tips.Remove(expired);
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Queue the current state fro removal.
        /// </summary>
        /// <returns>Currently running state.</returns>
        public static IGameState PopState()
        {
            mNumPopQueued++;
            return mGameStates[mGameStates.Count - mNumPopQueued];
        }

        /// <summary>
        /// Queue a new state for addition.
        /// </summary>
        /// <param name="gameState">State to be added.</param>
        public static void PushState(IGameState gameState)
        {
            mGameStateAddQueue.Add(gameState);
        }

        public static void AddTip(GameTip tip)
        {
            Tips.Add(tip);
        }
    }
}
