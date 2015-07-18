using System;

namespace cocos2d
{
    public class CCWavesTiles3D : CCTiledGrid3DAction
    {
        protected int m_nWaves;
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
        /// <summary>
        /// waves amplitude 
        /// </summary>
        /// <returns></returns>
        public float Amplitude
        {
            get { return m_fAmplitude; }
            set { m_fAmplitude = value; }
        }

        /// <summary>
        /// waves amplitude rate
        /// </summary>
        /// <returns></returns>
        public float AmplitudeRate
        {
            get { return m_fAmplitudeRate; }
            set { m_fAmplitudeRate = value; }
        }

        /// <summary>
        ///  initializes the action with a number of waves, the waves amplitude, the grid size and the duration 
        /// </summary>
        public bool initWithWaves(int wav, float amp, ccGridSize gridSize, float duration)
        {
            if (base.initWithSize(gridSize, duration))
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
            CCWavesTiles3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCWavesTiles3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCWavesTiles3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithWaves(m_nWaves, m_fAmplitude, m_sGridSize, Duration);

            //CC_SAFE_DELETE(pNewZone);
            pNewZone = null;
            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < m_sGridSize.x; i++)
            {
                for (j = 0; j < m_sGridSize.y; j++)
                {
                    ccQuad3 coords = originalTile(i, j);
                    if (coords == null)
                    {
                        // Scene not ready yet
                        return;
                    }
                    coords.bl.z = ((float)Math.Sin(time * (float)Math.PI * m_nWaves * 2 +
                        (coords.bl.y + coords.bl.x) * .01f) * m_fAmplitude * m_fAmplitudeRate);
                    coords.br.z = coords.bl.z;
                    coords.tl.z = coords.bl.z;
                    coords.tr.z = coords.bl.z;

                    setTile(i, j, coords);
                }
            }
        }

        /// <summary>
        /// creates the action with a number of waves, the waves amplitude, the grid size and the duration
        /// </summary>
        public static CCWavesTiles3D actionWithWaves(int wav, float amp, ccGridSize gridSize, float duration)
        {
            CCWavesTiles3D pAction = new CCWavesTiles3D();

            if (pAction.initWithWaves(wav, amp, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }
    }
}
