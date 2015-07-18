using System;

namespace cocos2d
{
    /** @brief CCLiquid action */
    public class CCLiquid : CCGrid3DAction
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

        /** initializes the action with amplitude, a grid and duration */
        public bool initWithWaves(int wav, float amp, ccGridSize gridSize, float duration)
        {
            // if (CCGrid3DAction::initWithSize(gridSize, duration))
            if (initWithSize(gridSize, duration))
            {
                m_nWaves = wav;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCLiquid pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCLiquid)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCLiquid();
                pZone = pNewZone = new CCZone(pCopy);
            }

            // CCGrid3DAction::copyWithZone(pZone);
            base.copyWithZone(pZone);
            pCopy.initWithWaves(m_nWaves, m_fAmplitude, m_sGridSize, Duration);

            // CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            float coeffA = time * (float)Math.PI * m_nWaves * 2f;
            float coeffB = m_fAmplitude * m_fAmplitudeRate;

            for (i = 1; i < m_sGridSize.x; ++i)
            {
                for (j = 1; j < m_sGridSize.y; ++j)
                {
                    ccVertex3F v = originalVertex(i, j);
                    v.x = (v.x + ((float)Math.Sin(coeffA + v.x * .01f) * coeffB));
                    v.y = (v.y + ((float)Math.Sin(coeffA + v.y * .01f) * coeffB));
                    setVertex(i, j, v);
                }
            }
        }

        /** creates the action with amplitude, a grid and duration */
        public static CCLiquid actionWithWaves(int wav, float amp, ccGridSize gridSize, float duration)
        {
            CCLiquid pAction = new CCLiquid();

            if (pAction != null)
            {
                if (pAction.initWithWaves(wav, amp, gridSize, duration))
                {
                    // pAction->autorelease();
                }
                else
                {
                    // CC_SAFE_RELEASE_NULL(pAction);
                }
            }

            return pAction;
        }

        protected int m_nWaves;
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
    }
}
