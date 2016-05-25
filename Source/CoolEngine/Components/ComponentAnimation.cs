using OpenGL_Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    public class ComponentAnimation : IComponent
    {
        public ComponentAnimation()
        {
            ElapsedTime = 0.0f;
        }

        public ComponentAnimation(IAnimation animation) : this()
        {
            Animation = animation;
        }

        public IAnimation Animation { get; set; }

        public float ElapsedTime { get; set; }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_ANIMATION; }
        }
    }
}
