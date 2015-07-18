using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTMXLayerInfo : CCObject
    {
        protected Dictionary<string, string> m_pProperties;
        public virtual Dictionary<string, string> Properties
        {
            get { return m_pProperties; }
            set { m_pProperties = value; }
        }

        public string m_sName;
        public CCSize m_tLayerSize;
        public int[] m_pTiles;
        public bool m_bVisible;
        public byte m_cOpacity;
        public bool m_bOwnTiles;
        public int m_uMinGID;
        public int m_uMaxGID;
        public CCPoint m_tOffset;

        public CCTMXLayerInfo()
        {
            m_sName = "";
            m_pTiles = null;
            m_bOwnTiles = true;
            m_uMinGID = 100000;
            m_uMaxGID = 0;
            m_tOffset = new CCPoint(0, 0);
            m_pProperties = new Dictionary<string, string>(); ;
        }
    }
}
