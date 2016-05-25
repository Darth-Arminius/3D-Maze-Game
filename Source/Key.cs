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
    class Key
    {
        bool moveAfterSound=false;
        Entity keyEntity;

        public Key(Entity keyEntity)
        {
            this.keyEntity = keyEntity;
        }

        public void UpdateKey(Vector3 playerPos)
        {
            var positionComponent = keyEntity.GetComponent<ComponentPosition>();
            var audioComponent = keyEntity.GetComponent<ComponentAudio>();
            var position = positionComponent.Position;

            playerPos = playerPos / 2;
            Console.WriteLine("Key: " + position.Z + " " + position.X);
            float dx = playerPos.Z - position.Z;
            float dz = playerPos.X - position.X;
            float radii = 0.4f + 0.4f; //player and key radii
            if ((dx * dx) + (dz * dz) < radii * radii)
            {
                if (!moveAfterSound)
                {
                    audioComponent.setPlay();
                    moveAfterSound = true;

                    Console.WriteLine("The 2 circles are colliding!");
                }

            }
            if (moveAfterSound && audioComponent.stopped())
            {

                positionComponent.Position = new Vector3(100, 100, 100);
                MyGame.keys++;
                moveAfterSound = false;
            }
        }
    }
}
