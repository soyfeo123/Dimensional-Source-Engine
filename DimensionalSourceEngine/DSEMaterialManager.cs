using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace DimensionalSourceEngine
{
    public class DSEMaterial
    {
        public string name;
        public Texture2D texture;
    }

    public class DSEMaterialManager
    {
        LoadedGame game_;
        List<DSEMaterial> materials = new List<DSEMaterial>();

        public DSEMaterialManager(LoadedGame game, GraphicsDevice device)
        {
            game_ = game;

            foreach (string picture in Directory.GetFiles(Path.Combine(game.fullPath, "materials"), "*.png", SearchOption.AllDirectories))
            {
                DSEMaterial material = new DSEMaterial();
                material.texture = Texture2D.FromFile(device, picture);
                material.name = new FileInfo(picture).Name.Substring(0, new FileInfo(picture).Name.Length - 4);
                materials.Add(material);
                Debug.WriteLine("Registered material " + material.name);
            }
        }

        public DSEMaterial GetMaterial(string materialName)
        {
            foreach(DSEMaterial material in materials)
            {
                if(material.name == materialName)
                {
                    return material;
                }
            }
            return null;
        }
    }
}
