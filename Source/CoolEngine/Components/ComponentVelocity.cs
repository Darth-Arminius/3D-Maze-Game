using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoolEngine.Xaml;
using Microsoft.Xna.Framework;

namespace OpenGL_Game.Components
{
    public class ComponentVelocity : IComponent
    {
        Vector3 vel;

        public ComponentVelocity()
        { }

        public ComponentVelocity(float x, float y, float z)
        {
            vel = new Vector3(x, y, z);
        }

        public ComponentVelocity(Vector3 v)
        {
            vel = v;
        }

        [TypeConverter(typeof(StringVector3Converter))]
        public Vector3 Velocity
        {
            get { return vel; }
            set { vel = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_VELOCITY; }
        }
    }
}
