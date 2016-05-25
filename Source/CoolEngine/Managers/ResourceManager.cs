using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Objects;
using Microsoft.Xna.Framework.Audio;

namespace OpenGL_Game.Managers
{
    public static class ResourceManager
    {
        static Dictionary<string, Geometry> geometryDictionary = new Dictionary<string, Geometry>();
        static Dictionary<string, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();

        public static Geometry LoadGeometry(string filename)
        {
            Geometry geometry;
            geometryDictionary.TryGetValue(filename, out geometry);
            if (geometry == null)
            {
                geometry = new Geometry();
                geometry.LoadObject(filename);
                geometryDictionary.Add(filename, geometry);
            }

            return geometry;
        }

        public static Texture2D LoadTexture(string filename)
        {
            Texture2D texture;
            textureDictionary.TryGetValue(filename, out texture);
            if (texture == null)
            {
                texture = CoolGameBase.gameInstance.Content.Load<Texture2D>(filename);
                textureDictionary.Add(filename, texture);
            }

            return texture;
        }

        public static SoundEffectInstance LoadSoundEffectInstance(string filename)
        {
            var soundEffect = CoolGameBase.gameInstance.Content.Load<SoundEffect>(filename);
            return soundEffect.CreateInstance();
        }
    }
}
