using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace liwq
{
#if !WINDOWS_PHONE && !XBOX && !NETFX_CORE
    [Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof (CCRectConverter))]
#endif
    public struct CCRect
    {
        public static readonly CCRect Zero = new CCRect(0, 0, 0, 0);

        public CCPoint Origin;
        public CCSize Size;

        public CCRect(CCSize sz)
        {
            Origin = CCPoint.Zero;
            Size = sz;
        }

        /// <summary>
        ///     Creates the rectangle at (x,y) -> (width,height)
        /// </summary>
        /// <param name="x">Lower Left corner X</param>
        /// <param name="y">Lower left corner Y</param>
        /// <param name="width">width of the rectangle</param>
        /// <param name="height">height of the rectangle</param>
        public CCRect(float x, float y, float width, float height)
        {
            // Only support that, the width and height > 0
            Debug.Assert(width >= 0 && height >= 0);

            Origin.X = x;
            Origin.Y = y;

            Size.Width = width;
            Size.Height = height;
        }

        /// <summary>
        ///     Returns the inversion of this rect's size, which is the height and width swapped, while the origin stays unchanged.
        /// </summary>
        public CCRect InvertedSize
        {
            get { return new CCRect(Origin.X, Origin.Y, Size.Height, Size.Width); }
        }

        // return the rightmost x-value of 'rect'
        public float MaxX
        {
            get { return Origin.X + Size.Width; }
        }

        // return the midpoint x-value of 'rect'
        public float MidX
        {
            get { return Origin.X + Size.Width / 2.0f; }
        }

        // return the leftmost x-value of 'rect'
        public float MinX
        {
            get { return Origin.X; }
        }

        // Return the topmost y-value of 'rect'
        public float MaxY
        {
            get { return Origin.Y + Size.Height; }
        }

        // Return the midpoint y-value of 'rect'
        public float MidY
        {
            get { return Origin.Y + Size.Height / 2.0f; }
        }

        // Return the bottommost y-value of 'rect'
        public float MinY
        {
            get { return Origin.Y; }
        }

        public CCPoint Center
        {
            get
            {
                CCPoint pt;
                pt.X = MidX;
                pt.Y = MidY;
                return pt;
            }
        }

        public CCPoint UpperRight
        {
            get
            {
                CCPoint pt;
                pt.X = MaxX;
                pt.Y = MaxY;
                return (pt);
            }
        }

        public CCPoint LowerLeft
        {
            get
            {
                CCPoint pt;
                pt.X = MinX;
                pt.Y = MinY;
                return (pt);
            }
        }

        public CCRect Union(CCRect rect)
        {
            float minx = Math.Min(MinX, rect.MinX);
            float miny = Math.Min(MinY, rect.MinY);
            float maxx = Math.Max(MaxX, rect.MaxX);
            float maxy = Math.Max(MaxY, rect.MaxY);
            return (new CCRect(minx, miny, maxx - minx, maxy - miny));
        }

        public CCRect Intersection(CCRect rect)
        {
            if (!IntersectsRect(rect))
            {
                return (Zero);
            }

            /*       +-------------+
             *       |             |
             *       |         +---+-----+
             * +-----+---+     |   |     |
             * |     |   |     |   |     |
             * |     |   |     +---+-----+
             * |     |   |         |
             * |     |   |         |
             * +-----+---+         |
             *       |             |
             *       +-------------+
             */
            float minx = 0, miny = 0, maxx = 0, maxy = 0;
            // X
            if (rect.MinX < MinX)
            {
                minx = MinX;
            }
            else if (rect.MinX < MaxX)
            {
                minx = rect.MinX;
            }
            if (rect.MaxX < MaxX)
            {
                maxx = rect.MaxX;
            }
            else if (rect.MaxX > MaxX)
            {
                maxx = MaxX;
            }
            //  Y
            if (rect.MinY < MinY)
            {
                miny = MinY;
            }
            else if (rect.MinY < MaxY)
            {
                miny = rect.MinY;
            }
            if (rect.MaxY < MaxY)
            {
                maxy = rect.MaxY;
            }
            else if (rect.MaxY > MaxY)
            {
                maxy = MaxY;
            }
            return new CCRect(minx, miny, maxx - minx, maxy - miny);
        }

        public bool IntersectsRect(CCRect rect)
        {
            return !(MaxX < rect.MinX || rect.MaxX < MinX || MaxY < rect.MinY || rect.MaxY < MinY);
        }

        public bool IntersectsRect(ref CCRect rect)
        {
            return !(MaxX < rect.MinX || rect.MaxX < MinX || MaxY < rect.MinY || rect.MaxY < MinY);
        }

        public bool ContainsPoint(CCPoint point)
        {
            return point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY;
        }

        public bool ContainsPoint(float x, float y)
        {
            return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
        }

        public static bool Equal(ref CCRect rect1, ref CCRect rect2)
        {
            return rect1.Origin.Equals(rect2.Origin) && rect1.Size.Equals(rect2.Size);
        }

        public static bool ContainsPoint(ref CCRect rect, ref CCPoint point)
        {
            bool bRet = false;

            if (float.IsNaN(point.X))
            {
                point.X = 0;
            }

            if (float.IsNaN(point.Y))
            {
                point.Y = 0;
            }

            if (point.X >= rect.MinX && point.X <= rect.MaxX && point.Y >= rect.MinY && point.Y <= rect.MaxY)
            {
                bRet = true;
            }

            return bRet;
        }

        public static bool IntersetsRect(ref CCRect rectA, ref CCRect rectB)
        {
            return !(rectA.MaxX < rectB.MinX || rectB.MaxX < rectA.MinX || rectA.MaxY < rectB.MinY || rectB.MaxY < rectA.MinY);
        }

        public static bool operator ==(CCRect p1, CCRect p2)
        {
            return (p1.Equals(p2));
        }

        public static bool operator !=(CCRect p1, CCRect p2)
        {
            return (!p1.Equals(p2));
        }

        public override int GetHashCode()
        {
            return Origin.GetHashCode() + Size.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (Equals((CCRect)obj));
        }

        public bool Equals(CCRect rect)
        {
            return Origin.Equals(rect.Origin) && Size.Equals(rect.Size);
        }

        public override string ToString()
        {
            return String.Format("CCRect : (x={0}, y={1}, width={2}, height={3})", Origin.X, Origin.Y, Size.Width, Size.Height);
        }

        public static CCRect Parse(string s)
        {
#if !WINDOWS_PHONE && !XBOX && !NETFX_CORE
            return (CCRect) TypeDescriptor.GetConverter(typeof (CCRect)).ConvertFromString(s);
#else
            return (CCRectConverter.CCRectFromString(s));
#endif
        }
    }


