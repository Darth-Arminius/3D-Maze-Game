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
    class Portal
    {
        bool isOpen = false;
        Entity portalEntity;

        public Portal(Entity portalEntity)
        {
            this.portalEntity = portalEntity;
        }

        public void Update(Vector3 playerPos, float playerRot)
        {
            playerPos = playerPos / 2;

            var rotationComponent = portalEntity.GetComponent<ComponentRotation>();
            var audioComponent = portalEntity.GetComponent<ComponentAudio>();
            rotationComponent.YAxis = (-playerRot - (float)Math.PI / 2);

            var position = portalEntity.GetComponent<ComponentPosition>().Position;

            float dx = playerPos.Z - position.Z;
            float dz = playerPos.X - position.X;
            float radii = 0.4f + 0.4f; //player and key radii

            if (MyGame.keys == 3 && !isOpen)
            {
                isOpen = true;
                audioComponent.changeSound("Audio/portalOpen.wav");
            }
            
            if ((dx * dx) + (dz * dz) < radii * radii)
            {
                if (isOpen)
                    MyGame.wonGame = true;
                Console.WriteLine("The 2 circles are colliding!");
            }
        }
    }
}
