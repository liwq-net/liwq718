using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public enum CCTMXOrientatio : int
    {
        /// <summary>
        /// Orthogonal orientation
        /// </summary>
        CCTMXOrientationOrtho = 0,

        /// <summary>
        /// Hexagonal orientation
        /// </summary>
        CCTMXOrientationHex = 1,

        /// <summary>
        ///  Isometric orientation
        /// </summary>
        CCTMXOrientationIso = 2,
    };

    public enum TMXLayerAttrib
    {
        TMXLayerAttribNone = 1 << 0,
        TMXLayerAttribBase64 = 1 << 1,
        TMXLayerAttribGzip = 1 << 2,
        TMXLayerAttribZlib = 1 << 3,
    };

    public enum TMXProperty
    {
        TMXPropertyNone,
        TMXPropertyMap,
        TMXPropertyLayer,
        TMXPropertyObjectGroup,
        TMXPropertyObject,
        TMXPropertyTile
    };

}
