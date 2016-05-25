using OpenGL_Game.Objects;
using OpenGL_Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Animations
{
    /// <summary>
    /// Animation of Y axis using repeated inverted hyperbole.
    /// </summary>
    class BouncingAnimation : IAnimation
    {
        private float duration;
        private float maxHeight;
        private float a, b;

        public BouncingAnimation()
        { }

        public BouncingAnimation(float maxHeight, float duration)
        {
            MaxHeight = maxHeight;
            Duration = duration;
        }

        public float MaxHeight
        {
            get { return maxHeight; }
            set
            {
                maxHeight = value;
                RefreshAB();
            }
        }

        public float Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                RefreshAB();
            }
        }

        private void RefreshAB()
        {
            if (Duration != 0.0f)
            {
                b = 4 * MaxHeight / Duration;
                a = -b / Duration;
            }
        }

        public void UpdateEntity(Entity entity, float time)
        {
            var translationComponent = entity.Components.OfType<ComponentTranslation>().First();
            var vector = translationComponent.Translation;
            vector.Y = a * time * time + b * time;
            translationComponent.Translation = vector;
        }
    }
}
