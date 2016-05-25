using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;

namespace OpenGL_Game.Systems
{
    public class SystemAnimation : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_ANIMATION | ComponentTypes.COMPONENT_TRANSLATION | ComponentTypes.COMPONENT_ROTATION);

        public string Name
        {
            get { return "SystemAnimation"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK) { 
                var animationComponent = entity.Components.OfType<ComponentAnimation>().First();
                animationComponent.ElapsedTime += CoolGameBase.dt;
                var time = animationComponent.ElapsedTime % animationComponent.Animation.Duration;
                animationComponent.Animation.UpdateEntity(entity, time);
            }
        }
    }
}
