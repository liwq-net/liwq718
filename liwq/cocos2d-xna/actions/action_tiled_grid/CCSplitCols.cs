using liwq;

namespace cocos2d
{
    public class CCSplitCols : CCTiledGrid3DAction
    {
        /// <summary>
        ///  initializes the action with the number of columns to split and the duration 
        /// </summary>
        public bool initWithCols(int nCols, float duration)
        {
            m_nCols = nCols;
            return base.initWithSize(new ccGridSize(nCols, 1), duration);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCSplitCols pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCSplitCols)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCSplitCols();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);
            pCopy.initWithCols(m_nCols, Duration);

            //CC_SAFE_DELETE(pNewZone);
            pNewZone = null;
            return pCopy;
        }

        public override void Update(float time)
        {
            int i;

            for (i = 0; i < m_sGridSize.x; ++i)
            {
                ccQuad3 coords = originalTile(i, 0);
                if (coords == null)
                {
                    return;
                }
                float direction = 1;

                if ((i % 2) == 0)
                {
                    direction = -1;
                }

                coords.bl.y += direction * m_winSize.Height * time;
                coords.br.y += direction * m_winSize.Height * time;
                coords.tl.y += direction * m_winSize.Height * time;
                coords.tr.y += direction * m_winSize.Height * time;

                setTile(new ccGridSize(i, 0), coords);
            }
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            m_winSize = Director.SharedDirector.DisplaySize;
        }

        /// <summary>
        /// creates the action with the number of columns to split and the duration
        /// </summary>
        public static CCSplitCols actionWithCols(int nCols, float duration)
        {
            CCSplitCols pAction = new CCSplitCols();
            if (pAction.initWithCols(nCols, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nCols;
        protected CCSize m_winSize;
    }
}
