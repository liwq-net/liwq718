using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace cocos2d
{
    /// <summary>
    /// A CCCamera is used in every CCNode.
    /// Useful to look at the object from different views.
    /// The OpenGL gluLookAt() function is used to locate the camera.
    ///
    ///	If the object is transformed by any of the scale, rotation or
    /// position attributes, then they will override the camera.

    /// IMPORTANT: Either your use the camera or the rotation/scale/position properties. You can't use both.
    /// World coordinates won't work if you use the camera.
    ///
    /// Limitations:
    ///	 - Some nodes, like CCParallaxNode, CCParticle uses world node coordinates, and they won't work properly if you move them (or any of their ancestors)
    /// using the camera.
    /// - It doesn't work on batched nodes like CCSprite objects when they are parented to a CCSpriteBatchNode object.
    /// - It is recommended to use it ONLY if you are going to create 3D effects. For 2D effecs, use the action CCFollow or position/scale/rotate.
    /// </summary>
    public class CCCamera : CCObject
    {
        protected float m_fEyeX;
        protected float m_fEyeY;
        protected float m_fEyeZ;

        protected float m_fCenterX;
        protected float m_fCenterY;
        protected float m_fCenterZ;

        protected float m_fUpX;
        protected float m_fUpY;
        protected float m_fUpZ;

        protected bool m_bDirty;
        /// <summary>
        ///  sets \ get the dirty value
        /// </summary>
        public bool Dirty
        {
            get { return m_bDirty; }
            set { m_bDirty = value; }
        }

        public CCCamera()
        {
            init();
        }

        public void init()
        {
            restore();
        }

        public string description()
        {
            return String.Format("<CCCamera | center = ({0},{1},{2})>", m_fCenterX, m_fCenterY, m_fCenterZ);
        }

        /// <summary>
        /// sets the camera in the default position
        /// </summary>
        public void restore()
        {
            m_fEyeX = m_fEyeY = 0.0f;
            m_fEyeZ = getZEye();

            m_fCenterX = m_fCenterY = m_fCenterZ = 0.0f;

            m_fUpX = 0.0f;
            m_fUpY = 1.0f;
            m_fUpZ = 0.0f;

            m_bDirty = false;
        }

        /// <summary>
        ///  Sets the camera using gluLookAt using its eye, center and up_vector
        /// </summary>
        public Matrix? locate()
        {
            if (m_bDirty)
            {
                return Matrix.CreateLookAt(new Vector3(m_fEyeX, m_fEyeY, m_fEyeZ),
                             new Vector3(m_fCenterX, m_fCenterY, m_fCenterZ),
                             new Vector3(m_fUpX, m_fUpY, m_fUpZ));

                //gluLookAt(m_fEyeX, m_fEyeY, m_fEyeZ,
                //    m_fCenterX, m_fCenterY, m_fCenterZ,
                //    m_fUpX, m_fUpY, m_fUpZ);
            }

            return null;
        }

        /// <summary>
        /// sets the eye values in points
        /// </summary>
        /// <param name="fEyeX"></param>
        /// <param name="fEyeY"></param>
        /// <param name="fEyeZ"></param>
        public void setEyeXYZ(float fEyeX, float fEyeY, float fEyeZ)
        {
            m_fEyeX = fEyeX * Director.SharedDirector.ContentScaleFactor;
            m_fEyeY = fEyeY * Director.SharedDirector.ContentScaleFactor;
            m_fEyeZ = fEyeZ * Director.SharedDirector.ContentScaleFactor;

            m_bDirty = true;
        }

        /// <summary>
        /// sets the center values in points
        /// </summary>
        /// <param name="fCenterX"></param>
        /// <param name="fCenterY"></param>
        /// <param name="fCenterZ"></param>
        public void setCenterXYZ(float fCenterX, float fCenterY, float fCenterZ)
        {
            m_fCenterX = fCenterX * Director.SharedDirector.ContentScaleFactor;
            m_fCenterY = fCenterY * Director.SharedDirector.ContentScaleFactor;
            m_fCenterZ = fCenterZ * Director.SharedDirector.ContentScaleFactor;

            m_bDirty = true;
        }

        /// <summary>
        ///  sets the up values
        /// </summary>
        /// <param name="fUpX"></param>
        /// <param name="fUpY"></param>
        /// <param name="fUpZ"></param>
        public void setUpXYZ(float fUpX, float fUpY, float fUpZ)
        {
            m_fUpX = fUpX;
            m_fUpY = fUpY;
            m_fUpZ = fUpZ;

            m_bDirty = true;
        }

        /// <summary>
        ///  get the eye vector values in points
        /// </summary>
        /// <param name="pEyeX"></param>
        /// <param name="pEyeY"></param>
        /// <param name="pEyeZ"></param>
        public void getEyeXYZ(out float pEyeX, out float pEyeY, out float pEyeZ)
        {
            pEyeX = m_fEyeX / Director.SharedDirector.ContentScaleFactor;
            pEyeY = m_fEyeY / Director.SharedDirector.ContentScaleFactor;
            pEyeZ = m_fEyeZ / Director.SharedDirector.ContentScaleFactor;
        }

        /// <summary>
        ///  get the center vector values int points 
        /// </summary>
        /// <param name="pCenterX"></param>
        /// <param name="pCenterY"></param>
        /// <param name="pCenterZ"></param>
        public void getCenterXYZ(out float pCenterX, out float pCenterY, out float pCenterZ)
        {
            pCenterX = m_fCenterX / Director.SharedDirector.ContentScaleFactor;
            pCenterY = m_fCenterY / Director.SharedDirector.ContentScaleFactor;
            pCenterZ = m_fCenterZ / Director.SharedDirector.ContentScaleFactor;
        }

        /// <summary>
        ///  get the up vector values
        /// </summary>
        /// <param name="pUpX"></param>
        /// <param name="pUpY"></param>
        /// <param name="pUpZ"></param>
        public void getUpXYZ(out float pUpX, out float pUpY, out float pUpZ)
        {
            pUpX = m_fUpX;
            pUpY = m_fUpY;
            pUpZ = m_fUpZ;
        }

        /// <summary>
        /// returns the Z eye
        /// </summary>
        /// <returns></returns>
        public static float getZEye()
        {
            return 1.192092896e-07F;
        }
    }
}
