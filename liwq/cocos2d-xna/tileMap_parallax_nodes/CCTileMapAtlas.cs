using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    /** @brief CCTileMapAtlas is a subclass of CCAtlasNode.

    It knows how to render a map based of tiles.
    The tiles must be in a .PNG format while the map must be a .TGA file.

    For more information regarding the format, please see this post:
    http://www.cocos2d-iphone.org/archives/27

    All features from CCAtlasNode are valid in CCTileMapAtlas

    IMPORTANT:
    This class is deprecated. It is maintained for compatibility reasons only.
    You SHOULD not use this class.
    Instead, use the newer TMX file format: CCTMXTiledMap
    */
    public class CCTileMapAtlas : CCAtlasNode
    {
        tImageTGA m_pTGAInfo;
        /// <summary>
        /// TileMap info
        /// </summary>
        public tImageTGA TGAInfo
        {
            get { return m_pTGAInfo; }
            set { m_pTGAInfo = value; }
        }

        public CCTileMapAtlas()
        { }

        /// <summary>
        /// creates a CCTileMap with a tile file (atlas) with a map file and the width and height of each tile in points.
        /// The tile file will be loaded using the TextureMgr.
        /// </summary>
        public static CCTileMapAtlas tileMapAtlasWithTileFile(string tile, string mapFile, int tileWidth, int tileHeight)
        {
            CCTileMapAtlas pRet = new CCTileMapAtlas();
            if (pRet.initWithTileFile(tile, mapFile, tileWidth, tileHeight))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// initializes a CCTileMap with a tile file (atlas) with a map file and the width and height of each tile in points.
        ///	The file will be loaded using the TextureMgr.
        /// </summary>
        public bool initWithTileFile(string tile, string mapFile, int tileWidth, int tileHeight)
        {
            this.loadTGAfile(mapFile);
            this.calculateItemsToRender();
            if (base.initWithTileFile(tile, tileWidth, tileHeight, m_nItemsToRender))
            {
                m_pPosToAtlasIndex = new Dictionary<string, int>();
                this.updateAtlasValues();
                this.contentSize = new CCSize((float)(m_pTGAInfo.width * m_uItemWidth),
                                                (float)(m_pTGAInfo.height * m_uItemHeight));
                return true;
            }
            return false;
        }

        /// <summary>
        /// returns a tile from position x,y.
        /// For the moment only channel R is used
        /// </summary>
        public ccColor3B tileAt(ccGridSize position)
        {
            throw new NotImplementedException();
        }
        /** sets a tile at position x,y.
        For the moment only channel R is used
        */
        void setTile(ccColor3B tile, ccGridSize position) { }

        /// <summary>
        /// dealloc the map from memory
        /// </summary>
        public void releaseMap()
        {
            m_pTGAInfo = null;
            m_pPosToAtlasIndex = null;
        }

        private void loadTGAfile(string file)
        {
            Debug.Assert(!string.IsNullOrEmpty(file), "file must be non-nil");

            //	//Find the path of the file
            //	NSBundle *mainBndl = [CCDirector sharedDirector].loadingBundle;
            //	CCString *resourcePath = [mainBndl resourcePath];
            //	CCString * path = [resourcePath stringByAppendingPathComponent:file];

            //    m_pTGAInfo = tImageTGA.tgaLoad( CCFileUtils.fullPathFromRelativePath(file) );
            //#if 1
            //    if( m_pTGAInfo->status != TGA_OK ) 
            //    {
            //        CCAssert(0, "TileMapAtlasLoadTGA : TileMapAtas cannot load TGA file");
            //    }
            //#endif
        }

        private void calculateItemsToRender()
        {
            Debug.Assert(m_pTGAInfo != null, "tgaInfo must be non-nil");

            m_nItemsToRender = 0;
            for (int x = 0; x < m_pTGAInfo.width; x++)
            {
                for (int y = 0; y < m_pTGAInfo.height; y++)
                {
                    ccColor3B ptr = new ccColor3B() { r = m_pTGAInfo.imageData[0], g = m_pTGAInfo.imageData[1], b = m_pTGAInfo.imageData[2] };
                    //ccColor3B value = ptr[x + y * m_pTGAInfo.width];
                    //if (value.r)
                    //{
                    //    ++m_nItemsToRender;
                    //}
                }
            }
        }
        private void updateAtlasValueAt(ccGridSize pos, ccColor3B value, int index) { }
        private void updateAtlasValues() { }

        //! x,y to altas dicctionary

        protected Dictionary<string, int> m_pPosToAtlasIndex;
        //! numbers of tiles to render
        protected int m_nItemsToRender;
    };
}
