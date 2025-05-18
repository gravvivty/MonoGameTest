using System.Collections.Generic;
using LDtk;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Input;
using MLEM.Maths;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Style;

namespace SWEN_Game
{
    public class MainGame : Game
    {
        private UiSystem uiSystem;
        private MainMenuUI mainMenuUI;
        private bool wasEscPressed = false;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameManager _gameManager;
        private InputHandler InputHandler { get; set; } = null!;
        private Texture2D _backgroundTexture;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.HardwareModeSwitch = true;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create necessary classes and set Global Values
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            Globals.Content = Content;
            Globals.File = LDtkFile.FromFile("World", Content);
            Globals.World = Globals.File.LoadWorld(Worlds.World.Iid);
            Globals.Collisions = new List<Rectangle>();
            Globals.Graphics = _graphics;
            Globals.WindowSize = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            _backgroundTexture = Content.Load<Texture2D>("Menu/Background");
            var style = new UntexturedStyle(_spriteBatch)
            {
                Font = new GenericSpriteFont(Content.Load<SpriteFont>("Menu/GameFont")),
                TextColor = Color.Black,
                TextScale = 2.5F,
                ButtonTexture = new NinePatch(Content.Load<Texture2D>("Menu/button_normal"), padding: 1),
            };

            // Initialize the UI system
            this.InputHandler = new InputHandler(this);

            uiSystem = new UiSystem(this, style, this.InputHandler);
            uiSystem.AutoScaleReferenceSize = new Point(1280, 720);
            uiSystem.AutoScaleWithScreen = true;

            Components.Add(uiSystem);
            mainMenuUI = new MainMenuUI(uiSystem);
            mainMenuUI.Show();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            IsMouseVisible = GameStateManager.CurrentGameState != GameState.Playing;
            this.InputHandler.Update();
            Globals.UpdateTime(gameTime);
            uiSystem.Update(gameTime);

            if (GameStateManager.CurrentGameState == GameState.Playing && _gameManager == null)
            {
                System.Diagnostics.Debug.WriteLine("Creating GameManager...");
                _gameManager = new GameManager();
            }

            var keyboardState = Keyboard.GetState();
            if (GameStateManager.CurrentGameState == GameState.Playing)
            {
                if (keyboardState.IsKeyDown(Keys.Escape) && !wasEscPressed)
                {
                    wasEscPressed = true;
                    GameStateManager.ChangeGameState(GameState.Paused);
                    mainMenuUI.ShowOptionsOnly();
                }
            }

            if (keyboardState.IsKeyUp(Keys.Escape))
            {
                wasEscPressed = false;
            }

            if (GameStateManager.CurrentGameState == GameState.Playing)
            {
                _gameManager?.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (GameStateManager.CurrentGameState == GameState.MainMenu)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(
                    _backgroundTexture,
                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                    Color.White * 0.2f);
                _spriteBatch.End();
            }

            if (GameStateManager.CurrentGameState == GameState.Playing || GameStateManager.CurrentGameState == GameState.Paused)
            {
                _gameManager?.Draw();
            }

            uiSystem.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
