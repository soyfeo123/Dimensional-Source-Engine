using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework.Audio;
using System;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace DimensionalSourceEngine
{
    public enum GameState
    {
        Cutscene,
        MainMenu,
        Game,
        GameIntro,
        LoadingScreen
    }

    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public List<LoadedGame> games = new List<LoadedGame>();
        public static Game1 Instance;
        public GameState currentState = GameState.LoadingScreen;
        public VideoPlayer videoPlayer;
        public Video introVideo;
        bool sC = false;
        public GameSettings loadedGameSettings;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            string[] folders = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
            foreach(string folder in folders)
            {
                if (File.Exists(Path.Combine(folder, "gameinfo.txt")))
                {
                    LoadedGame game = new LoadedGame();
                    game.info = JsonConvert.DeserializeObject<GameInfo>(File.ReadAllText(Path.Combine(folder, "gameinfo.txt")));
                    game.fullPath = folder;
                    
                    if (game.info.mainGame == true)
                    {
                        //DSEDebug.Log(Path.Combine(folder, @"resource\clientscheme.res"));
                        Debug.WriteLine(Path.Combine(folder, @"resource\clientscheme.res"));
                        Debug.WriteLine(File.ReadAllText(Path.Combine(folder, @"resource\clientscheme.res")));
                        loadedGameSettings = JsonConvert.DeserializeObject<GameSettings>(File.ReadAllText(Path.Combine(folder, "cfg", "game_settings.cfg")));
                        Window.Title = game.info.title;
                        Fullscreen(loadedGameSettings.fullScreen);

                        game.clientScheme = JsonConvert.DeserializeObject<ClientScheme>(File.ReadAllText(Path.Combine(folder, "resource", "clientscheme.res")));
                        game.loadedGUI = new GUI_Stuff(GraphicsDevice, game);

                    }
                    game.soundSystem = new DSESoundSystem(game);
                    game.soundSystem.LoadAllSounds();
                    games.Add(game);
                }
            }
            
            base.Initialize();
            
        }

        public void Fullscreen(bool fs)
        {
            Window.IsBorderless = fs;
            if (fs)
            {
                _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                Window.Position = new Point(0, 0);
            }
            else
            {
                _graphics.PreferredBackBufferWidth = 800;
                _graphics.PreferredBackBufferHeight = 480;
            }
            _graphics.ApplyChanges();
        }

        LoadedGame GetMainGame()
        {
            foreach(LoadedGame game in games)
            {
                if (game.info.mainGame)
                {
                    return game;
                }
            }
            return null;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            introVideo = Content.Load<Video>("intro");
            videoPlayer = new VideoPlayer();
            // TODO: use this.Content to load your game content here
        }

        void Start()
        {
            videoPlayer.Play(introVideo);
            currentState = GameState.GameIntro;
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

            if (!sC)
            {
                sC = true;
                Start();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            switch (currentState)
            {
                case GameState.MainMenu: foreach(LoadedGame game in games) {  if(game.info.mainGame) { game.loadedGUI.DrawMainMenu(); } } break;
                case GameState.GameIntro:
                    if (videoPlayer.State != MediaState.Stopped)
                    {
                        try
                        {
                            Texture2D texture = videoPlayer.GetTexture();
                            if (texture != null)
                            {
                                _spriteBatch.Draw(texture, new Rectangle(GraphicsDevice.Viewport.Width / 2 - (640 / 2), GraphicsDevice.Viewport.Height / 2 - (480 / 2), 640, 480), Color.White);
                            }
                        }
                        catch(Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("An error occured while playing intro video:\n" + ex.ToString());
                            Exit();
                        }
                    }
                    else
                    {
                        currentState = GameState.MainMenu;
                        GetMainGame().soundSystem.PlaySound("menu_background.wav");
                    }
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}