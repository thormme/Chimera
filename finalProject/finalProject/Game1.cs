using BEPUphysics;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysicsDrawer;
using BEPUphysicsDrawer.Models;
using System;
using BEPUphysics.MathExtensions;
using BEPUphysics.Collidables;

namespace finalProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private InputAction forward;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static public World World;
        ModelDrawer mModelDrawer;

        private Camera camera;
        private IMobileObject dude = null;
        private AnimateModel dudeModel = null;

        private bool dudeControlToggle = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            World = new World();

            forward = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.W);
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
            // TODO: Add your initialization logic here
            GraphicsManager.CelShading = true;

            camera = new Camera(graphics.GraphicsDevice.Viewport);
            
            base.Initialize();
        }
        PhysicsObject mp;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            mModelDrawer = new InstancedModelDrawer(this);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            GraphicsManager.LoadContent(this.Content, this.graphics.GraphicsDevice);

            dudeModel = new AnimateModel("dude");
            dudeModel.PlayAnimation("Take 001");

            //World.Add(new PhysicsObject(new InanimateModel("dude"), new BoxShape(1.0f, 1.0f, 1.0f)));

            dude = new PhysicsObject(dudeModel, new CapsuleShape(100.0f, 100.0f));
            World.Add(dude);
            dude.Position = new Vector3(dude.Position.X, 0.0f, dude.Position.Z);
            (dude as PhysicsObject).BecomeKinematic();

            Capsule cap = new Capsule(new Vector3(0, 50, 0), 100f, 30f, 1.0f);
            World.mSpace.Add(cap);
            mModelDrawer.Add(cap);

            //World.Add(mp = new PhysicsObject(dudeModel, new BoxShape(200.0f, 20.0f, 200.0f)));
            //mp.BecomeKinematic();
            //mp.Position = new Vector3(0.0f, -90.0f, 0.0f);
            int xLength = 180;
            int zLength = 180;

            float xSpacing = 8f;
            float zSpacing = 8f;
            var heights = GraphicsManager.LookupTerrainHeightMap("test_level").GetHeights();
            //Create the terrain.
            var terrain = new Terrain(heights, new AffineTransform(
                    new Vector3(xSpacing, 1, zSpacing),
                    new Quaternion(),
                    new Vector3(-xLength * xSpacing / 2, 100, -zLength * zSpacing / 2)));

            //terrain.Thickness = 5; //Uncomment this and shoot some things at the bottom of the terrain! They'll be sucked up through the ground.

            //World.mSpace.Add(terrain);




            //mModelDrawer.Add(terrain);



            TerrainPhysics terr = new TerrainPhysics("test_level", 
                    new Vector3(xSpacing, 1, zSpacing),
                    new Quaternion(),
                    new Vector3(-xLength * xSpacing / 2, -100, -zLength * zSpacing / 2));
            terr.Thickness = 5;
            World.mSpace.Add(terr);
            World.mSpace.Add(new Terrain(terr.Shape, terr.WorldTransform));
            
            mModelDrawer.Add(new Terrain(terr.Shape, terr.WorldTransform));
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            InputAction.Update();
            UpdateCamera(gameTime);

            // TODO: Add your update logic here
            World.Update(gameTime);
            dudeModel.Update(gameTime);

            GraphicsManager.Update(camera);
            mModelDrawer.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            /*GraphicsManager.RenderToShadowMap();

            // TODO: Add your drawing code here
            World.Render();
            dude.Render();

            GraphicsManager.RenderToBackBuffer();

            World.Render();
            dude.Render();*/

            mModelDrawer.Draw(camera.ViewTransform, camera.ProjectionTransform);

            base.Draw(gameTime);
        }

        private void UpdateCamera(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.Milliseconds;

            
            if (dudeControlToggle == true)
            {
                camera.MoveForward(forward.Degree * 0.1f * time);
            }
            else
            {
                dude.Position += forward.Degree * time * 0.1f * dude.XNAOrientationMatrix.Forward;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (dudeControlToggle == true)
                {
                    camera.MoveForward(-0.1f * time);
                }
                else
                {
                    dude.Position += time * -0.1f * dude.XNAOrientationMatrix.Forward;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (dudeControlToggle == true)
                {
                    camera.MoveRight(0.1f * time);
                }
                else
                {
                    Matrix rotate = Matrix.CreateRotationY(MathHelper.ToRadians(time * 0.1f));
                    Matrix newMatrix = dude.XNAOrientationMatrix;
                    newMatrix.Forward = Vector3.Transform(dude.XNAOrientationMatrix.Forward, rotate);
                    dude.XNAOrientationMatrix = newMatrix;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (dudeControlToggle == true)
                {
                    camera.MoveRight(-0.1f * time);
                }
                else
                {
                    Matrix rotate = Matrix.CreateRotationY(MathHelper.ToRadians(time * -0.1f));
                    Matrix newMatrix = dude.XNAOrientationMatrix;
                    newMatrix.Forward = Vector3.Transform(dude.XNAOrientationMatrix.Forward, rotate);
                    dude.XNAOrientationMatrix = newMatrix;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                Vector3 right = Vector3.Cross(Vector3.Up, dude.XNAOrientationMatrix.Forward);
                right.Normalize();
                dude.Position += time * 0.1f * right;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Vector3 right = Vector3.Cross(Vector3.Up, dude.XNAOrientationMatrix.Forward);
                right.Normalize();
                dude.Position += time * -0.1f * right;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                if (dudeControlToggle == false)
                {
                    dudeControlToggle = true;
                }
                else
                {
                    dudeControlToggle = false;
                }
            }

            bool reset = true;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (dudeControlToggle == true)
                {
                    camera.RotatePitch(-0.1f * time);
                }
                else
                {
                    camera.PanPitch(-0.1f * time);
                    reset = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (dudeControlToggle == true)
                {
                    camera.RotatePitch(0.1f * time);
                }
                else
                {
                    camera.PanPitch(0.1f * time);
                    reset = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (dudeControlToggle == true)
                {
                    camera.RotateYaw(0.1f * time);
                }
                else
                {
                    camera.PanYaw(0.1f * time);
                    reset = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (dudeControlToggle == true)
                {
                    camera.RotateYaw(-0.1f * time);
                }
                else
                {
                    camera.PanYaw(-0.1f * time);
                    reset = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                dude.Position = new Vector3(0.0f, 0.0f, 0.0f);
                Matrix newMatrix = dude.XNAOrientationMatrix;
                newMatrix.Forward = new Vector3(0.0f, 0.0f, 1.0f);
                dude.XNAOrientationMatrix = newMatrix;
            }

            if (reset == true)
            {
                camera.ResetPitch();
                camera.ResetYaw();
            }

            if (dudeControlToggle == false)
            {
                camera.Target = dude.Position + new Vector3(0.0f, 75.0f, 0.0f);
                Vector3 direction = dude.XNAOrientationMatrix.Forward;
                direction.Normalize();
                camera.Position = camera.Target - 250.0f * direction;
            }
        }
    }
}
