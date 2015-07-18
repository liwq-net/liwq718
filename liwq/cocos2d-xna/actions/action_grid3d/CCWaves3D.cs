using System;

namespace cocos2d
{
    /// <summary>
    /// CCWaves3D action 
    /// </summary>
    public class CCWaves3D : CCGrid3DAction
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
        ///  init the action
        /// </summary>
        public bool initWithWaves(int wav, float amp, ccGridSize gridSize, float duration)
        {
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
            CCWaves3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCWaves3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCWaves3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            copyWithZone(pZone);

            pCopy.initWithWaves(m_nWaves, m_fAmplitude, m_sGridSize, Duration);

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
                    v.z += ((float)Math.Sin((float)Math.PI * time * m_nWaves * 2 + (v.y + v.x) * .01f) * m_fAmplitude * m_fAmplitudeRate);
                    //CCLog("v.z offset is %f\n", ((float)Math.Sin((float)Math.PI * time * m_nWaves * 2 + (v.y + v.x) * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    setVertex(new ccGridSize(i, j), v);
                }
            }
        }

        /// <summary>
        /// create the action
        /// </summary>
        public static CCWaves3D actionWithWaves(int wav, float amp, ccGridSize gridSize, float duration)
        {
            CCWaves3D pAction = new CCWaves3D();

            if (pAction.initWithWaves(wav, amp, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nWaves;
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
    }
}
