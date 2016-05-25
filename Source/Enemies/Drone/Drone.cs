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

/// <summary>
/// The drone code. Author: Armin
/// </summary>
namespace OpenGL_Game.Enemies.Drone
{
    /// <summary>
    /// All of the states of the drone
    /// </summary>
    enum DroneState
    {
        normal,
        attacking,
        returning
    }

    /// <summary>
    /// These two classes are used to form lists that are used when calculating the return vector for the drone
    /// </summary>
    class nodeDifferencesX
    {
        public float difference;
        public Vector3 node;
    }

    class nodeDifferencesZ
    {
        public float difference;
        public Vector3 node;
    }

    class Drone
    {
        EntityManager entityManager;
        AI droneAI;
        DroneState droneState = DroneState.normal;
        float dx, dz, radii, speed;
        float bulletTime, closestX, closestZ, closestNodeX, closestNodeZ = 0;
        List<nodeDifferencesX> differencesX = new List<nodeDifferencesX>();
        List<nodeDifferencesZ> differencesZ = new List<nodeDifferencesZ>();
        Vector3 playerPos = MyGame.cameraPos;
        Vector3 closestNode = new Vector3(0, 0, 0);
        Entity droneEntity;
        Entity newBullet;

        /// <summary>
        /// Creating the drone and setting things like speed and position
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pos"></param>
        /// <param name="vel"></param>
        /// <param name="spe"></param>
        public Drone(EntityManager e, Entity droneEntity, float spe)
        {
            droneAI = new AI();
            speed = spe;
            entityManager = e;
            this.droneEntity = droneEntity;
        }

