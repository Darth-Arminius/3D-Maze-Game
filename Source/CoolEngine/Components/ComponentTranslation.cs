using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoolEngine.Xaml;
using Microsoft.Xna.Framework;

namespace OpenGL_Game.Components
{
    public class ComponentTranslation : IComponent
    {
        Vector3 trans;

        public ComponentTranslation()
        { }
        
        public  ComponentTranslation(float x, float y, float z)
        {
            trans = new Vector3(x, y, z);
        }

        public ComponentTranslation(Vector3 tra)
        {
            trans = tra;
        }

        [TypeConverter(typeof(StringVector3Converter))]
        public Vector3 Translation
        {
            get { return trans; }
            set { trans = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_TRANSLATION; }
        }
    }
}
