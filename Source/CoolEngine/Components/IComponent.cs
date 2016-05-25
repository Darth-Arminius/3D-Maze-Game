using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    public enum ComponentTypes {
        COMPONENT_NONE = 0,
	    COMPONENT_POSITION = 1 << 0,
        COMPONENT_GEOMETRY = 1 << 1,
        COMPONENT_TEXTURE  = 1 << 2,
        COMPONENT_VELOCITY = 1 << 3,
        COMPONENT_ROTATION = 1 << 4,
        COMPONENT_TRANSLATION = 1 << 5,
        COMPONENT_AUDIO = 1 << 6,
        COMPONENT_UI = 1 << 7,
        COMPONENT_ANIMATION = 1 << 8
    }

    public interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
