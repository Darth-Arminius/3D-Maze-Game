using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenGL_Game.Animations;
using OpenGL_Game.Enemies.Drone;

namespace OpenGL_Game
{
    class WallDraw
    {


        List<Entity> Map = new List<Entity>();

        private void floorChunk(Vector3 trans, float rot)
        {
            Entity newEntity;
            newEntity = new Entity("LWall");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentTranslation(trans));
            newEntity.AddComponent(new ComponentGeometry("Geometry/FloorGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/T.jpg"));
            newEntity.AddComponent(new ComponentRotation(rot));
            Map.Add(newEntity);
        }

        private void ceilingChunk(Vector3 trans, float rot)
        {
            Entity newEntity;
            newEntity = new Entity("LWall");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentTranslation(trans));
            newEntity.AddComponent(new ComponentGeometry("Geometry/CeilingGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/T.jpg"));
            newEntity.AddComponent(new ComponentRotation(rot));
            Map.Add(newEntity);
        }

        private void verticalCorr(int length, float x, float zStart)
        {
            for (int i = 0; i < length; ++i)
            {
                floorChunk(new Vector3(x, 0.0f, zStart - (5.0f * i)), 0.0f);
            }
        }

        private void horizontalCorr(int length, float xStart, float z)
        {
            for (int i = 0; i < length; ++i)
            {
                floorChunk(new Vector3(xStart - (5.0f * i), 0.0f, z), 0.0f);
            }
        }

        private void room(float xBotR, float zBotR, bool ceiling)
        {
            if (ceiling)
            {
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        floorChunk(new Vector3(xBotR - (5.0f * i), 0.0f, zBotR - (5.0f * j)), 0.0f);
                        ceilingChunk(new Vector3(xBotR - (5.0f * i), 3.0f, zBotR - (5.0f * j)), 0.0f);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        floorChunk(new Vector3(xBotR - (5.0f * i), 0.0f, zBotR - (5.0f * j)), 0.0f);
                    }
                }
            }
        }

        private void makeMazeFloor()
        {

            verticalCorr(5, 0.0f, 0.0f);
            horizontalCorr(2, -5.0f, -10.0f);

            room(5.0f, -25.0f, true);

            horizontalCorr(5, -10.0f, -30.0f);
            verticalCorr(2, -20.0f, -20.0f);

            room(-35.0f, -25.0f, false);

            verticalCorr(5, -40.0f, 0.0f);
            horizontalCorr(2, -30.0f, -10.0f);

            room(-35.0f, 15.0f, false);

            horizontalCorr(5, -10.0f, 10.0f);
            verticalCorr(2, -20.0f, 5.0f);

            room(5.0f, 15.0f, false);

            room(-15.0f, -5.0f, false);
        }

        private void vCorrChunk(bool left, bool right, Vector3 trans, float rot)
        {
            Entity newEntity;
            if (right)
            {
                newEntity = new Entity("LWall");
                newEntity.AddComponent(new ComponentPosition(2.5f, 0.0f, 0.0f));
                newEntity.AddComponent(new ComponentTranslation(trans));
                newEntity.AddComponent(new ComponentGeometry("Geometry/RightWallGeometry.txt"));
                newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
                newEntity.AddComponent(new ComponentRotation(rot));
                Map.Add(newEntity);

            }
            if (left)
            {
                newEntity = new Entity("LWall");
                newEntity.AddComponent(new ComponentPosition(-2.5f, 0.0f, 0.0f));
                newEntity.AddComponent(new ComponentTranslation(trans));
                newEntity.AddComponent(new ComponentGeometry("Geometry/LeftWallGeometry.txt"));
                newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
                newEntity.AddComponent(new ComponentRotation(rot));
                Map.Add(newEntity);
            }
        }

        private void hCorrChunk(bool bottom, bool top, Vector3 trans, float rot)
        {
            Entity newEntity;
            if (top)
            {
                newEntity = new Entity("LWall");
                newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, -2.5f));
                newEntity.AddComponent(new ComponentTranslation(trans));
                newEntity.AddComponent(new ComponentGeometry("Geometry/FrontWallGeometry.txt"));
                newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
                newEntity.AddComponent(new ComponentRotation(rot));
                Map.Add(newEntity);

            }
            if (bottom)
            {
                newEntity = new Entity("LWall");
                newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 2.5f));
                newEntity.AddComponent(new ComponentTranslation(trans));
                newEntity.AddComponent(new ComponentGeometry("Geometry/BackWallGeometry.txt"));
                newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
                newEntity.AddComponent(new ComponentRotation(rot));
                Map.Add(newEntity);
            }
        }

        private void roomWalls(bool gTop, bool gBot, bool gLeft, bool gRight, float xmid, float zmid)
        {
            Entity newEntity;
            if (gTop)
            {
                newEntity = new Entity("LWall");
                newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, -5.0f));
                newEntity.AddComponent(new ComponentTranslation(xmid, 0.0f, zmid - 5.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/FrontWallGeometry.txt"));
                newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
                newEntity.AddComponent(new ComponentRotation(0.0f));
                Map.Add(newEntity);

            }
            if (gBot)
            {
                newEntity = new Entity("LWall");
                newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
                newEntity.AddComponent(new ComponentTranslation(xmid, 0.0f, zmid + 5.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/BackWallGeometry.txt"));
                newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
                newEntity.AddComponent(new ComponentRotation(0.0f));
                Map.Add(newEntity);
            }
            if (gLeft)
            {
                newEntity = new Entity("LWall");
                newEntity.AddComponent(new ComponentPosition(-2.5f, 0.0f, 0.0f));
                newEntity.AddComponent(new ComponentTranslation(xmid - 5.0f, 0.0f, zmid));
                newEntity.AddComponent(new ComponentGeometry("Geometry/LeftWallGeometry.txt"));
                newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
                newEntity.AddComponent(new ComponentRotation(0.0f));
                Map.Add(newEntity);

            }
            if (gRight)
            {
                newEntity = new Entity("LWall");
                newEntity.AddComponent(new ComponentPosition(2.5f, 0.0f, 0.0f));
                newEntity.AddComponent(new ComponentTranslation(xmid + 5.0f, 0.0f, zmid));
                newEntity.AddComponent(new ComponentGeometry("Geometry/RightWallGeometry.txt"));
                newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
                newEntity.AddComponent(new ComponentRotation(0.0f));
                Map.Add(newEntity);
            }
            //left wall
            newEntity = new Entity("LWall");
            newEntity.AddComponent(new ComponentPosition(-2.5f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentTranslation(xmid - 5.0f, 0.0f, zmid - 5.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/LeftWallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
            newEntity.AddComponent(new ComponentRotation(0.0f));
            Map.Add(newEntity);

            newEntity = new Entity("LWall");
            newEntity.AddComponent(new ComponentPosition(-2.5f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentTranslation(xmid - 5.0f, 0.0f, zmid + 5.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/LeftWallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
            newEntity.AddComponent(new ComponentRotation(0.0f));
            Map.Add(newEntity);

            //right wall
            newEntity = new Entity("LWall");
            newEntity.AddComponent(new ComponentPosition(2.5f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentTranslation(xmid + 5.0f, 0.0f, zmid - 5.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/RightWallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
            newEntity.AddComponent(new ComponentRotation(0.0f));
            Map.Add(newEntity);

            newEntity = new Entity("LWall");
            newEntity.AddComponent(new ComponentPosition(2.5f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentTranslation(xmid + 5.0f, 0.0f, zmid + 5.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/RightWallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
            newEntity.AddComponent(new ComponentRotation(0.0f));
            Map.Add(newEntity);

            //top wall
            newEntity = new Entity("LWall");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, -5.0f));
            newEntity.AddComponent(new ComponentTranslation(xmid + 5.0f, 0.0f, zmid - 5.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/FrontWallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
            newEntity.AddComponent(new ComponentRotation(0.0f));
            Map.Add(newEntity);

            newEntity = new Entity("LWall");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, -5.0f));
            newEntity.AddComponent(new ComponentTranslation(xmid - 5.0f, 0.0f, zmid - 5.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/FrontWallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
            newEntity.AddComponent(new ComponentRotation(0.0f));
            Map.Add(newEntity);

            //bottom wall
            newEntity = new Entity("LWall");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentTranslation(xmid + 5.0f, 0.0f, zmid + 5.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/BackWallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
            newEntity.AddComponent(new ComponentRotation(0.0f));
            Map.Add(newEntity);

            newEntity = new Entity("LWall");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentTranslation(xmid - 5.0f, 0.0f, zmid + 5.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/BackWallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/t.jpg"));
            newEntity.AddComponent(new ComponentRotation(0.0f));
            Map.Add(newEntity);
        }

        private void makeMazeWalls()
        {
            //long corridors
            float offset = 5.0f;
            for (int i = 0; i < 5; ++i)
            {
                if (i == 2)
                {
                    vCorrChunk(false, true, new Vector3(0.0f, 0.0f, -(offset * i)), 0.0f);
                    vCorrChunk(true, false, new Vector3(-40.0f, 0.0f, -(offset * i)), 0.0f);
                    hCorrChunk(false, true, new Vector3(-10.0f - (offset * i), 0.0f, -32.5f), 0.0f);
                    hCorrChunk(true, false, new Vector3(-10.0f - (offset * i), 0.0f, 7.5f), 0.0f);
                }
                else
                {
                    vCorrChunk(true, true, new Vector3(0.0f, 0.0f, -(offset * i)), 0.0f);
                    vCorrChunk(true, true, new Vector3(-40.0f, 0.0f, -(offset * i)), 0.0f);
                    hCorrChunk(true, true, new Vector3(-10.0f - (offset * i), 0.0f, -32.5f), 0.0f);
                    hCorrChunk(true, true, new Vector3(-10.0f - (offset * i), 0.0f, 7.5f), 0.0f);
                }
            }
            //short corridors
            for (int i = 0; i < 2; ++i)
            {
                vCorrChunk(true, true, new Vector3(-20.0f, 0.0f, 5.0f - (offset * i)), 0.0f);
                vCorrChunk(true, true, new Vector3(-20.0f, 0.0f, -20.0f - (offset * i)), 0.0f);
                hCorrChunk(true, true, new Vector3(-5.0f - (offset * i), 0.0f, -12.5f), 0.0f);
                hCorrChunk(true, true, new Vector3(-30.0f - (offset * i), 0.0f, -12.5f), 0.0f);
            }
            //rooms
            roomWalls(true, false, false, true, 0.0f, -30.0f);
            roomWalls(true, false, true, false, -40.0f, -30.0f);
            roomWalls(false, true, true, false, -40.0f, 10.0f);
            roomWalls(false, true, false, true, 0.0f, 10.0f);
            roomWalls(false, false, false, false, -20.0f, -10.0f);
        }

        public List<Entity> MakeMap()
        {
            makeMazeFloor();
            makeMazeWalls();
            return Map;
        }
    }
}
