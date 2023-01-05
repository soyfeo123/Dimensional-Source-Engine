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
        public SGUIMenu loadedMenu;
        public static GUI_Stuff instance;
        private SpriteBatch spriteBatch;
        public SpriteFont buttonFont;
        public LoadedGame game_;
        public SpriteFont gameTitleFont;

        public GUI_Stuff(GraphicsDevice device, LoadedGame game)
        {
            game_ = game;


            if (game.info.mainGame)
            {
                Debug.WriteLine("Main game found! " + game.info.title);
                Debug.WriteLine(game.clientScheme);
                buttonFont = ClientScheme.GetSpriteFontWithName(game.clientScheme, "Buttons", game, device);
                gameTitleFont = ClientScheme.GetSpriteFontWithName(game.clientScheme, "GameTitle", game, device);
                loadedMenu = JsonConvert.DeserializeObject<SGUIMenu>(File.ReadAllText(Path.Combine(game.fullPath, "resource", "gamemenu.res")));
                Vector2 currentDrawingPos = new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, (Game1.Instance.GraphicsDevice.Viewport.Height / 2) - 60 + ClientScheme.GetFontWithName(game_.clientScheme, "GameTitle").FontSize + game_.clientScheme.Spacing.MainMenuBtnYMargin);

                for (int currentIndex = 0; currentIndex < loadedMenu.i.Length; currentIndex++)
                {
                    foreach (SGUIMenuItems item in loadedMenu.i)
                    {
                        if (item.order == currentIndex)
                        {
                            item.assignedMainMenuButton = new MainMenuButton();
                            item.assignedMainMenuButton.y = currentDrawingPos.Y;
                            item.assignedMainMenuButton.size = ClientScheme.GetFontWithName(game_.clientScheme, "Buttons").FontSize;
                        }
                    }

                    currentDrawingPos.Y += ClientScheme.GetFontWithName(game_.clientScheme, "Buttons").FontSize + game_.clientScheme.Spacing.MainMenuBtnYMargin;
                }
            }
            
        }

        public void DrawMainMenu()
        {
            
            int currentIndex = 0;

            if (string.IsNullOrEmpty(game_.info.mainmenu_title)) 
                Game1.Instance._spriteBatch.DrawString(gameTitleFont, game_.info.title, new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, (Game1.Instance.GraphicsDevice.Viewport.Height / 2) - 60), Color.White);
            else
                Game1.Instance._spriteBatch.DrawString(gameTitleFont, game_.info.mainmenu_title.Replace(']', '\\'), new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, (Game1.Instance.GraphicsDevice.Viewport.Height / 2) - 60), Color.White);


            foreach (SGUIMenuItems item in loadedMenu.i)
                {
                    
                    
                        
                        //DSEDebug.Log(item.text + " (order: " + item.order + ")");
                        
                        
                        if (item.assignedMainMenuButton.Hover(game_))
                        {
                            Game1.Instance._spriteBatch.DrawString(buttonFont, item.text, new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, item.assignedMainMenuButton.y), ClientScheme.GetColorWithName(game_.clientScheme, "BtnHover"));
                    if (item.assignedMainMenuButton.Click(game_))
                    {
                        
                        switch (item.command)
                        {
                            case "QuitGame":
                                Game1.Instance.Exit();
                                break;
                        }
                    }
                        }
                        else
                        {
                            Game1.Instance._spriteBatch.DrawString(buttonFont, item.text, new Vector2(game_.clientScheme.Spacing.MainMenuBtnXMargin, item.assignedMainMenuButton.y), ClientScheme.GetColorWithName(game_.clientScheme, "BtnNeutral"));
                        }
                    
                    


                }
                
            
        }
    }
}
