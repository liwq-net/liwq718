using System;
using Microsoft.Xna.Framework.Graphics;
using liwq;
namespace cocos2d
{
    public class ccMacros
    {
        /// <summary>
        /// simple macro that swaps 2 variables
        /// </summary>
        public static void CC_SWAP<T>(ref T x, ref  T y)
        {
            T temp = x;
            x = y; y = temp;
        }

        private static Random rand = new Random();
        /// <summary>
        /// returns a random float between -1 and 1
        /// </summary>
        public static float CCRANDOM_MINUS1_1()
        {
            return (2.0f * ((float)rand.Next() / int.MaxValue)) - 1.0f;
        }

        /** @def CCRANDOM_0_1
            returns a random float between 0 and 1
         */
        public static float CCRANDOM_0_1()
        {
            return (float)rand.Next() / int.MaxValue;
        }


        ///** @def CC_RADIANS_TO_DEGREES
        //    converts radians to degrees
        //*/
        //public static float CC_RADIANS_TO_DEGREES(float angle)
        //{
        //    return angle * 57.29577951f; // PI * 180
        //}

        public static readonly float FLT_EPSILON = 1.192092896e-07F;

        // Retina is only supported on iOS.
#if CC_IS_RETINA_DISPLAY_SUPPORTED
#else
        public static int CC_CONTENT_SCALE_FACTOR()
        {
            return 1;
        }

        public static CCRect CC_RECT_PIXELS_TO_POINTS(CCRect pixels)
        {
            return pixels;
        }

        public static CCRect CC_RECT_POINTS_TO_PIXELS(CCRect points)
        {
            return points;
        }
#endif // CC_IS_RETINA_DISPLAY_SUPPORTED

        public static bool CC_HOST_IS_BIG_ENDIAN()
        {
            return !BitConverter.IsLittleEndian;
        }

        // Only unsigned int can use these functions.

        public static uint CC_SWAP32(uint i)
        {
            return ((i & 0x000000ff) << 24 | (i & 0x0000ff00) << 8 | (i & 0x00ff0000) >> 8 | (i & 0xff000000) >> 24);
        }

        public static ushort CC_SWAP16(ushort i)
        {
            return (ushort)((i & 0x00ff) << 8 | (i & 0xff00) >> 8);
        }

        public static uint CC_SWAP_INT32_LITTLE_TO_HOST(uint i)
        {
            return ((CC_HOST_IS_BIG_ENDIAN() == true) ? CC_SWAP32(i) : (i));
        }

        public static ushort CC_SWAP_INT16_LITTLE_TO_HOST(ushort i)
        {
            return ((CC_HOST_IS_BIG_ENDIAN() == true) ? CC_SWAP16(i) : (i));
        }

        public static uint CC_SWAP_INT32_BIG_TO_HOST(uint i)
        {
            return ((CC_HOST_IS_BIG_ENDIAN() == true) ? (i) : CC_SWAP32(i));
        }

        public static ushort CC_SWAP_INT16_BIG_TO_HOST(ushort i)
        {
            return ((CC_HOST_IS_BIG_ENDIAN() == true) ? (i) : CC_SWAP16(i));
        }

        /*
         * Macros of CCGeometry.h
         */
        public static CCPoint CCPointMake(float x, float y)
        {
            return new CCPoint(x, y);
        }
        public static CCSize CCSizeMake(float width, float height)
        {
            return new CCSize(width, height);
        }
        public static CCRect CCRectMake(float x, float y, float width, float height)
        {
            return new CCRect(x, y, width, height);
        }

        /*
         * Macros defined in ccConfig.h
         */
        public static readonly string CC_RETINA_DISPLAY_FILENAME_SUFFIX = "-hd";


        /*
         * Macros defined in CCSprite.h
         */
        public static readonly int CCSpriteIndexNotInitialized = 320000000;// 0xffffffff; // CCSprite invalid index on the CCSpriteBatchode


        // The following macros are defined for opengl es, they are not needed.

#if CC_OPTIMIZE_BLEND_FUNC_FOR_PREMULTIPLIED_ALPHA
        public static readonly int CC_BLEND_SRC = OGLES.GL_ONE;
        public static readonly int CC_BLEND_DST = OGLES.GL_ONE_MINUS_SRC_ALPHA;
#else
         public int CC_BLEND_SRC= OGLES.GL_SRC_ALPHA;
         public int CC_BLEND_DST=  OGLES.GL_ONE_MINUS_SRC_ALPHA;
#endif

        // #define CC_BLEND_DST GL_ONE_MINUS_SRC_ALPHA

        // #define CC_ENABLE_DEFAULT_GL_STATES() {				\
        // glEnableClientState(GL_VERTEX_ARRAY);			\
        // glEnableClientState(GL_COLOR_ARRAY);			\
        // glEnableClientState(GL_TEXTURE_COORD_ARRAY);	\
        // glEnable(GL_TEXTURE_2D);			

        // #define CC_DISABLE_DEFAULT_GL_STATES() {			\
        // glDisable(GL_TEXTURE_2D);						\
        // glDisableClientState(GL_COLOR_ARRAY);			\
        // glDisableClientState(GL_TEXTURE_COORD_ARRAY);	\
        // glDisableClientState(GL_VERTEX_ARRAY);			\
    }
}
