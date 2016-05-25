using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OpenGL_Game.Components
{
    public class ComponentRotation : IComponent
    {
        Vector3 RotationVector;

        public ComponentRotation()
        { }

        public ComponentRotation(float rotX, float rotY, float rotZ)
        {
            RotationVector = new Vector3(rotX, rotY, rotZ);
        }

        public ComponentRotation(float rotY)
        {
            RotationVector = new Vector3(0.0f, rotY, 0.0f);
        }

        public Vector3 AllAxes {
            get { return RotationVector; }
            set { RotationVector = value; }
        }

        public float XAxis
        {
            get { return RotationVector.X; }
            set { RotationVector.X = value; }
        }

        public float YAxis
        {
            get { return RotationVector.Y; }
            set { RotationVector.Y = value; }
        }

        public float ZAxis
        {
            get { return RotationVector.Z; }
            set { RotationVector.Z = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_ROTATION; }
        }
    }
}
