using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace liwq
{

#if !WINDOWS_PHONE && !XBOX && !NETFX_CORE
    [Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof (CCSizeConverter))]
#endif
    public struct CCSize
    {
        public static readonly CCSize Zero = new CCSize(0, 0);

        public float Width;
        public float Height;

        public CCSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public CCSize Clamp(CCSize max)
        {
            float w = (Width > max.Width) ? max.Width : Width;
            float h = (Height > max.Height) ? max.Height : Height;
            return (new CCSize(w, h));
        }

        /// <summary>
        /// Computes the diagonal length of this size. This method will always compute
        /// the length using Sqrt()
        /// </summary>
        public float Diagonal
        {
            get
            {
                return ((float)Math.Sqrt(Width * Width + Height * Height));
            }
        }

        /// <summary>
        ///     Returns the inversion of this size, which is the height and width swapped.
        /// </summary>
        public CCSize Inverted
        {
            get { return new CCSize(Height, Width); }
        }

        public static bool Equal(ref CCSize size1, ref CCSize size2)
        {
            return ((size1.Width == size2.Width) && (size1.Height == size2.Height));
        }

        public override int GetHashCode()
        {
            return (Width.GetHashCode() + Height.GetHashCode());
        }

        public bool Equals(CCSize s)
        {
            return Width == s.Width && Height == s.Height;
        }

        public override bool Equals(object obj)
        {
            return (Equals((CCSize)obj));
        }

        public CCPoint Center
        {
            get { return new CCPoint(Width / 2f, Height / 2f); }
        }

        public override string ToString()
        {
            return String.Format("{0} x {1}", Width, Height);
        }

        public static bool operator ==(CCSize p1, CCSize p2)
        {
            return (p1.Equals(p2));
        }

        public static bool operator !=(CCSize p1, CCSize p2)
        {
            return (!p1.Equals(p2));
        }

        public static CCSize operator *(CCSize p, float f)
        {
            return (new CCSize(p.Width * f, p.Height * f));
        }

        public static CCSize operator /(CCSize p, float f)
        {
            return (new CCSize(p.Width / f, p.Height / f));
        }

        public static CCSize operator +(CCSize p, float f)
        {
            return (new CCSize(p.Width + f, p.Height + f));
        }

        public static CCSize operator +(CCSize p, CCSize q)
        {
            return (new CCSize(p.Width + q.Width, p.Height + q.Height));
        }

        public static CCSize operator -(CCSize p, float f)
        {
            return (new CCSize(p.Width - f, p.Height - f));
        }

        public static CCSize Parse(string s)
        {
#if !WINDOWS_PHONE && !XBOX && !NETFX_CORE
            return (CCSize) TypeDescriptor.GetConverter(typeof (CCSize)).ConvertFromString(s);
#else
            return (CCSizeConverter.CCSizeFromString(s));
#endif
        }

        /**
         * Allow Cast CCPoint to CCSize
         */

        public static explicit operator CCSize(CCPoint point)
        {
            CCSize size;
            size.Width = point.X;
            size.Height = point.Y;
            return size;
        }

        public CCRect AsRect
        {
            get
            {
                return (new CCRect(0, 0, Width, Height));
            }
        }
    }

#if !NETFX_CORE
    public class CCSizeConverter : TypeConverter
    {
        public CCSizeConverter() { }

        // Overrides the CanConvertFrom method of TypeConverter.
        // The ITypeDescriptorContext interface provides the context for the
        // conversion. Typically, this interface is used at design time to 
        // provide information about the design-time container.
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        // Overrides the ConvertFrom method of TypeConverter.
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return CCSizeFromString(value as String);
            }
            return base.ConvertFrom(context, culture, value);
        }

        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return "{" + ((CCSize)value).Width + "," + ((CCSize)value).Height + "}";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public static CCSize CCSizeFromString(string pszContent)
        {
            CCSize ret = new CCSize();

            do
            {
                List<string> strs = new List<string>();
                if (!CCUtils.SplitWithForm(pszContent, strs)) break;

                float width = CCUtils.CCParseFloat(strs[0]);
                float height = CCUtils.CCParseFloat(strs[1]);

                ret = new CCSize(width, height);
            } while (false);

            return ret;
        }

    }
#else
    public class CCSizeConverter
    {
        public static CCSize CCSizeFromString(string pszContent)
        {
            CCSize ret = new CCSize();

            do
            {
                List<string> strs = new List<string>();
                if (!CCUtils.SplitWithForm(pszContent, strs)) break;

                float width = CCUtils.CCParseFloat(strs[0]);
                float height = CCUtils.CCParseFloat(strs[1]);

                ret = new CCSize(width, height);
            } while (false);

            return ret;
        }

    }
#endif
}