#if !NETFX_CORE
    public class CCRectConverter : TypeConverter
    {
        public CCRectConverter() { }

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
                return CCRectFromString(value as String);
            }
            return base.ConvertFrom(context, culture, value);
        }

        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return "{" + ((CCRect)value).Origin.X + "," + ((CCRect)value).Origin.Y + "," + ((CCRect)value).Size.Width + "," + ((CCRect)value).Size.Height + "}";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public static CCRect CCRectFromString(string pszContent)
        {
            CCRect result = CCRect.Zero;
            do
            {
                if (pszContent == null)
                {
                    break;
                }

                string content = pszContent;

                // find the first '{' and the third '}'
                int nPosLeft = content.IndexOf('{');
                int nPosRight = content.IndexOf('}');
                for (int i = 1; i < 3; ++i)
                {
                    if (nPosRight == -1)
                    {
                        break;
                    }
                    nPosRight = content.IndexOf('}', nPosRight + 1);
                }
                if (nPosLeft == -1 || nPosRight == -1)
                {
                    break;
                }
                content = content.Substring(nPosLeft + 1, nPosRight - nPosLeft - 1);
                int nPointEnd = content.IndexOf('}');
                if (nPointEnd == -1)
                {
                    break;
                }
                nPointEnd = content.IndexOf(',', nPointEnd);
                if (nPointEnd == -1)
                {
                    break;
                }

                // get the point string and size string
                string pointStr = content.Substring(0, nPointEnd);
                string sizeStr = content.Substring(nPointEnd + 1);
                //, content.Length - nPointEnd
                // split the string with ','
                List<string> pointInfo = new List<string>();

                if (!CCUtils.SplitWithForm(pointStr, pointInfo))
                {
                    break;
                }
                List<string> sizeInfo = new List<string>();
                if (!CCUtils.SplitWithForm(sizeStr, sizeInfo))
                {
                    break;
                }

                float x = CCUtils.CCParseFloat(pointInfo[0]);
                float y = CCUtils.CCParseFloat(pointInfo[1]);
                float width = CCUtils.CCParseFloat(sizeInfo[0]);
                float height = CCUtils.CCParseFloat(sizeInfo[1]);

                result = new CCRect(x, y, width, height);
            } while (false);

            return result;
        }
    }
#else
    public class CCRectConverter
    {
        public static CCRect CCRectFromString(string pszContent)
        {
            CCRect result = CCRect.Zero;

            do
            {
                if (pszContent == null)
                {
                    break;
                }

                string content = pszContent;

                // find the first '{' and the third '}'
                int nPosLeft = content.IndexOf('{');
                int nPosRight = content.IndexOf('}');
                for (int i = 1; i < 3; ++i)
                {
                    if (nPosRight == -1)
                    {
                        break;
                    }
                    nPosRight = content.IndexOf('}', nPosRight + 1);
                }
                if (nPosLeft == -1 || nPosRight == -1)
                {
                    break;
                }
                content = content.Substring(nPosLeft + 1, nPosRight - nPosLeft - 1);
                int nPointEnd = content.IndexOf('}');
                if (nPointEnd == -1)
                {
                    break;
                }
                nPointEnd = content.IndexOf(',', nPointEnd);
                if (nPointEnd == -1)
                {
                    break;
                }

                // get the point string and size string
                string pointStr = content.Substring(0, nPointEnd);
                string sizeStr = content.Substring(nPointEnd + 1);
                //, content.Length - nPointEnd
                // split the string with ','
                List<string> pointInfo = new List<string>();

                if (!CCUtils.SplitWithForm(pointStr, pointInfo))
                {
                    break;
                }
                List<string> sizeInfo = new List<string>();
                if (!CCUtils.SplitWithForm(sizeStr, sizeInfo))
                {
                    break;
                }

                float x = CCUtils.CCParseFloat(pointInfo[0]);
                float y = CCUtils.CCParseFloat(pointInfo[1]);
                float width = CCUtils.CCParseFloat(sizeInfo[0]);
                float height = CCUtils.CCParseFloat(sizeInfo[1]);

                result = new CCRect(x, y, width, height);
            } while (false);

            return result;
        }
    }
#endif

}