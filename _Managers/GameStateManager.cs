using System;
using LDtk;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
    }

    public class GameStateManager
    {
        public GameState CurrentGameState { get; private set; } = GameState.MainMenu;
        private UIManager _uiManager;
        private GameManager _gameManager;

        public GameStateManager(Game game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, SpriteBatch spriteBatch)
        {
            Globals.SpriteBatch = spriteBatch;
            Globals.Content = content;
            Globals.File = LDtkFile.FromFile("World", content);
            Globals.World = Globals.File.LoadWorld(Worlds.World.Iid);
            Globals.Collisions = new List<Rectangle>();
            Globals.Graphics = graphicsDeviceManager;
            Globals.WindowSize = new Point(graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight);
            _uiManager = new UIManager(this, game, content, graphicsDeviceManager, spriteBatch);
        }

        public void ChangeGameState(GameState newGameState)
        {
            if (newGameState == GameState.Playing && _gameManager == null)
            {
                _gameManager = new GameManager();
            }
            CurrentGameState = newGameState;
        }

        public void Update(GameTime gameTime)
        {
            Globals.UpdateTime(gameTime);

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    _uiManager.Update(gameTime);
                    break;

                case GameState.Playing:
                    _uiManager.Update(gameTime);
                    _gameManager?.Update();
                    break;

                case GameState.Paused:
                    _uiManager.Update(gameTime);
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    _uiManager.Draw(gameTime, spriteBatch);
                    break;
                case GameState.Playing:
                    _gameManager?.Draw();
                    break;
                case GameState.Paused:
                    _uiManager.Draw(gameTime, spriteBatch);
                    break;
            }
        }
    }
}
