using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenGL_Game.Animations;
using OpenGL_Game.Enemies.Drone;

namespace OpenGL_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

public class MyGame : CoolGameBase
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        HUD hud;
        bool lightDir = true;
        public static Vector3 cameraPos = new Vector3(0.0f,-0.8f,0.0f);
        float facing = -1.5708f;
        float mouseSpeed = 0.0f;
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        Drone drone;
        Key key1, key2, key3;
        public static bool wonGame;
        public static int keys;
        Portal port;
        int[,] LevelStructure;
        bool CollisionsON = true, keydown;
        bool droneDeactivate, keydown2 = false;

        public static MyGame myGameInstance;
        Vector3 playerPos = new Vector3(0,-1.0f, 0.0f);

       

        public MyGame() : base()
        {
            myGameInstance = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "";
            LevelStructure = new int[13,13]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0},
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                { 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0},
                { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0},
                { 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0},
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                { 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0},
                { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0},
                { 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0},
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                { 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };
        }

        private void addUI()
        {

        }

        private void CreateEntities()
        {
            //gets references to the gameplay elements creates from file
            key1 = new Key(entityManager.FindEntity("Key1"));
            key2 = new Key(entityManager.FindEntity("Key2"));
            key3 = new Key(entityManager.FindEntity("Key3"));
            port = new Portal(entityManager.FindEntity("Portal"));


            //UI
            addUI();
            hud = new HUD(ref entityManager);

            //walls, floor, skybox
            var gameMap = new WallDraw();
            entityManager.AddEntities(gameMap.MakeMap());

            //drone
            drone = new Drone(entityManager, entityManager.FindEntity("Drone"), 10f);
        }

        private void CreateSystems()
        {
            ISystem systemRen;
            systemRen = new SystemRender(Content.Load<Effect>("Shaders/shaderGAME.mgfx"));
            systemManager.AddSystem(systemRen);

            ISystem systemPhy;
            systemPhy = new SystemPhysics();
            systemManager.AddSystem(systemPhy);
            
            ISystem systemAud;
            systemAud = new SystemAudio();
            systemManager.AddSystem(systemAud);

            ISystem systemAnim = new SystemAnimation();
            systemManager.AddSystem(systemAnim);

            ISystem systemRenUI;
            systemRenUI = new SystemRenderUI(Content.Load<Effect>("Shaders/shaderUI.mgfx"));
            systemManager.AddSystem(systemRenUI);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            LoadConfiguration("GameConfiguration.xaml");

            view = Matrix.CreateLookAt(playerPos, new Vector3(playerPos.X, playerPos.Y, playerPos.Z - 3.0f), new Vector3(0, 1, 0));
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 1280f / 720f, 0.01f, 1000f);
            Mouse.SetPosition(1280 / 2, 720 / 2);
            CreateEntities();
            CreateSystems();
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            base.Initialize();
            CoolGameBase.sp1 = new Vector3(4.5f,0.5f,-35.0f);
            CoolGameBase.sp2 = new Vector3(-4.5f, 0.5f, -32.0f);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            //endgame scenarios
            if (HUD.health <= 0)
                GameOver(false);

            if (wonGame)
                GameOver(true);
            base.Update(gameTime);

            Console.Clear();
            //get input states if game has focus
            if (myGameInstance.IsActive)
            {
                KeyboardState state = Keyboard.GetState();
                MouseState st = Mouse.GetState();
                Point mousepos = st.Position;
                //reposition mouse to center of the screen
                Mouse.SetPosition(1280 / 2, 720 / 2);
                //get how much it has moved for rotation
                int XDelta = mousepos.X - 1280 / 2;

                //keyboard movement
                if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
                {
                    if (CollisionsON)
                    {
                        float x = cameraPos.X;
                        float z = cameraPos.Z;
                        bool coll = checkCollision(new Vector2(x += 4 * ((float)Math.Cos(facing) * 0.1f), z += 4 * ((float)Math.Sin(facing) * 0.1f)));
                        if (!coll)
                        {
                            cameraPos.X += (float)Math.Cos(facing) * 0.1f;
                            cameraPos.Z += (float)Math.Sin(facing) * 0.1f;
                        }

                    }
                    else
                    {
                        cameraPos.X += (float)Math.Cos(facing) * 0.1f;
                        cameraPos.Z += (float)Math.Sin(facing) * 0.1f;
                    }
                }
                if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
                {
                    if (CollisionsON)
                    {
                        float x = cameraPos.X;
                        float z = cameraPos.Z;
                        bool coll = checkCollision(new Vector2(x -= 0.5f * ((float)Math.Cos(facing) * 0.3f), z -= 0.5f * ((float)Math.Sin(facing) * 0.3f)));
                        if (!coll)
                        {
                            cameraPos.X -= (float)Math.Cos(facing) * 0.1f;
                            cameraPos.Z -= (float)Math.Sin(facing) * 0.1f;
                        }
                    }
                    else
                    {
                        cameraPos.X -= (float)Math.Cos(facing) * 0.1f;
                        cameraPos.Z -= (float)Math.Sin(facing) * 0.1f;
                    }
                }
                //keyboard movement

                //debug tools
                if (state.IsKeyDown(Keys.D))
                {
                    droneDeactivate = true;
                }

                if (state.IsKeyDown(Keys.O))
                {
                    droneDeactivate = false;
                }

                if (state.IsKeyDown(Keys.C))
                {
                    if (keydown == false)
                    {
                        CollisionsON = !CollisionsON;
                        keydown = true;
                    }
                }
                else
                {
                    keydown = false;
                }


                mouseSpeed *= 0.2f;
                mouseSpeed += XDelta * 0.003f;
                facing += mouseSpeed;
                if (facing < -1.5708f)
                {
                    facing = 4.69494f;
                }
                else if (facing > 4.69494f)
                {
                    facing = -1.5708f;
                }

                Vector3 lookatPoint = new Vector3((float)Math.Cos(facing), 0.0f, (float)Math.Sin(facing));
                view = Matrix.CreateLookAt(cameraPos, cameraPos + lookatPoint, up);
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            drone.UpdateDrone(facing, droneDeactivate);
            hud.updateMap(cameraPos, facing);
            hud.updateHealth();
            hud.updateKeys();
            Console.WriteLine("Player: " + cameraPos.Z + " " + cameraPos.X);
            key1.UpdateKey(cameraPos);
            key2.UpdateKey(cameraPos);
            key3.UpdateKey(cameraPos);
            port.Update(cameraPos, facing);

            CoolGameBase.cp = cameraPos;
            if (lightDir)
            {
                CoolGameBase.sp1.X -= 0.1f;
                CoolGameBase.sp2.X += 0.1f;
                if (CoolGameBase.sp1.X < -4.8f && CoolGameBase.sp2.X > 4.8f)
                {
                    lightDir = false;
                }
            }
            else if (!lightDir)
            {
                CoolGameBase.sp1.X += 0.1f;
                CoolGameBase.sp2.X -= 0.1f;
                if (CoolGameBase.sp1.X > 4.8f && CoolGameBase.sp2.X < -4.8f)
                {
                    lightDir = true;
                }
            }
            //drone.updateDrone();
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            systemManager.ActionSystems(entityManager);

            base.Draw(gameTime);
        }

        protected bool checkCollision(Vector2 pos)
        {
            int gridX, gridY;
            gridY = (int)Math.Round((decimal)(pos.X / 5)) -2;
            gridX = (int)Math.Round((decimal)((pos.Y-2.5f) / 5)) -3;
            Console.WriteLine("GridX: {0} - GridY: {1} Grid {2}", gridX, gridY, LevelStructure[-gridX, -gridY]);
            switch(LevelStructure[-gridX, -gridY])
            {
                case 0:
                    return true;
                case 1:
                    return false;
            }
            return false;
        }

        protected void GameOver(bool win)
        {
            Entity newEntity;
            newEntity = new Entity("Game Over");
            newEntity.AddComponent(new ComponentGeometry("Geometry/MapGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture((win) ? "Textures/win.jpg" : "Textures/lose.jpg"));
            newEntity.AddComponent(new ComponentUI(new Vector2(0.0f, 0.0f), 0.4f));
            newEntity.AddComponent(new ComponentRotation(0));
            entityManager.AddEntity(newEntity);
        }


    }
}