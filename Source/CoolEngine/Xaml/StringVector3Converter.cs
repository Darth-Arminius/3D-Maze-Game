using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoolEngine.Xaml
{
    class StringVector3Converter : TypeConverter
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
            string str = value as string;
            if (str != null)
            {
                var fields = str.Split(',');
                if (fields.Length != 3)
                {
                    throw new FormatException("Three values separated by commas were expected");
                }

                var format = CultureInfo.InvariantCulture.NumberFormat;
                var vector = new Vector3(
                    float.Parse(fields[0], format),
                    float.Parse(fields[1], format),
                    float.Parse(fields[2], format)
                );
                return vector;
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
