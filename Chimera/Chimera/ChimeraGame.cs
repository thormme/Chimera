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
using Chimera.Parts;
using Chimera.Creatures;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities;
using System.Collections.Generic;
using Chimera.Projectiles;
using GameConstructLibrary.Menu;
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Chimera.Menus;
using Nuclex.Input;
using Nuclex.UserInterface;

namespace Chimera
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ChimeraGame : Game
    {
        public static int NumParts = 3;
        
        // TODO: Make this not be the worst thing ever.
        public static ChimeraGame Game
        {
            get;
            protected set;
        }

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
        private KeyInputAction sunUp = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.Down);
        private KeyInputAction sunDown = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.Up);
        private KeyInputAction sunLeft = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.Left);
        private KeyInputAction sunRight = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.Right);
        private static InputAction enterConsoleCommand = new KeyInputAction(Microsoft.Xna.Framework.PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.Enter);
        private static InputAction previousConsoleCommand = new KeyInputAction(Microsoft.Xna.Framework.PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.Up);
        private static InputAction nextConsoleCommand = new KeyInputAction(Microsoft.Xna.Framework.PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.Down);
        bool debugMode = false;

        private Nuclex.UserInterface.Screen mDebugScreen;
        private GuiManager mDebugGUI;
        private InputManager mDebugInput;
        // END

        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch spriteBatch;
        private SpriteFont font;

        private static List<IGameState> mGameStates = new List<IGameState>();
        private static List<IGameState> mGameStateAddQueue = new List<IGameState>();
        private static int mNumPopQueued = 0;

        public static PlayerCreature Player;
        public static ICamera Camera;

        private Sprite mSprite = new Sprite("test_tex");

        public ChimeraGame()
        {

            Game = this;
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.PreferMultiSampling = true;
            //Graphics.ToggleFullScreen();

            Content.RootDirectory = "Content";

            forward = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.W);
            celShading = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.F2);

            // DEBUG
            mDebugScreen = new Nuclex.UserInterface.Screen(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            mDebugGUI = new GuiManager(Services);
            mDebugGUI.Screen = mDebugScreen;
            DebugConsole debugConsole = new DebugConsole();
            mDebugScreen.Desktop.Children.Add(debugConsole);
            mDebugInput = new InputManager(Services, Window.Handle);

            Components.Add(mDebugInput);
            Components.Add(mDebugGUI);
            mDebugGUI.DrawOrder = 1000;

            mDebugScreen.FocusedControl = DebugConsole.ConsoleInput;
            DebugConsole.AddCommand("exit", new DebugConsole.ConsoleCommand(ExitConsoleCommand));
            DebugConsole.AddCommand("enableAllParts", new DebugConsole.ConsoleCommand(EnableParts));
            DebugConsole.AddCommand("partMenu", new DebugConsole.ConsoleCommand(PartMenuCommand));
            DebugConsole.AddCommand("resizeShadowCascade", new DebugConsole.ConsoleCommand(ResizeShadowCascadesCommand));
            DebugConsole.AddCommand("wireframe", new DebugConsole.ConsoleCommand(WireframeConsoleCommand));
            DebugConsole.AddCommand("debug", new DebugConsole.ConsoleCommand(DebugConsoleCommand));
            DebugConsole.AddCommand("visualizeCascades", new DebugConsole.ConsoleCommand(VisualizeCascadesCommand));
            DebugConsole.AddCommand("celShading", new DebugConsole.ConsoleCommand(CelShadingCommand));
            DebugConsole.AddCommand("outlining", new DebugConsole.ConsoleCommand(OutliningCommand));
            DebugConsole.Hide();
            // END

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

        ~ChimeraGame()
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
                GraphicsManager.LoadContent(Content, Graphics.GraphicsDevice, spriteBatch);
                CollisionMeshManager.LoadContent(Content);
                SoundManager.LoadContent(Content);

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

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            float up = sunUp.Active && sunDown.Active ? 0.0f : (sunUp.Active ? 10.0f : (sunDown.Active ? -10.0f : 0.0f));
            float left = sunLeft.Active && sunRight.Active ? 0.0f : (sunLeft.Active ? 10.0f : (sunRight.Active ? -10.0f : 0.0f));
            GraphicsManager.DirectionalLight.Position += new Vector3(up, 0, left);

            // DEBUG STUFF
            if (DebugConsole.IsVisible && enterConsoleCommand.Active)
            {
                DebugConsole.RunEnteredCommand();
            } 
            if (DebugConsole.IsVisible && nextConsoleCommand.Active)
            {
                DebugConsole.NavigateToNextCommand();
            }
            if (DebugConsole.IsVisible && previousConsoleCommand.Active)
            {
                DebugConsole.NavigateToPreviousCommand();
            }
            if (debug.Active)
            {
                if (DebugConsole.IsVisible)
                {
                    DebugConsole.Hide();
                }
                else
                {
                    DebugConsole.Show();
                    mDebugScreen.FocusedControl = DebugConsole.ConsoleInput;
                }
            }

            try
            {
                IsMouseVisible = !InputAction.IsMouseLocked;

                Chimera.ChaseCamera camera = Camera as Chimera.ChaseCamera;

                if (mGameStates.Count > 0 && mGameStates[mGameStates.Count - 1] is PauseState && pause.Active)
                {
                    PopState();
                    if (mGameStates.Count >= 2 && mGameStates[mGameStates.Count - 2] is World)
                    {
                        InputAction.IsMouseLocked = true;
                    }
                }
                else if (mGameStates.Count > 0 && (mGameStates[mGameStates.Count - 1] is World) && pause.Active)
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
            catch (Exception e) 
            {
                TextWriter tw = new StreamWriter("log.txt");
                tw.WriteLine(e.Message);
                tw.WriteLine(e.StackTrace);
                tw.Close();
                throw e;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //try
            //{
                ChimeraGame.Graphics.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

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

                // DEBUG
                if (debugMode && Camera != null)
                {
                    DebugModelDrawer.Draw(ChimeraGame.Camera.GetViewTransform(), ChimeraGame.Camera.GetProjectionTransform());
                }
                // END

                base.Draw(gameTime);
            //}
            //catch (Exception e) 
            //{
            //    TextWriter tw = new StreamWriter("log.txt");
            //    tw.WriteLine(e.Message);
            //    tw.WriteLine(e.StackTrace);
            //    tw.Close();
            //    throw e;
            //}
        }

        /// <summary>
        /// Queue the current state for removal.
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

        #region ConsoleCommands

        private void ExitConsoleCommand(List<string> parameters)
        {
            Exit();
        }

        private void EnableParts(List<string> parameters)
        {
            PlayerInfo info = new PlayerInfo();
            info.AddPart(typeof(CheetahLegs));
            info.AddPart(typeof(KangarooLegs));
            info.AddPart(typeof(RhinoHead));
            info.AddPart(typeof(FrogHead));
            info.AddPart(typeof(EagleWings));
            info.AddPart(typeof(PenguinLimbs));
            Player.Info = info;
        }

        private void PartMenuCommand(List<string> parameters)
        {

            PushState(new PlayerSlotMenu(this, Player));

        }

        private void WireframeConsoleCommand(List<string> parameters)
        {
            debugMode = !debugMode;
        }

        private void DebugConsoleCommand(List<string> parameters)
        {
            GraphicsManager.DebugVisualization = !GraphicsManager.DebugVisualization;
        }

        private void ResizeShadowCascadesCommand(List<string> parameters)
        {
            if (parameters.Count < 2)
            {
                return;
            }
        }

        private void VisualizeCascadesCommand(List<string> parameters)
        {
            GraphicsManager.VisualizeCascades = !GraphicsManager.VisualizeCascades;
        }

        private void CelShadingCommand(List<string> parameters)
        {
            if (parameters.Count > 0)
            {
                if (parameters[0].ToLower() == "all")
                {
                    GraphicsManager.CelShading = GraphicsManager.CelShaded.All;
                }
                else if (parameters[0].ToLower() == "none")
                {
                    GraphicsManager.CelShading = GraphicsManager.CelShaded.None;
                }
                else if (parameters[0].ToLower() == "models")
                {
                    GraphicsManager.CelShading = GraphicsManager.CelShaded.Models;
                }
                else if (parameters[0].ToLower() == "terrain")
                {
                    GraphicsManager.CelShading = GraphicsManager.CelShaded.Terrain;
                }
                else if (parameters[0].ToLower() == "animatemodels")
                {
                    GraphicsManager.CelShading = GraphicsManager.CelShaded.AnimateModels;
                }
            }
        }

        private void OutliningCommand(List<string> parameters)
        {
            if (parameters.Count > 0)
            {
                if (parameters[0].ToLower() == "all")
                {
                    GraphicsManager.Outlining = GraphicsManager.Outlines.All;
                }
                else if (parameters[0].ToLower() == "none")
                {
                    GraphicsManager.Outlining = GraphicsManager.Outlines.None;
                }
                else if (parameters[0].ToLower() == "animatemodels")
                {
                    GraphicsManager.Outlining = GraphicsManager.Outlines.AnimateModels;
                }
            }
        }

        #endregion
    }
}
