using System.Collections.Generic;
using LDtk;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameManager _gameManager;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.HardwareModeSwitch = false;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create neccessary classes and set Global Values
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            Globals.Content = Content;
            Globals.File = LDtkFile.FromFile("World", Content);
            Globals.World = Globals.File.LoadWorld(Worlds.World.Iid);
            Globals.Collisions = new List<Rectangle>();
            Globals.Graphics = _graphics;
            Globals.WindowSize = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _gameManager = new GameManager();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            // Update
            _gameManager.Update();
            Globals.UpdateTime(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _gameManager.Draw();

            base.Draw(gameTime);
        }
    }
}
