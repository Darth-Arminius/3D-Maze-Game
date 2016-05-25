using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemPhysics : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY);

        public string Name
        {
            get { return "SystemPhysics"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                var components = entity.Components;

                IComponent velocityComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });

                IComponent positionComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                Motion((ComponentVelocity)velocityComponent, (ComponentPosition)positionComponent);
            }
        }

        public void Motion(ComponentVelocity vel, ComponentPosition pos)
        {
            pos.Position = pos.Position + vel.Velocity * CoolGameBase.dt;

            //Console.WriteLine("Position of Drone: " + pos.Position);
            //Console.WriteLine("Velocity of Drone: " + vel.Velocity);
        }
    }
}
