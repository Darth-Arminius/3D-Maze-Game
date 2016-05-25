using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game
{
    class HUD
    {
        EntityManager entityManager;
        public static int health;
        //bool key1 = false, key2 = false, key3 = false;


        public HUD(ref EntityManager e)
        {
            health = 3;
            entityManager = e;
        }

        public void update()
        {
            updateHealth();
            updateKeys();
        }

        public void updateKeys()
        {
            Entity e;
            string bar;
            switch (MyGame.keys)
            {
                case 3:
                    bar = "KeyUI2";
                    break;
                case 2:
                    bar = "KeyUI1";
                    break;
                case 1:
                    bar = "KeyUI0";
                    break;
                default:
                    bar = null;
                    break;
            }
            if (bar != null)
            {
                e = entityManager.FindEntity(bar);

                IComponent UIComponent = e.Components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_UI;
                });
                Vector2 hold = ((ComponentUI)UIComponent).Position;
                ((ComponentUI)UIComponent).Position = new Vector2(hold.X, -0.8f);
            }
        }

        public void updateHealth()
        {
            Entity e;
            string bar;
            switch(health)
            {
                case 2:
                    bar = "Healthbar2";
                    break;
                case 1:
                    bar = "Healthbar1";
                    break;
                case 0:
                    bar = "Healthbar0";
                    break;
                default:
                    bar = null;
                    break;
            }
            if (bar != null)
            {
                e = entityManager.FindEntity(bar);

                IComponent UIComponent = e.Components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_UI;
                });

                ((ComponentUI)UIComponent).Position = new Vector2(2f, 2f);
            }
        }

        public void updateMap(Vector3 playerPos, float playerRotation)
        {
            Entity e;
            e = entityManager.FindEntity("PlayerBlip");

            IComponent UIComponent = e.Components.FirstOrDefault(delegate(IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_UI;
            });

            ((ComponentUI)UIComponent).Position = new Vector2(0.805f + playerPos.Z / 115, 0.88f + playerPos.X / 115);

            IComponent RotationComponent = e.Components.FirstOrDefault(delegate(IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_ROTATION;
            });

            ((ComponentRotation)RotationComponent).YAxis = (-playerRotation + (float)Math.PI/2);

            //drone position
            Entity d;
            d = entityManager.FindEntity("Drone");

            IComponent PositionComponent = d.Components.FirstOrDefault(delegate(IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });

            Vector3 dronePosition = ((ComponentPosition)PositionComponent).Position;

            RotationComponent = d.Components.FirstOrDefault(delegate(IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_ROTATION;
            });

            float droneRotation = ((ComponentRotation)RotationComponent).YAxis ;

            Entity db;
            db = entityManager.FindEntity("DroneBlip");

            UIComponent = db.Components.FirstOrDefault(delegate(IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_UI;
            });

            RotationComponent = db.Components.FirstOrDefault(delegate(IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_ROTATION;
            });
            
            ((ComponentUI)UIComponent).Position = new Vector2(0.805f + dronePosition.Z / 115, 0.88f + dronePosition.X / 115);

            ((ComponentRotation)RotationComponent).YAxis = (droneRotation);
        }


        
    }
}
