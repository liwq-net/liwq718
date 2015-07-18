using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTMXTilesetInfo : CCObject
    {
        public string m_sName;
        public int m_uFirstGid;
        public CCSize m_tTileSize;
        public int m_uSpacing;
        public int m_uMargin;
        //! filename containing the tiles (should be spritesheet / texture atlas)
        public string m_sSourceImage;
        //! size in pixels of the image
        public CCSize m_tImageSize;

        public CCTMXTilesetInfo()
        {
        }
        public CCRect rectForGID(int gid)
        {
            CCRect rect = new CCRect();
            rect.size = m_tTileSize;
            gid = gid - m_uFirstGid;
            int max_x = (int)((m_tImageSize.width - m_uMargin * 2 + m_uSpacing) / (m_tTileSize.width + m_uSpacing));
            //	int max_y = (imageSize.height - margin*2 + spacing) / (tileSize.height + spacing);
            rect.origin.x = (gid % max_x) * (m_tTileSize.width + m_uSpacing) + m_uMargin;
            rect.origin.y = (gid / max_x) * (m_tTileSize.height + m_uSpacing) + m_uMargin;
            return rect;
        }
    }
}
