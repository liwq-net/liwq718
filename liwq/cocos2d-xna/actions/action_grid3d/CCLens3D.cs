using liwq;
using System;

namespace cocos2d
{
    public class CCLens3D : CCGrid3DAction
    {
        /// <summary>
        /// Get lens center position
        /// </summary>
        public float getLensEffect()
        {
            return m_fLensEffect;
        }

        /// <summary>
        /// Set lens center position
        /// </summary>
        public void setLensEffect(float fLensEffect)
        {
            m_fLensEffect = fLensEffect;
        }

        public CCPoint getPosition()
        {
            return m_position;
        }

        public void setPosition(CCPoint pos)
        {
            if (!pos.Equals(m_position))
            {
                m_position = pos;
                m_positionInPixels.X = pos.X * Director.SharedDirector.ContentScaleFactor;
                m_positionInPixels.Y = pos.Y * Director.SharedDirector.ContentScaleFactor;
                m_bDirty = true;
            }
        }

        /// <summary>
        ///  initializes the action with center position, radius, a grid size and duration
        /// </summary>
        public bool initWithPosition(CCPoint pos, float r, ccGridSize gridSize, float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_position = new CCPoint(-1, -1);
                m_positionInPixels = new CCPoint();

                setPosition(pos);
                m_fRadius = r;
                m_fLensEffect = 0.7f;
                m_bDirty = true;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCLens3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                // in case of being called at sub class
                pCopy = (CCLens3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCLens3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithPosition(m_position, m_fRadius, m_sGridSize, Duration);

            return pCopy;
        }

        public override void Update(float time)
        {
            if (m_bDirty)
            {
                int i, j;

                for (i = 0; i < m_sGridSize.x + 1; ++i)
                {
                    for (j = 0; j < m_sGridSize.y + 1; ++j)
                    {
                        ccVertex3F v = originalVertex(new ccGridSize(i, j));
                        CCPoint vect = new CCPoint(m_positionInPixels.X - new CCPoint(v.x, v.y).X, m_positionInPixels.Y - new CCPoint(v.x, v.y).Y);
                        float r = CCPointExtension.ccpLength(vect);

                        if (r < m_fRadius)
                        {
                            r = m_fRadius - r;
                            float pre_log = r / m_fRadius;
                            if (pre_log == 0)
                            {
                                pre_log = 0.001f;
                            }

                            float l = (float)Math.Log(pre_log) * m_fLensEffect;
                            float new_r = (float)Math.Exp(l) * m_fRadius;

                            if (Math.Sqrt((vect.X * vect.X + vect.Y * vect.Y)) > 0)
                            {
                                vect = CCPointExtension.ccpNormalize(vect);

                                CCPoint new_vect = CCPointExtension.ccpMult(vect, new_r); ;
                                v.z += CCPointExtension.ccpLength(new_vect) * m_fLensEffect;
                            }
                        }

                        setVertex(new ccGridSize(i, j), v);
                    }
                }

                m_bDirty = false;
            }
        }

        /// <summary>
        /// creates the action with center position, radius, a grid size and duration
        /// </summary>
        public static CCLens3D actionWithPosition(CCPoint pos, float r, ccGridSize gridSize, float duration)
        {
            CCLens3D pAction = new CCLens3D();

            if (pAction.initWithPosition(pos, r, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        /* lens center position */
        protected CCPoint m_position;
        protected float m_fRadius;
        /** lens effect. Defaults to 0.7 - 0 means no effect, 1 is very strong effect */
        protected float m_fLensEffect;

        /* @since v0.99.5 */
        // CCPoint m_lastPosition;
        protected CCPoint m_positionInPixels;
        protected bool m_bDirty;
    }
}