        /// <summary>
        /// This is an update function for the drone
        /// </summary>
        /// <param name="playerRot"></param>
        public void UpdateDrone(float playerRot, bool keyDown)
        {
            if (keyDown == true)
            {
                droneState = DroneState.normal;
            }
            Console.WriteLine("The closest node position is: " + closestNode);

            IComponent VelocityComponent = droneEntity.Components.FirstOrDefault(delegate(IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
            });

            IComponent PositionComponent = droneEntity.Components.FirstOrDefault(delegate(IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });

            IComponent RotationComponent = droneEntity.Components.FirstOrDefault(delegate(IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_ROTATION;
            });

            //This switch statement determines what the drone will do depending on its state
            switch (droneState)
            {
                //The Normal state: The drone will patrol around the borders of the map
                case DroneState.normal:
                    ((ComponentVelocity)VelocityComponent).Velocity = droneAI.updatedVeloctiy(((ComponentVelocity)VelocityComponent).Velocity, ((ComponentPosition)PositionComponent).Position, speed, keyDown);
                    ((ComponentRotation)RotationComponent).YAxis = droneAI.updatedRotation(((ComponentPosition)PositionComponent).Position, ((ComponentRotation)RotationComponent).YAxis);
                    break;
                //The attacking state: The drone will lock on and follow the player and shoot at them. In order to get to this state the player must be detected by the drone
                case DroneState.attacking:
                    bulletTime += CoolGameBase.dt;
                    if (bulletTime >= 5) //Cooldown for drone gun
                    {
                        bulletTime = 0;
                        entityManager.RemoveEntity(newBullet);
                    }
                    //Console.WriteLine("Current bullet time: " + bulletTime);
                    ((ComponentVelocity)VelocityComponent).Velocity = new Vector3(playerPos.X - ((ComponentPosition)PositionComponent).Position.X, 0.0f, playerPos.Z - ((ComponentPosition)PositionComponent).Position.Z) / 2;
                    ((ComponentRotation)RotationComponent).YAxis = (-playerRot - (float)Math.PI/2);;
                    createBullet(((ComponentPosition)PositionComponent).Position, ((ComponentVelocity)VelocityComponent).Velocity * 1.5f); //Making the bullet
                    break;
                    //The returning state: The drone will return back to the original path by going to the closest point of the path
                case DroneState.returning:
                    ((ComponentVelocity)VelocityComponent).Velocity = new Vector3(closestNode.X - ((ComponentPosition)PositionComponent).Position.X, 0.0f, closestNode.Z - ((ComponentPosition)PositionComponent).Position.Z);
                    break;
                default:
                    break;
            }

            if (keyDown == false)
            { 
            playerPos = MyGame.cameraPos;

            if (bulletPos() != new Vector3(0, 0, 0)) //Checks to see if the bullet has hit the player
            {
                dx = playerPos.Z - bulletPos().Z;
                dz = playerPos.X - bulletPos().X;
                radii = 0.4f + 0.4f; //player and bullet radii
                if ((dx * dx) + (dz * dz) < radii * radii)
                {
                    Console.WriteLine("The player has been hit by a bullet");
                    entityManager.RemoveEntity(newBullet);
                    HUD.health -= 1;
                }
            }

            dx = playerPos.Z - ((ComponentPosition)PositionComponent).Position.Z;
            dz = playerPos.X - ((ComponentPosition)PositionComponent).Position.X;
            radii = 0.4f + 10f; //player and drone radii
            Console.WriteLine("Camera position: " + playerPos);
            Console.WriteLine("Drone position: " + ((ComponentPosition)PositionComponent).Position);
                //Console.WriteLine("Position squared difference: " + (dx * dx) + (dz * dz));
                if ((dx * dx) + (dz * dz) < radii * radii) //Checks to see if the drone has detected the player, if so then the drone changes to the attacking state
                {
                    Console.WriteLine("The drone is detecting the player");
                    droneState = DroneState.attacking;
                }
                else if (((ComponentPosition)PositionComponent).Position.X <= closestNode.X + 2 && ((ComponentPosition)PositionComponent).Position.Z <= closestNode.Z + 2 && droneState == DroneState.returning) //Checks to see if the drone has returned to the patrolling line, if so then the drone returns to the normal state
                {
                    Console.WriteLine("The drone has reached the start point");
                    ((ComponentPosition)PositionComponent).Position = closestNode;
                    droneState = DroneState.normal;
                }
                else
                {
                    Console.WriteLine("The drone is no longer detecting the player");
                    if (droneState != DroneState.normal) //This runs when the drone has lost sight of the player and now needs to return to the patrolling line
                    {
                        Console.WriteLine("The drone is returning");
                        droneState = DroneState.returning;

                        foreach (Vector3 node in AI.nodes) //This calculates the difference between each point on the patrolling line and the position of the drone to calculate the shortest distance
                        {
                            nodeDifferencesX nodeDifferenceX = new nodeDifferencesX();
                            nodeDifferenceX.difference = node.X - ((ComponentPosition)PositionComponent).Position.X;
                            if (nodeDifferenceX.difference < 0)
                                nodeDifferenceX.difference = -nodeDifferenceX.difference;
                            nodeDifferenceX.node = node;
                            differencesX.Add(nodeDifferenceX);

                            nodeDifferencesZ nodeDifferenceZ = new nodeDifferencesZ();
                            nodeDifferenceZ.difference = node.Z - ((ComponentPosition)PositionComponent).Position.Z;
                            if (nodeDifferenceZ.difference < 0)
                                nodeDifferenceZ.difference = -nodeDifferenceZ.difference;
                            nodeDifferenceZ.node = node;
                            differencesZ.Add(nodeDifferenceZ);
                        }
                        foreach (nodeDifferencesX difference in differencesX) //Finding the shortest distance on the X axis
                        {
                            if (closestX == 0)
                            {
                                closestX = difference.difference;
                                closestNodeX = difference.node.X;
                            }
                            if (difference.difference < closestX)
                            {
                                closestX = difference.difference;
                                closestNodeX = difference.node.X;
                            }
                        }
                        foreach (nodeDifferencesZ difference in differencesZ) //Finding the shortest distance on the Z axis
                        {
                            if (closestZ == 0)
                            {
                                closestZ = difference.difference;
                                closestNodeZ = difference.node.Z;
                            }
                            if (difference.difference < closestZ)
                            {
                                closestZ = difference.difference;
                                closestNodeZ = difference.node.Z;
                            }
                        }
                        if (closestX < closestZ) //This if and else check to see whether the X or the Z has the shortest distance and moves the drone there accordingly
                        {
                            closestNode.X = closestNodeX;
                            closestNode.Z = ((ComponentPosition)PositionComponent).Position.Z;
                        }
                        else
                        {
                            closestNode.Z = closestNodeZ;
                            closestNode.X = ((ComponentPosition)PositionComponent).Position.X;
                        }
                    }
                    else //If nothing else happens then the drone must be in the normal mode
                    {
                        Console.WriteLine("The drone has returned to normal");
                        droneState = DroneState.normal;
                    }
                }
            }
        }

        /// <summary>
        /// This is the function that is run when a new bullet needs to be created (when the drone fires a bullet)
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="vel"></param>
        public void createBullet(Vector3 pos, Vector3 vel)
        {
            if (bulletTime <= 0.01)
            {
                newBullet = new Entity("NewBullet");
                newBullet.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
                newBullet.AddComponent(new ComponentTranslation(0.0f, 0.0f, 0.0f));
                newBullet.AddComponent(new ComponentPosition(pos));
                newBullet.AddComponent(new ComponentTexture("Textures/Drone.jpg"));
                newBullet.AddComponent(new ComponentVelocity(vel));
                newBullet.AddComponent(new ComponentRotation(0.0f));
                entityManager.AddEntity(newBullet);
                IComponent audio = droneEntity.Components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ((ComponentAudio)audio).setPlay();
                //Console.WriteLine("Bullet has been created");
            }
        }

        /// <summary>
        /// A simple get function to find the current position of the current bullet that is then used to calculate whether or not the bullet has hit the player
        /// </summary>
        /// <returns>Either the position of the bullet or a blank vector if there aren't any bullets</returns>
        public Vector3 bulletPos()
        {
            var bulletEntity = entityManager.FindEntity("NewBullet");

            if (bulletEntity != null)
            {
                IComponent PositionComponent = bulletEntity.Components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                return ((ComponentPosition)PositionComponent).Position;
            }

            return new Vector3(0, 0, 0);
        }
    }
}
