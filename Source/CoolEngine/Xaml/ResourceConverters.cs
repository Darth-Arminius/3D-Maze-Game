using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace CoolEngine.Xaml
{
    public abstract class ResourceConverterBase<T> : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string filename = value as string;
            if (filename != null)
            {
                return LoadFromManager(filename);
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        protected abstract T LoadFromManager(string filename);
    }

    public class GeometryResourceConverter : ResourceConverterBase<Geometry>
    {
        protected override Geometry LoadFromManager(string filename)
        {
            return ResourceManager.LoadGeometry(filename);
        }
    }

    public class TextureResourceConverter : ResourceConverterBase<Texture2D>
    {
        protected override Texture2D LoadFromManager(string filename)
        {
            return ResourceManager.LoadTexture(filename);
        }
    }

    public class SoundEffectInstanceResourceConverter : ResourceConverterBase<SoundEffectInstance>
    {
        protected override SoundEffectInstance LoadFromManager(string filename)
        {
            return ResourceManager.LoadSoundEffectInstance(filename);
        }
    }
}
