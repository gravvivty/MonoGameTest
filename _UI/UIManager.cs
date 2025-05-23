using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Font;
using MLEM.Input;
using MLEM.Maths;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Style;


namespace SWEN_Game
{
    public class UIManager
    {
        private readonly GameStateManager _gameStateManager;
        private readonly UiSystem _uiSystem;
        private readonly MainMenuUI _mainMenuUI;
        private Texture2D _backgroundTexture;
        private InputHandler _inputHandler;

        private bool wasEscPressed = false;

        public UIManager(GameStateManager gameStateManager, Game game, ContentManager content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            _gameStateManager = gameStateManager;
            _backgroundTexture = content.Load<Texture2D>("Menu/Background");
            _inputHandler = new InputHandler(game);

            var style = new UntexturedStyle(spriteBatch)
            {
                Font = new GenericSpriteFont(content.Load<SpriteFont>("Menu/GameFont")),
                TextColor = Color.Black,
                TextScale = 2.5F,
                ButtonTexture = new NinePatch(content.Load<Texture2D>("Menu/button_normal"), padding: 1), // Fixed Padding argument
            };

            _uiSystem = new UiSystem(game, style, _inputHandler)
            {
                AutoScaleReferenceSize = new Point(1280, 720),
                AutoScaleWithScreen = true,
            };

            _mainMenuUI = new MainMenuUI(_uiSystem, _gameStateManager, game);
            _mainMenuUI.Show();
        }

        public void Update(GameTime gameTime)
        {
            _inputHandler.Update();
            _uiSystem.Update(gameTime);

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape) && !wasEscPressed)
            {
                wasEscPressed = true;
                if (_gameStateManager.CurrentGameState == GameState.Playing)
                {
                    _gameStateManager.ChangeGameState(GameState.Paused);
                    _mainMenuUI.ShowOptionsOnly();
                }
                else if (_gameStateManager.CurrentGameState == GameState.Paused)
                {
                    _gameStateManager.ChangeGameState(GameState.Playing);
                    _mainMenuUI.Hide();
                }
            }
            if (keyboardState.IsKeyUp(Keys.Escape))
            {
                wasEscPressed = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.White);
            spriteBatch.End();
            _uiSystem.Draw(gameTime, spriteBatch);
        }
    }
}
