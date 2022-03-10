using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Startup;
using MLEM.Ui;
using MLEM.Ui.Elements;
using Pong.Entities;

namespace Pong
{
    public class Pong : MlemGame
    {
        public static Pong Instance { get; private set; }

        private Ball _ball;
        public Player PlayerOne;
        public Player PlayerTwo;
        private Board _board; 

        private Group _gameUi;
        private Group _mainMenu;
        private Group _startCounter;
        private string _counter;

        private const string GameTitle = "\"PONG\"";
        private const string GameOver = "GAME OVER!";

        private string _mainMenuTitle;
        
        private readonly List<BaseEntity> _entities = new List<BaseEntity>();

        public Pong()
        {
            Instance = this;
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            _mainMenuTitle = GameTitle;
        }

        protected override void LoadContent()
        {
            this.GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
            this.GraphicsDeviceManager.PreferredBackBufferHeight = 720;
            this.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            this.GraphicsDeviceManager.ApplyChanges();
            base.LoadContent();
            
            // Entities
            
            _ball = new Ball(new Vector2(this.GraphicsDevice.Viewport.Width/2, this.GraphicsDevice.Viewport.Height/2));
            PlayerOne = new Player(new Vector2(10, this.GraphicsDevice.Viewport.Height / 2 - 64));
            PlayerTwo = new Player(new Vector2(
                    this.GraphicsDevice.Viewport.Width - 42, 
                    this.GraphicsDevice.Viewport.Height / 2 - 64),
                ControlType.ARROWS);
            _board = new Board(Vector2.Zero);
            
            // UI

            this.UiSystem.AutoScaleWithScreen = true;
            this.UiSystem.Style.Font = new GenericSpriteFont(LoadContent<SpriteFont>("Regular"));
            
            // Score
            _gameUi = new Group(Anchor.TopCenter, new Vector2(this.GraphicsDevice.Viewport.Width/2, 1), true);
            _gameUi.AddChild(new Paragraph(Anchor.TopLeft, 1, paragraph => PlayerOne?.Points.ToString(), true) {
                TextScale = 3F
            });
            _gameUi.AddChild(new Paragraph(Anchor.TopRight, 1, paragraph => PlayerTwo?.Points.ToString(), true) {
                TextScale = 3F
            });
            this.UiSystem.Add("GameUI", _gameUi);
            
            // Main Menu
            this._mainMenu = new Group(Anchor.TopLeft, Vector2.One, false);
            this.UiSystem.Add("Menu", this._mainMenu);
            var center = this._mainMenu.AddChild(new Group(Anchor.Center, Vector2.One));
            center.AddChild(new Paragraph(Anchor.AutoCenter, 1, paragraph => _mainMenuTitle, true) {TextScale = 5F});
            center.AddChild(new VerticalSpace(100));
            center.AddChild(new Button(Anchor.AutoCenter, new Vector2(400, 70), "New Game")
            {
                OnPressed = element => this.StartGame() 
            });
            center.Root.SelectElement(center.GetChildren(c => c.CanBeSelected).First(), true);

            OpenMainMenu();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            base.DoUpdate(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _entities.ForEach(e => e.Update(gameTime));
            
            if (PlayerOne?.Points == 10 || PlayerTwo?.Points == 10)
                OpenMainMenu(true);
            
            this.UiSystem.Update(gameTime);
        }

        protected override void DoDraw(GameTime gameTime)
        {
            this.UiSystem.DrawEarly(gameTime, SpriteBatch);
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            _entities.ForEach(e => e.Draw(gameTime, SpriteBatch));
            
            SpriteBatch.End();
            
            this.UiSystem.Draw(gameTime, SpriteBatch);
            
            base.DoDraw(gameTime);
        }

        private void StartGame()
        {
            this._mainMenu.IsHidden = true;
            this._gameUi.IsHidden = false;
            _entities.Clear();

            _entities.Add(_ball);
            _entities.Add(PlayerOne);
            _entities.Add(PlayerTwo);
            _entities.Add(_board);
            
            _entities.ForEach(e => e.ResetPosition());
        }

        private void OpenMainMenu(bool isGameOver = false)
        {
            this._mainMenu.IsHidden = false;
            this._gameUi.IsHidden = true;
            this._entities.Clear();

            this._mainMenuTitle = isGameOver ? GameOver : GameTitle;
        }
    }
}