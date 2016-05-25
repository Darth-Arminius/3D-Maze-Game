using OpenGL_Game.Objects;
using OpenGL_Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OpenGL_Game.Animations
{
    /// <summary>
    /// Animation of Y axis using repeated inverted hyperbole.
    /// </summary>
    class RollingCubeAnimation : IAnimation
    {
        private int rollsCount;
        private float rollDuration;

        private const float edgeLength = 1.0f;
        private static readonly float rotationDiameter = 1.0f / (float)Math.Sqrt(2);

        public RollingCubeAnimation()
        { }

        public RollingCubeAnimation(int rollsCount, float rollDuration)
        {
            this.RollsCount = rollsCount;
            this.RollDuration = rollDuration;
        }

        public int RollsCount
        {
            get { return rollsCount; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "The number of rolls must be larger than zero.");
                }
                rollsCount = value;
                RefreshDuration();
            }
        }

        public float RollDuration
        {
            get { return rollDuration; }
            set
            {
                if (value <= 0.0f)
                {
                    throw new ArgumentOutOfRangeException("value", "The duration of every roll must be larger than zero.");
                }
                rollDuration = value;
                RefreshDuration();
            }
        }

        public float Duration { get; private set; }

        private void RefreshDuration()
        {
            Duration = 4 * 4 * rollsCount * rollDuration;
        }

        public void UpdateEntity(Entity entity, float time)
        {
            float rollProgress = (time % rollDuration) / rollDuration;
            int previousRolls = (int)(time / rollDuration) % (4 * rollsCount);
            float rollAngle = rollProgress * ((float)Math.PI / 2.0f);
            float cubeAngle = previousRolls * ((float)Math.PI / 2.0f) + rollAngle;

            const float angle135 = (float)Math.PI * 3.0f / 4.0f;
            float yRaise = rotationDiameter * (float)Math.Sin(angle135 - rollAngle) - edgeLength / 2.0f;
            float xShift = rotationDiameter * (float)Math.Cos(angle135 - rollAngle) + edgeLength / 2.0f;
            var translation = new Vector2(edgeLength * previousRolls + xShift, yRaise);

            var entityRotation = entity.Components.OfType<ComponentRotation>().First();
            var entityTranslation = entity.Components.OfType<ComponentTranslation>().First();
            if (time < Duration / 4)
            {
                entityRotation.AllAxes = new Vector3(0.0f, 0.0f, -cubeAngle);
                entityTranslation.Translation = new Vector3(translation.X, translation.Y, 0.0f);
            }
            else if (time < Duration / 2)
            {
                entityRotation.AllAxes = new Vector3(cubeAngle, 0.0f, 0.0f);
                entityTranslation.Translation = new Vector3(4 * rollsCount, translation.Y, translation.X);
            }
            else if (time < Duration * 3 / 4)
            {
                entityRotation.AllAxes = new Vector3(0.0f, 0.0f, cubeAngle);
                entityTranslation.Translation = new Vector3(4 * rollsCount - translation.X, translation.Y, 4 * rollsCount);

            }
            else
            {
                entityRotation.AllAxes = new Vector3(-cubeAngle, 0.0f, 0.0f);
                entityTranslation.Translation = new Vector3(0.0f, translation.Y, 4 * rollsCount - translation.X);
            }
        }
    }
}
