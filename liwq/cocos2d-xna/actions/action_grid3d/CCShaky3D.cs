using System;

namespace cocos2d
{
    /// <summary>
    /// CCShaky3D action
    /// </summary>
    public class CCShaky3D : CCGrid3DAction
    {
        /// <summary>
        /// initializes the action with a range, shake Z vertices, a grid and duration
        /// </summary>
        public bool initWithRange(int range, bool shakeZ, ccGridSize gridSize, float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_nRandrange = range;
                m_bShakeZ = shakeZ;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCShaky3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCShaky3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCShaky3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithRange(m_nRandrange, m_bShakeZ, m_sGridSize, Duration);
            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < (m_sGridSize.x + 1); ++i)
            {
                for (j = 0; j < (m_sGridSize.y + 1); ++j)
                {
                    ccVertex3F v = originalVertex(new ccGridSize(i, j));
                    v.x += (random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    v.y += (random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    if (m_bShakeZ)
                    {
                        v.z += (random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    }

                    setVertex(new ccGridSize(i, j), v);
                }
            }
        }

        Random random = new Random();

        /// <summary>
        /// creates the action with a range, shake Z vertices, a grid and duration
        /// </summary>
        public static CCShaky3D actionWithRange(int range, bool shakeZ, ccGridSize gridSize, float duration)
        {
            CCShaky3D pAction = new CCShaky3D();

            if (pAction.initWithRange(range, shakeZ, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nRandrange;
        protected bool m_bShakeZ;
    }
}
