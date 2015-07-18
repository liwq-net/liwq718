using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using liwq;

namespace cocos2d
{
    /// <summary>
    /// Base class for other
    /// </summary>
    public class CCGridBase : CCObject
    {
        #region properties

        protected Texture m_pTexture;
        protected CCGrabber m_pGrabber;

        protected bool m_bActive;
        /// <summary>
        ///  wheter or not the grid is active
        /// </summary>
        public bool Active
        {
            get { return m_bActive; }
            set { m_bActive = value; }
        }

        protected int m_nReuseGrid;
        /// <summary>
        /// number of times that the grid will be reused 
        /// </summary>
        public int ReuseGrid
        {
            get { return m_nReuseGrid; }
            set { m_nReuseGrid = value; }
        }

        protected ccGridSize m_sGridSize;
        /// <summary>
        /// size of the grid 
        /// </summary>
        public ccGridSize GridSize
        {
            get { return m_sGridSize; }
            set { m_sGridSize = value; }
        }

        protected CCPoint m_obStep;
        /// <summary>
        /// pixels between the grids 
        /// </summary>
        public CCPoint Step
        {
            get { return m_obStep; }
            set { m_obStep = value; }
        }

        protected bool m_bIsTextureFlipped;
        /// <summary>
        /// is texture flipped 
        /// </summary>
        public bool TextureFlipped
        {
            get { return m_bIsTextureFlipped; }
            set { m_bIsTextureFlipped = value; }
        }

        #endregion

        public static CCGridBase gridWithSize(ccGridSize gridSize, Texture texture, bool flipped)
        {
            CCGridBase pGridBase = new CCGridBase();

            if (pGridBase.initWithSize(gridSize, texture, flipped))
            {
                return pGridBase;
            }

            return null;
        }

        public static CCGridBase gridWithSize(ccGridSize gridSize)
        {
            CCGridBase pGridBase = new CCGridBase();

            if (pGridBase.initWithSize(gridSize))
            {
                return pGridBase;
            }

            return null;
        }

        public bool initWithSize(ccGridSize gridSize, Texture pTexture, bool bFlipped)
        {
            bool bRet = true;

            m_bActive = false;
            m_nReuseGrid = 0;
            m_sGridSize = gridSize;

            m_pTexture = pTexture;

            m_bIsTextureFlipped = bFlipped;

            CCSize texSize = m_pTexture.ContentSize;
            m_obStep = new CCPoint();
            m_obStep.X = texSize.Width / (float)m_sGridSize.x;
            m_obStep.Y = texSize.Height / (float)m_sGridSize.y;

            m_pGrabber = new CCGrabber();
            if (m_pGrabber != null)
            {
                m_pGrabber.grab(ref m_pTexture);
            }
            else
            {
                bRet = false;
            }

            calculateVertexPoints();

            return bRet;
        }

        public bool initWithSize(ccGridSize gridSize)
        {
            Director pDirector = Director.SharedDirector;
            CCSize s = pDirector.DisplaySize;

            ulong POTWide = ccNextPOT((uint)s.Width);
            ulong POTHigh = ccNextPOT((uint)s.Height);

            // we only use rgba8888
            Texture pTexture = new Texture((int)POTWide, (int)POTHigh);

            if (pTexture == null)
            {
                System.Diagnostics.Debug.WriteLine("cocos2d: CCGrid: error creating texture");
                return false;
            }

            initWithSize(gridSize, pTexture, false);

            return true;
        }

        public ulong ccNextPOT(ulong x)
        {
            x = x - 1;
            x = x | (x >> 1);
            x = x | (x >> 2);
            x = x | (x >> 4);
            x = x | (x >> 8);
            x = x | (x >> 16);
            return x + 1;
        }

        public void beforeDraw()
        {
            //set2DProjection();
            m_pGrabber.beforeRender(ref m_pTexture);
        }

        public void afterDraw(Node pTarget)
        {
            m_pGrabber.afterRender(ref m_pTexture);

            //set3DProjection();
            //applyLandscape();

            //if (pTarget.getCamera()->getDirty())
            //{
            //    const CCPoint& offset = pTarget->getAnchorPointInPixels();

            //    //
            //    // XXX: Camera should be applied in the AnchorPoint
            //    //
            //    ccglTranslate(offset.x, offset.y, 0);
            //    pTarget->getCamera()->locate();
            //    ccglTranslate(-offset.x, -offset.y, 0);
            //}

            //glBindTexture(GL_TEXTURE_2D, m_pTexture->getName());

            //// restore projection for default FBO .fixed bug #543 #544
            ////CCDirector.SharedDirector.Projection=CCDirector.SharedDirector.Projection;
            //CCDirector.SharedDirector.applyOrientation();

            blit();
        }

        public virtual void blit()
        {
            Debug.Assert(false);
        }

        public virtual void reuse()
        {
            Debug.Assert(false);
        }

        public virtual void calculateVertexPoints()
        {
            Debug.Assert(false);
        }

        public void set2DProjection()
        {
            CCSize winSize = Director.SharedDirector.DisplaySize;

            //glLoadIdentity();

            // set view port for user FBO, fixed bug #543 #544
            //glViewport((GLsizei)0, (GLsizei)0, (GLsizei)winSize.width, (GLsizei)winSize.height);
            //glMatrixMode(GL_PROJECTION);
            //glLoadIdentity();
            //ccglOrtho(0, winSize.width, 0, winSize.height, -1024, 1024);
            //glMatrixMode(GL_MODELVIEW);
        }

        public void set3DProjection()
        {
            CCSize winSize = Director.SharedDirector.DisplaySize;

            //// set view port for user FBO, fixed bug #543 #544
            //glViewport(0, 0, (GLsizei)winSize.width, (GLsizei)winSize.height);
            //glMatrixMode(GL_PROJECTION);
            //glLoadIdentity();
            //gluPerspective(60, (GLfloat)winSize.width/winSize.height, 0.5f, 1500.0f);

            //glMatrixMode(GL_MODELVIEW);	
            //glLoadIdentity();
            //gluLookAt( winSize.width/2, winSize.height/2, CCDirector::sharedDirector()->getZEye(),
            //    winSize.width/2, winSize.height/2, 0,
            //    0.0f, 1.0f, 0.0f
            //    );
        }

        protected void applyLandscape()
        {
            Director pDirector = Director.SharedDirector;

            CCSize winSize = pDirector.DisplaySize;
            float w = winSize.Width / 2;
            float h = winSize.Height / 2;

        }
    }
}
