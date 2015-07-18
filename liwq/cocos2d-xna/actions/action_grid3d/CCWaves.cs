using System;

namespace cocos2d
{
    /// <summary>
    /// CCWaves actio
    /// </summary>
    public class CCWaves : CCGrid3DAction
    {
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
        /// initializes the action with amplitude, horizontal sin, vertical sin, a grid and duration
        /// </summary>
        public bool initWithWaves(int wav, float amp, bool h, bool v, ccGridSize gridSize, float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_nWaves = wav;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;
                m_bHorizontal = h;
                m_bVertical = v;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCWaves pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCWaves)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCWaves();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithWaves(m_nWaves, m_fAmplitude, m_bHorizontal, m_bVertical, m_sGridSize, Duration);

            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < m_sGridSize.x + 1; ++i)
            {
                for (j = 0; j < m_sGridSize.y + 1; ++j)
                {
                    ccVertex3F v = originalVertex(new ccGridSize(i, j));

                    if (m_bVertical)
                    {
                        v.x = (v.x + ((float)Math.Sin(time * (float)Math.PI * m_nWaves * 2 + v.y * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    }

                    if (m_bHorizontal)
                    {
                        v.y = (v.y + ((float)Math.Sin(time * (float)Math.PI * m_nWaves * 2 + v.x * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    }

                    setVertex(new ccGridSize(i, j), v);
                }
            }
        }

        /// <summary>
        /// initializes the action with amplitude, horizontal sin, vertical sin, a grid and duration
        /// </summary>
        public static CCWaves actionWithWaves(int wav, float amp, bool h, bool v, ccGridSize gridSize,
            float duration)
        {
            CCWaves pAction = new CCWaves();

            if (pAction.initWithWaves(wav, amp, h, v, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nWaves;
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
        protected bool m_bVertical;
        protected bool m_bHorizontal;
    }
}
