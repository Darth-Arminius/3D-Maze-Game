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
    public class SystemAudio : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AUDIO);

        public string Name
        {
            get { return "SystemAudio"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                var components = entity.Components;

                IComponent positionComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                IComponent audioComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                Set((ComponentAudio)audioComponent, (ComponentPosition)positionComponent);
            }
        }

        public void Set(ComponentAudio audio, ComponentPosition pos)
        {
            audio.Gpos = pos.Position;
            Matrix world = Matrix.CreateTranslation(audio.Gpos);

            Matrix wvp = world * CoolGameBase.view * CoolGameBase.projection;
            audio.setPos = Vector3.Transform(pos.Position,wvp);
            audio.updateA();
        }
    }
}
