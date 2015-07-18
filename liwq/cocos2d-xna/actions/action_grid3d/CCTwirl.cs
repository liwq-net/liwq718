using liwq;
using System;

namespace cocos2d
{
    /// <summary>
    /// CCTwirl action
    /// </summary>
    public class CCTwirl : CCGrid3DAction
    {
        /// <summary>
        /// get twirl center
        /// </summary>
        public CCPoint getPosition()
        {
            return m_position;
        }

        /// <summary>
        /// set twirl center
        /// </summary>
        public void setPosition(CCPoint position)
        {
            m_position = position;
            m_positionInPixels.X = position.X * Director.SharedDirector.ContentScaleFactor;
            m_positionInPixels.Y = position.Y * Director.SharedDirector.ContentScaleFactor;
        }

        public float getAmplitude()
        {
            return m_fAmplitude;
        }

        public void setAmplitude(float fAmplitude)
        {
            m_fAmplitude = fAmplitude;
        }

        public float getAmplitudeRate()
        {
            return m_fAmplitudeRate;
        }

        public void setAmplitudeRate(float fAmplitudeRate)
        {
            m_fAmplitudeRate = fAmplitudeRate;
        }

        /// <summary>
        /// initializes the action with center position, number of twirls, amplitude, a grid size and duration
        /// </summary>
        public bool initWithPosition(CCPoint pos, int t, float amp, ccGridSize gridSize,
            float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_positionInPixels = new CCPoint();
                setPosition(pos);
                m_nTwirls = t;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCTwirl pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCTwirl)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCTwirl();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithPosition(m_position, m_nTwirls, m_fAmplitude, m_sGridSize, Duration);
            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;
            CCPoint c = m_positionInPixels;

            for (i = 0; i < (m_sGridSize.x + 1); ++i)
            {
                for (j = 0; j < (m_sGridSize.y + 1); ++j)
                {
                    ccVertex3F v = originalVertex(new ccGridSize(i, j));

                    CCPoint avg = new CCPoint(i - (m_sGridSize.x / 2.0f), j - (m_sGridSize.y / 2.0f));
                    float r = (float)Math.Sqrt((avg.X * avg.X + avg.Y * avg.Y));

                    float amp = 0.1f * m_fAmplitude * m_fAmplitudeRate;
                    float a = r * (float)Math.Cos((float)Math.PI / 2.0f + time * (float)Math.PI * m_nTwirls * 2) * amp;

                    CCPoint d = new CCPoint();

                    d.X = (float)Math.Sin(a) * (v.y - c.Y) + (float)Math.Cos(a) * (v.x - c.X);
                    d.Y = (float)Math.Cos(a) * (v.y - c.Y) - (float)Math.Sin(a) * (v.x - c.X);

                    v.x = c.X + d.X;
                    v.y = c.Y + d.Y;

                    setVertex(new ccGridSize(i, j), v);
                }
            }
        }


        /// <summary>
        ///  creates the action with center position, number of twirls, amplitude, a grid size and duration
        /// </summary>
        public static CCTwirl actionWithPosition(CCPoint pos, int t, float amp, ccGridSize gridSize,
            float duration)
        {
            CCTwirl pAction = new CCTwirl();

            if (pAction.initWithPosition(pos, t, amp, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        /* twirl center */
        protected CCPoint m_position;
        protected int m_nTwirls;
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;

        /*@since v0.99.5 */
        protected CCPoint m_positionInPixels;
    }
}
