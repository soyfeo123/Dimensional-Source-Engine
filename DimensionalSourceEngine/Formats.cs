using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace DimensionalSourceEngine
{
    public class SGUIMenuItems
    {
        public string text;
        public int order;
        public string command;
        public bool onlyOnMainMenu;
        public bool isDivider;
        public MainMenuButton assignedMainMenuButton;
    }

    public class MainMenuButton
    {
        public float y;
        public int size;
        bool hasHoverSoundBeenMade = false;
        bool hasClickSoundBeenMade = false;
        bool hasHoverMouseCursorDisable = false;

        /// <summary>
        /// Detects if the button is being hovered or not.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="doSound"></param>
        /// <returns></returns>
        public bool Hover(LoadedGame game, bool doSound = true)
        {
            Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            if (mouseState.Y > y && mouseState.Y < y + size && mouseState.X > game.clientScheme.Spacing.MainMenuBtnXMargin && mouseState.X < 300)
            {
                if (!hasHoverSoundBeenMade && doSound)
                {
                    game.soundSystem.PlaySound("ui_hover.wav");
                    hasHoverSoundBeenMade = true;
                    if (game.clientScheme.Settings.ShowHandCursorOnBtnHover)
                    {
                        hasHoverMouseCursorDisable = false;
                        Microsoft.Xna.Framework.Input.Mouse.SetCursor(Microsoft.Xna.Framework.Input.MouseCursor.Hand);
                    }
                    }
                    return true;
            }
            else
            {
                hasHoverSoundBeenMade = false;
                if (!hasHoverMouseCursorDisable)
                {
                    Microsoft.Xna.Framework.Input.Mouse.SetCursor(Microsoft.Xna.Framework.Input.MouseCursor.Arrow);
                    hasHoverMouseCursorDisable = true;
                }
                return false;
                
            }
        }

        /// <summary>
        /// Detects if the mouse is clicking on the button or not.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="doSound"></param>
        /// <returns></returns>
        public bool Click(LoadedGame game, bool doSound = true)
        {
            bool click = Hover(game, false) && Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            if (click)
            {
                if(!hasClickSoundBeenMade && doSound)
                {
                    game.soundSystem.PlaySound("ui_accept.wav");
                    hasClickSoundBeenMade = true;
                }
            }
            else
            {
                hasClickSoundBeenMade= false;
            }

            return click;
        }
    }

    public class SGUIMenu
    {
        public SGUIMenuItems[] i;
        public string id;
    }

    public class GameInfo
    {
        public string title;
        public string mainmenu_title;
        public string icon;
        public bool extension;
        public bool mainGame;
    }

    public class LoadedGame
    {
        public GameInfo info;
        public GUI_Stuff loadedGUI;
        public string fullPath;
        public ClientScheme clientScheme;
        public DSESoundSystem soundSystem;
    }

    public class SchemeSettings
    {
        public bool ShowHandCursorOnBtnHover;
    }

    public class SchemeColor
    {
        public string Name;
        public float R;
        public float G;
        public float B;
    }

    public class SchemeSpacing
    {
        public float MainMenuBtnXMargin;
        public float MainMenuBtnYMargin;
    }

    public class ClientScheme
    {
        public SchemeFont[] Fonts;
        public SchemeColor[] Colors;
        public SchemeSpacing Spacing;
        public SchemeSettings Settings;

        public static SchemeFont GetFontWithName(ClientScheme scheme, string name)
        {
            foreach(SchemeFont font in scheme.Fonts)
            {
                if(font.Name == name)
                {
                    return font;
                }
            }
            return null;
        }

        public static Color GetColorWithName(ClientScheme scheme, string name)
        {
            // Yes I copied and pasted code please don't kill me
            foreach (SchemeColor font in scheme.Colors)
            {
                if (font.Name == name)
                {
                    Color color = new Color(new Vector3(font.R, font.G, font.B));
                    return color;
                }
            }
            return new Color(Vector3.Zero);
        }

        public static SpriteFont GetSpriteFontWithName(ClientScheme scheme, string name, LoadedGame game, GraphicsDevice device)
        {
            SchemeFont schemeFont = GetFontWithName(scheme, name);
            TtfFontBakerResult fontBakeResult = null;

            try
            {
                if (File.Exists(Path.Combine(game.fullPath, "resource", "fonts", schemeFont.FontPath)))
                {

                    fontBakeResult = TtfFontBaker.Bake(File.ReadAllBytes(Path.Combine(game.fullPath, "resource", "fonts", schemeFont.FontPath)),
        schemeFont.FontSize,
        1024,
        1024,
        new[]
        {
        CharacterRange.BasicLatin,
        CharacterRange.Latin1Supplement,
        CharacterRange.LatinExtendedA,
        CharacterRange.Cyrillic
        }
    );
                }
                else
                {
                    fontBakeResult = TtfFontBaker.Bake(File.ReadAllBytes(Path.Combine(@"C:\Windows\Fonts\", schemeFont.FontPath)),
        schemeFont.FontSize,
        1024,
        1024,
        new[]
        {
        CharacterRange.BasicLatin,
        CharacterRange.Latin1Supplement,
        CharacterRange.LatinExtendedA,
        CharacterRange.Cyrillic
        }
    );
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to load font " + schemeFont.FontPath + ", aborting", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Game1.Instance.Exit();
            }

            if(fontBakeResult == null)
            {
                Game1.Instance.Exit();
                return null;
            }
            return fontBakeResult.CreateSpriteFont(device);
        }
    }

    public class GameSettings
    {
        public bool fullScreen;
    }

    public class SchemeFont
    {
        public string Name;
        public string FontPath;
        public int FontSize;
    }

    public class DSESoundSystemClip
    {
        public string name;
        public bool isPlaying;
        public bool loop;
        public SoundEffect sfx;
    }

    public class DSESoundSystem
    {
        public List<DSESoundSystemClip> sounds = new List<DSESoundSystemClip>();
        LoadedGame game;

        public DSESoundSystem(LoadedGame attachedGame)
        {
            game = attachedGame;
        }

        public void LoadAllSounds()
        {
            string[] allSounds = Directory.GetFiles(Path.Combine(game.fullPath, "sound"), "*.wav", SearchOption.AllDirectories);
            foreach(string sound in allSounds)
            {
                Debug.WriteLine("Loading clip " + new FileInfo(sound).Name);
                DSESoundSystemClip clip = new DSESoundSystemClip();
                clip.name = new FileInfo(sound).Name;
                clip.sfx = SoundEffect.FromFile(sound);
                sounds.Add(clip);
            }
        }

        public void PlaySound(string soundName)
        {
            foreach(DSESoundSystemClip sound in sounds)
            {
                
                if(sound.name == soundName)
                {
                    sound.isPlaying = true;
                    sound.sfx.Play();
                }
            }
        }

        public void StopSound(string soundName)
        {
            foreach(DSESoundSystemClip sound in sounds)
            {
                if(sound.name == soundName && sound.isPlaying)
                {
                    sound.isPlaying = false;
                    
                }
            }
        }
    }
}
