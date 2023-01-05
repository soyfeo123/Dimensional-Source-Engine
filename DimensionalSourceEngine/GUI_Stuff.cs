using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using SpriteFontPlus;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DimensionalSourceEngine
{
    public class GUI_Stuff
    {
        public List<SGUIMenu> loadedMenus = new List<SGUIMenu>();
        public static GUI_Stuff instance;
        public SpriteFont buttonFont;
        public LoadedGame game_;
        public SpriteFont gameTitleFont;
        public string currentMenuId = "gamemenu_main.res";
        public bool canClickYet = true;
        Microsoft.Xna.Framework.Input.MouseState oldMouseState;

        public GUI_Stuff(GraphicsDevice device, LoadedGame game)
        {
            game_ = game;


            if (game.info.mainGame)
            {
                Debug.WriteLine("Main game found! " + game.info.title);
                Debug.WriteLine(game.clientScheme);
                buttonFont = ClientScheme.GetSpriteFontWithName(game.clientScheme, "Buttons", game, device);
                gameTitleFont = ClientScheme.GetSpriteFontWithName(game.clientScheme, "GameTitle", game, device);
                foreach (string subMenu in Directory.GetFiles(Path.Combine(game.fullPath, "resource"), "gamemenu_*"))
                {
                    SGUIMenu menu = JsonConvert.DeserializeObject<SGUIMenu>(File.ReadAllText(subMenu));
                    menu.id = new FileInfo(subMenu).Name;
                    loadedMenus.Add(menu);
                }
                RePositionEverything();
            }
            
        }

        void RePositionEverything()
        {
            Vector2 currentDrawingPos = new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, (Game1.Instance.GraphicsDevice.Viewport.Height / 2) - 60 + ClientScheme.GetFontWithName(game_.clientScheme, "GameTitle").FontSize + game_.clientScheme.Spacing.MainMenuBtnYMargin);

            foreach (SGUIMenu menu in loadedMenus)
            {
                for (int currentIndex = 0; currentIndex < menu.i.Length; currentIndex++)
                {
                    foreach (SGUIMenuItems item in menu.i)
                    {
                        if (item.order == currentIndex)
                        {
                            Debug.WriteLine(item.text);
                            item.assignedMainMenuButton = new MainMenuButton();
                            item.assignedMainMenuButton.y = currentDrawingPos.Y;
                            item.assignedMainMenuButton.size = ClientScheme.GetFontWithName(game_.clientScheme, "Buttons").FontSize;
                        }
                    }

                    currentDrawingPos.Y += ClientScheme.GetFontWithName(game_.clientScheme, "Buttons").FontSize + game_.clientScheme.Spacing.MainMenuBtnYMargin;
                }
                currentDrawingPos = new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, (Game1.Instance.GraphicsDevice.Viewport.Height / 2) - 60 + ClientScheme.GetFontWithName(game_.clientScheme, "GameTitle").FontSize + game_.clientScheme.Spacing.MainMenuBtnYMargin);
            }
        }

        public void DrawMainMenu()
        {
            

            if (string.IsNullOrEmpty(game_.info.mainmenu_title)) 
                Game1.Instance._spriteBatch.DrawString(gameTitleFont, game_.info.title, new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, (Game1.Instance.GraphicsDevice.Viewport.Height / 2) - 60), Color.White);
            else
                Game1.Instance._spriteBatch.DrawString(gameTitleFont, game_.info.mainmenu_title.Replace(']', '\\'), new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, (Game1.Instance.GraphicsDevice.Viewport.Height / 2) - 60), Color.White);
            try 
            {
                foreach (SGUIMenu menu in loadedMenus)
                {
                    if (menu.id == currentMenuId)
                    {
                        foreach (SGUIMenuItems item in menu.i)
                        {



                            //DSEDebug.Log(item.text + " (order: " + item.order + ")");

                            Microsoft.Xna.Framework.Input.MouseState newState = Microsoft.Xna.Framework.Input.Mouse.GetState();
                            
                            if(oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && newState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                            {
                                canClickYet = true;
                            }

                                if (item.assignedMainMenuButton.Hover(game_, !item.isDivider) && canClickYet)
                                {
                                    Game1.Instance._spriteBatch.DrawString(buttonFont, string.Format(item.text, Game1.Instance.fullscreen.ToString()), new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, item.assignedMainMenuButton.y), ClientScheme.GetColorWithName(game_.clientScheme, "BtnHover"));
                                    if (item.assignedMainMenuButton.Click(game_, !item.isDivider))
                                    {
                                        string[] splitCommand = item.command.Split(' ');
                                    canClickYet = false;
                                        switch (splitCommand[0])
                                        {
                                            case "QuitGame":
                                                Game1.Instance.Exit();
                                                break;

                                            case "OpenSubGameMenu":
                                                currentMenuId = splitCommand[1];
                                                break;
                                        case "ToggleFullscreen":
                                            Game1.Instance.fullscreen = !Game1.Instance.fullscreen;
                                            break;
                                        case "ApplyVideoChanges":
                                            Game1.Instance.Fullscreen(Game1.Instance.fullscreen);
                                            RePositionEverything();
                                            GameSettings settingsToSave = new GameSettings();
                                            settingsToSave.fullScreen = Game1.Instance.fullscreen;
                                            File.WriteAllText(Path.Combine(game_.fullPath, "cfg", "game_settings.cfg"), JsonConvert.SerializeObject(settingsToSave));
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    Game1.Instance._spriteBatch.DrawString(buttonFont, string.Format(item.text, Game1.Instance.fullscreen.ToString()), new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, item.assignedMainMenuButton.y), ClientScheme.GetColorWithName(game_.clientScheme, "BtnNeutral"));
                                }
                            oldMouseState = newState;




                        }
                    }
                }

            }
            catch
            {

            }
        }
    }
}
