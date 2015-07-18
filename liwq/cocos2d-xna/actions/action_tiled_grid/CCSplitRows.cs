using liwq;

namespace cocos2d
{
    public class CCSplitRows : CCTiledGrid3DAction
    {
        /// <summary>
        /// initializes the action with the number of rows to split and the duration
        /// </summary>
        public bool initWithRows(int nRows, float duration)
        {
            m_nRows = nRows;

            return base.initWithSize(new ccGridSize(1, nRows), duration);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCSplitRows pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCSplitRows)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCSplitRows();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithRows(m_nRows, Duration);

            //CC_SAFE_DELETE(pNewZone);
            pNewZone = null;
            return pCopy;
        }

        public override void Update(float time)
        {
            int j;

            for (j = 0; j < m_sGridSize.y; ++j)
            {
                ccQuad3 coords = originalTile(0, j);
                if (coords == null)
                {
                    // Scene not ready yet
                    return;
                }
                float direction = 1;

                if ((j % 2) == 0)
                {
                    direction = -1;
                }

                coords.bl.x += direction * m_winSize.Width * time;
                coords.br.x += direction * m_winSize.Width * time;
                coords.tl.x += direction * m_winSize.Width * time;
                coords.tr.x += direction * m_winSize.Width * time;

                setTile(new ccGridSize(0, j), coords);
            }
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            m_winSize = Director.SharedDirector.DisplaySize;
        }

        /// <summary>
        ///  creates the action with the number of rows to split and the duration 
        /// </summary>
        public static CCSplitRows actionWithRows(int nRows, float duration)
        {
            CCSplitRows pAction = new CCSplitRows();
            if (pAction.initWithRows(nRows, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nRows;
        protected CCSize m_winSize;
    }
}
