using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using Microsoft.Xna.Framework.Design;
using System.ComponentModel;


namespace OpenGL_Game.Components
{
    public class ComponentUI : IComponent
    {
        Vector2 position;
        float scale;

        public ComponentUI()
        { }

        /// <param name="position"></param>
        /// <param name="scale"></param>
        public ComponentUI(Vector2 Position, float Scale)
        {
            scale = Scale;
            position = Position;
        }

        [TypeConverter(typeof(Vector2TypeConverter))]
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_UI; }
        }
    }
}