using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoolEngine.Xaml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    public class ComponentGeometry : IComponent
    {
        public ComponentGeometry()
        { }

        public ComponentGeometry(string geometryName)
        {
            this.Geometry = ResourceManager.LoadGeometry(geometryName);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_GEOMETRY; }
        }

        [TypeConverter(typeof(GeometryResourceConverter))]
        public Geometry Geometry { get; set; }
    }
}
