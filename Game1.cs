using System;
using System.Collections.Generic;
using System.Linq;
using Assimp.Configs;
using LDtk;
using LDtk.Renderer;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Enum = LDtkTypes.Enum;

namespace Project1
{
    public class Game1 : Game
    {
        // Textures (Characters)
        private Texture2D playerTexture;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private LDtkWorld world;
        private ExampleRenderer renderer;

        // Player and collision
        private Vector2 playerPosition;
        private float playerSpeed = 150f;
        private List<Rectangle> collisions;
        private Rectangle playerRect;

        //Camera
        private Matrix _translation;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Window Size
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the LDtk file and prepare it
            LDtkFile file = LDtkFile.FromFile("World", Content);
            world = file.LoadWorld(Worlds.World.Iid);
            renderer = new ExampleRenderer(_spriteBatch, Content);

            // Load player texture
            playerTexture = Content.Load<Texture2D>("hm_1");

            // Prerender each level
            foreach (LDtkLevel level in world.Levels)
            {
                renderer.PrerenderLevel(level);
            }

            // Set initial player position (in level space)
            playerPosition = new Vector2(50, 50);

            // Build collision rectangles from level 0 using intGrid Layer
            collisions = new List<Rectangle>();
            var level0 = world.Levels[0];
            var collisionLayer = level0.LayerInstances[0];

            if (collisionLayer != null)
            {
                int gridSize = collisionLayer._GridSize;
                // Number of Cells in the layer horizontally
                int gridCellWidth = collisionLayer._CWid;
                int cells = 0;
                foreach (var element in collisionLayer.IntGridCsv)
                {
                    if (collisionLayer.IntGridCsv[cells] == 1)
                    {
                        int x = (cells % gridCellWidth) * gridSize;
                        int y = (cells / gridCellWidth) * gridSize;

                        collisions.Add(new Rectangle(x, y, gridSize, gridSize));
                    }
                    cells++;
                }

            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 moveDir = Vector2.Zero;
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W)) moveDir.Y = -1;
            if (state.IsKeyDown(Keys.S)) moveDir.Y = 1;
            if (state.IsKeyDown(Keys.A)) moveDir.X = -1;
            if (state.IsKeyDown(Keys.D))
            {
                moveDir.X = 1;
            }

            if (moveDir != Vector2.Zero)
                moveDir.Normalize();

            // Process horizontal movement first
            Vector2 tentativePos = playerPosition;
            tentativePos.X += moveDir.X * playerSpeed * delta;
            if (!IsColliding(tentativePos))
                playerPosition.X = tentativePos.X;

            // Then process vertical movement
            tentativePos = playerPosition;
            tentativePos.Y += moveDir.Y * playerSpeed * delta;
            if (!IsColliding(tentativePos))
                playerPosition.Y = tentativePos.Y;

            //Translation Matrix
            float zoom = 4.0f;
            _translation = Matrix.CreateTranslation(-playerPosition.X, -playerPosition.Y, 0) *  // Move world relative to player
                           Matrix.CreateScale(zoom, zoom, 1.0f) *  // Apply zoom
                           Matrix.CreateTranslation(_graphics.PreferredBackBufferWidth / 2f, _graphics.PreferredBackBufferHeight / 2f, 0); // Re-center screen
            base.Update(gameTime);
        }

        private bool IsColliding(Vector2 pos)
        {
            // Player's bounding rectangle in level space
            playerRect = new Rectangle((int)pos.X + playerTexture.Width / 2, (int)pos.Y + (playerTexture.Height - 5), playerTexture.Width / 4, playerTexture.Height / 10);
            foreach (var rect in collisions)
            {
                if (playerRect.Intersects(rect))
                {
                    Console.WriteLine("[PLAYER] Colliding - TARGET");
                    return true;
                }
            }

            return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var level = world.Levels[0];
            int levelWidth = level.PxWid;
            int levelHeight = level.PxHei;

            // Calculate a uniform scale to fit the level into the window.
            /*
            float scaleX = (float)_graphics.PreferredBackBufferWidth / levelWidth;
            float scaleY = (float)_graphics.PreferredBackBufferHeight / levelHeight;
            float finalScale = Math.Min(scaleX, scaleY);
            Matrix transform = Matrix.CreateScale(finalScale);
            */

            // Create a scale transform
            _spriteBatch.Begin(transformMatrix: _translation, samplerState: SamplerState.PointClamp);

            renderer.RenderPrerenderedLevel(level);
            _spriteBatch.Draw(playerTexture, playerRect, Color.Blue);
            _spriteBatch.Draw(playerTexture, playerPosition, Color.White);
            foreach (var rect in collisions)
            {
                _spriteBatch.Draw(playerTexture, rect, Color.Red);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}