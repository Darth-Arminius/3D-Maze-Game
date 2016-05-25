using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// The AI code. Author: Armin
/// </summary>
namespace OpenGL_Game.Enemies.Drone
{
    class AI
    {
        public static List<Vector3> nodes = new List<Vector3>();
        Vector3 previous = new Vector3(10, 0, 0);
        int counter = 0;
        /// <summary>
        /// When an AI is created it takes in nodes in the form of Vector3s from the game which it then uses to calculate the pathfinding
        /// </summary>
        public AI ()
        {
            nodes.Add(new Vector3(-40.0f, 0.0f, -32.5f));
            nodes.Add(new Vector3(0.0f, 0.0f, -32.5f));
            nodes.Add(new Vector3(0.0f, 0.0f, 7.5f));
            nodes.Add(new Vector3(-40.0f, 0.0f, 7.5f));
        }

        /// <summary>
        /// The function that calculates the pathfindng of the drone based on the nodes
        /// </summary>
        /// <param name="vel"></param>
        /// <param name="pos"></param>
        /// <param name="speed"></param>
        /// <returns>It returns an updated velocity vector to reflect a new course in the pathfinding</returns>
        public Vector3 updatedVeloctiy(Vector3 vel, Vector3 pos, float speed, bool keyDown)
        {
            if (pos.X > 0.0f && vel.X != 0.0f)
            {
                vel = new Vector3(0.0f, vel.Y, speed);
            }
            else if (pos.X < -40.0f && vel.X != 0.0f)
            {
                vel = new Vector3(0.0f, vel.Y, -speed);
            }

            else if (pos.Z > 7.5f && vel.Z != 0.0f)
            {
                vel = new Vector3(-speed, vel.Y, 0.0f);
            }
            else if (pos.Z < -32.5f && vel.Z != 0.0f)
            {
                vel = new Vector3(speed, vel.Y, 0.0f);
            }

            if (keyDown == true)
            {
                Console.WriteLine("True");
                if (counter == 2)
                {
                    counter = 0;
                }
                if (counter == 0)
                {
                    previous = vel;
                    counter++;
                }
                vel = new Vector3(0, 0, 0);
            }
            else
            {
                Console.WriteLine("False");
                if (counter == 1)
                {
                    vel = previous;
                    counter++;
                }
                Console.WriteLine(previous);
                Console.WriteLine(vel);
            }

            return vel;
        }

        /// <summary>
        /// This function keeps the drone facing forwards by updating its rotation depending on which way it is moving
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <returns>An updated rotation float that the drone uses to change the way it is facing</returns>
        public float updatedRotation(Vector3 pos, float rot)
        {
            if (pos.X <= -40.0f)
            {
                rot = (float)Math.PI;
            }
            else if (pos.X >= 0.0f)
            {
                rot = (float)Math.PI * 2;
            }

            else if (pos.Z <= -32.5f)
            {
                rot = (float)Math.PI / 2;
            }
            else if (pos.Z >= 7.5f)
            {
                rot = -(float)Math.PI / 2;
            }
            return rot;
        }
    }
}
