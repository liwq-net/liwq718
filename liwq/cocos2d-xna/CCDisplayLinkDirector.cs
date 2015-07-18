//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace cocos2d
//{
//    /***************************************************
//    * implementation of DisplayLinkDirector
//    **************************************************/

//    // should we afford 4 types of director ??
//    // I think DisplayLinkDirector is enough
//    // so we now only support DisplayLinkDirector

//    public class CCDisplayLinkDirector : Director
//    {
//        bool m_bInvalid;

//        public override void stopAnimation()
//        {
//            m_bInvalid = true;
//        }

//        public override void startAnimation()
//        {
//            m_bInvalid = false;
//            //sharedDirector().animationInterval = m_dAnimationInterval;
//        }

//        public override void mainLoop(GameTime gameTime)
//        {
//            if (_isPurgeDirecotorInNextLoop)
//            {
//                purgeDirector();
//                _isPurgeDirecotorInNextLoop = false;
//            }
//            else if (!m_bInvalid)
//            {
//                _drawScene(gameTime);
//            }
//        }

//        public override double animationInterval
//        {
//            get
//            {
//                return base.animationInterval;
//            }
//            set
//            {
//                _animationInterval = value;
//                if (!m_bInvalid)
//                {
//                    stopAnimation();
//                    startAnimation();
//                }
//            }
//        }
//    }
//}
