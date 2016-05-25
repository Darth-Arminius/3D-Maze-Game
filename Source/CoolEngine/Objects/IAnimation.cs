using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Objects
{
    public interface IAnimation
    {
        float Duration { get; }

        void UpdateEntity(Entity entity, float time);
    }
}
