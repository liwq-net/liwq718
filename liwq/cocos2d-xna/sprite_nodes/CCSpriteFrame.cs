using liwq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// A CCSpriteFrame has:
    ///- texture: A CCTexture2D that will be used by the CCSprite
    ///- rectangle: A rectangle of the texture
    ///
    /// You can modify the frame of a CCSprite by doing:
    ///	CCSpriteFrame *frame = CCSpriteFrame::frameWithTexture(texture, rect, offset);
    /// sprite->setDisplayFrame(frame);
    /// </summary>
    public class CCSpriteFrame : CCObject
    {
        #region properties

        protected CCRect m_obRectInPixels;
        protected bool m_bRotated;
        protected CCRect m_obRect;
        protected CCPoint m_obOffsetInPixels;
        protected CCSize m_obOriginalSizeInPixels;
        protected Texture m_pobTexture;

        public CCRect RectInPixels
        {
            get { return m_obRectInPixels; }
            set
            {
                m_obRectInPixels = value;
                m_obRect = ccMacros.CC_RECT_PIXELS_TO_POINTS(m_obRectInPixels);
            }
        }

        public bool IsRotated
        {
            get { return m_bRotated; }
            set { m_bRotated = value; }
        }

        /// <summary>
        /// get or set rect of the frame
        /// </summary>
        public CCRect Rect
        {
            get { return m_obRect; }
            set
            {
                m_obRect = value;
                m_obRectInPixels = ccMacros.CC_RECT_POINTS_TO_PIXELS(m_obRect);
            }
        }

        /// <summary>
        /// get or set offset of the frame
        /// </summary>
        public CCPoint OffsetInPixels
        {
            get { return m_obOffsetInPixels; }
            set { m_obOffsetInPixels = value; }
        }

        /// <summary>
        /// get or set original size of the trimmed image
        /// </summary>
        public CCSize OriginalSizeInPixels
        {
            get { return m_obOriginalSizeInPixels; }
            set { m_obOriginalSizeInPixels = value; }
        }

        /// <summary>
        /// get or set texture of the frame
        /// </summary>
        public Texture Texture
        {
            get { return m_pobTexture; }
            set { m_pobTexture = value; }
        }

        #endregion

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCSpriteFrame pCopy = new CCSpriteFrame();
            pCopy.initWithTexture(m_pobTexture, m_obRectInPixels, m_bRotated, m_obOffsetInPixels, m_obOriginalSizeInPixels);
            return pCopy;
        }

        /// <summary>
        /// Create a CCSpriteFrame with a texture, rect in points.
        /// It is assumed that the frame was not trimmed.
        /// </summary>
        public static CCSpriteFrame frameWithTexture(Texture pobTexture, CCRect rect)
        {
            CCSpriteFrame pSpriteFrame = new CCSpriteFrame(); ;
            pSpriteFrame.initWithTexture(pobTexture, rect);

            return pSpriteFrame;
        }

        /// <summary>
        /// Create a CCSpriteFrame with a texture, rect, rotated, offset and originalSize in pixels.
        /// The originalSize is the size in points of the frame before being trimmed.
        /// </summary>
        public static CCSpriteFrame frameWithTexture(Texture pobTexture, CCRect rect, bool rotated, CCPoint offset, CCSize originalSize)
        {
            CCSpriteFrame pSpriteFrame = new CCSpriteFrame();
            pSpriteFrame.initWithTexture(pobTexture, rect, rotated, offset, originalSize);

            return pSpriteFrame;
        }

        /// <summary>
        /// Initializes a CCSpriteFrame with a texture, rect in points.
        /// It is assumed that the frame was not trimmed.
        /// </summary>
        public bool initWithTexture(Texture pobTexture, CCRect rect)
        {
            CCRect rectInPixels = ccMacros.CC_RECT_POINTS_TO_PIXELS(rect);
            return initWithTexture(pobTexture, rectInPixels, false, new CCPoint(0, 0), rectInPixels.Size);
        }

        /// <summary>
        /// Initializes a CCSpriteFrame with a texture, rect, rotated, offset and originalSize in pixels.
        /// The originalSize is the size in points of the frame before being trimmed.
        /// </summary>
        public bool initWithTexture(Texture pobTexture, CCRect rect, bool rotated, CCPoint offset, CCSize originalSize)
        {
            m_pobTexture = pobTexture;

            m_obRectInPixels = rect;
            m_obRect = ccMacros.CC_RECT_PIXELS_TO_POINTS(rect);
            m_bRotated = rotated;
            m_obOffsetInPixels = offset;

            m_obOriginalSizeInPixels = originalSize;

            return true;
        }
    }
}
