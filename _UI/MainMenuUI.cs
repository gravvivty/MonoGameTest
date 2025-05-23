using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Maths;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;

namespace SWEN_Game;

public enum MenuState
{
    MainMenu,
    Options,
    Paused,
}

public class MainMenuUI
{
    private readonly UiSystem ui;
    private readonly GameStateManager gameStateManager; // Add this field to store the GameStateManager instance
    private readonly Game game; // Add this field to store the Game instance
    private Panel rootPanel;
    private MenuState currentMenuState;

    public MainMenuUI(UiSystem uiSystem, GameStateManager gameStateManager, Game game)
    {
        this.ui = uiSystem;
        this.gameStateManager = gameStateManager;
        this.game = game;

        // Create the root panel that contains all menu elements
        rootPanel = new Panel(Anchor.Center, new Vector2(0.8F, 0.2F), Vector2.Zero);
        rootPanel.Texture = null;
        uiSystem.Add("MainMenu", rootPanel);

        ClearAndSwitch(MenuState.MainMenu);
    }

    public void ClearAndSwitch(MenuState newMenuState)
    {
        currentMenuState = newMenuState;
        rootPanel.RemoveChildren();
        switch (currentMenuState)
        {
            case MenuState.MainMenu:
                ShowMainMenu();
                break;
            case MenuState.Options:
                ShowOptionsMenu();
                break;
        }
    }

    public void ShowOptionsOnly()
    {
        Show();
        ClearAndSwitch(MenuState.Options);
    }

    public void Show() => rootPanel.IsHidden = false;
    public void Hide() => rootPanel.IsHidden = true;

    private void ShowMainMenu()
    {
        var playButton = new Button(Anchor.AutoInline, new Vector2(0.3F, 0.6F), "Play");
        playButton.PositionOffset = new Vector2(30, 0);

        playButton.OnPressed += _ =>
        {
            gameStateManager.ChangeGameState(GameState.Playing); // Use the instance to call the method
            Hide();
        };

        rootPanel.AddChild(playButton);

        var optionsButton = new Button(Anchor.AutoInline, new Vector2(0.3F, 0.6F), "Options");
        optionsButton.PositionOffset = new Vector2(30, 0);
        optionsButton.OnPressed += _ => ClearAndSwitch(MenuState.Options);

        rootPanel.AddChild(optionsButton);

        var exitButton = new Button(Anchor.AutoInline, new Vector2(0.3F, 0.6F), "Exit");
        exitButton.PositionOffset = new Vector2(30, 0);
        exitButton.OnPressed += _ =>
        {
            System.Diagnostics.Debug.WriteLine("Exit Clicked");
            ui.Game.Exit();
        };

        rootPanel.AddChild(exitButton);
    }

    private void ShowOptionsMenu()
    {
        var dropdown = new Dropdown(Anchor.AutoLeft, new Vector2(0.5F, 0.6F), "Window Size");
        var resolutions = new[]
        {
            new { Label ="1280x720", Width = 1280, Height = 720 },
            new { Label ="1600x900", Width = 1600, Height = 900 },
            new { Label ="1920x1080", Width = 1920, Height = 1080 },
        };

        foreach (var res in resolutions)
        {
            dropdown.AddElement(
                res.Label,
                element =>
            {
                Globals.WindowSize = new Point(res.Width, res.Height);
                Globals.Graphics.PreferredBackBufferWidth = res.Width;
                Globals.Graphics.PreferredBackBufferHeight = res.Height;
                Globals.Graphics.ApplyChanges();
                dropdown.IsOpen = false;
            }, 0);
        }

        rootPanel.AddChild(dropdown);

        // Add other options UI elements here
        rootPanel.AddChild(new Button(Anchor.AutoLeft, new Vector2(0.5F, 0.6F), "Fullscreen")
        {
            CanBePressed = false,
            CanBeSelected = false,
            PositionOffset = new Vector2(0, 30),
        });

        var fullscreenCheck = new Checkbox(Anchor.AutoInline, new Vector2(0.5F, 0.6F), string.Empty)
        {
            Checked = Globals.Fullscreen,
        };

        fullscreenCheck.OnCheckStateChange += (element, isChecked) =>
        {
            Globals.Fullscreen = isChecked;
            Globals.Graphics.IsFullScreen = isChecked;
            Globals.Graphics.ApplyChanges();
        };

        rootPanel.AddChild(fullscreenCheck);

        // BACK Button: Switch game state to MainMenu
        var backButton = new Button(Anchor.AutoLeft, new Vector2(0.3F, 0.6F), "Save");
        backButton.PositionOffset = new Vector2(0, 50);
        backButton.OnPressed += _ =>
        {
            System.Diagnostics.Debug.WriteLine("Back Clicked");
            ClearAndSwitch(MenuState.MainMenu);
        };
        rootPanel.AddChild(backButton);
    }
}
