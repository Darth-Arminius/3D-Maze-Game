using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoolEngine.Xaml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Components
{
    public class ComponentTexture : IComponent
    {
        public ComponentTexture()
        { }

        public ComponentTexture(string textureName)
        {
            Texture = ResourceManager.LoadTexture(textureName);
        }

        [TypeConverter(typeof(TextureResourceConverter))]
        public Texture2D Texture { get; set; }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_TEXTURE; }
        }
    }
}
