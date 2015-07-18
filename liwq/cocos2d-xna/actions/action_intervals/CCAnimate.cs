using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
{
    /** @brief Animates a sprite given the name of an Animation */
    public class CCAnimate : CCActionInterval
    {
        CCAnimation m_pAnimation;
        CCSpriteFrame m_pOrigFrame;
        bool m_bRestoreOriginalFrame;

        public static CCAnimate actionWithAnimation(CCAnimation pAnimation)
        {
            CCAnimate pAnimate = new CCAnimate();
            pAnimate.initWithAnimation(pAnimation, true);

            return pAnimate;
        }

        public bool initWithAnimation(CCAnimation pAnimation)
        {
            Debug.Assert(pAnimation != null);

            return initWithAnimation(pAnimation, true);
        }

        public static CCAnimate actionWithAnimation(CCAnimation pAnimation, bool bRestoreOriginalFrame)
        {
            CCAnimate pAnimate = new CCAnimate();
            pAnimate.initWithAnimation(pAnimation, bRestoreOriginalFrame);

            return pAnimate;
        }

        public bool initWithAnimation(CCAnimation pAnimation, bool bRestoreOriginalFrame)
        {
            Debug.Assert(pAnimation != null);

            if (base.initWithDuration(pAnimation.getFrames().Count * pAnimation.getDelay()))
            {
                m_bRestoreOriginalFrame = bRestoreOriginalFrame;
                m_pAnimation = pAnimation;
                m_pOrigFrame = null;

                return true;
            }

            return false;
        }

        public static CCAnimate actionWithDuration(float duration, CCAnimation pAnimation, bool bRestoreOriginalFrame)
        {
            CCAnimate pAnimate = new CCAnimate();
            pAnimate.initWithDuration(duration, pAnimation, bRestoreOriginalFrame);

            return pAnimate;
        }

        public bool initWithDuration(float duration, CCAnimation pAnimation, bool bRestoreOriginalFrame)
        {
            Debug.Assert(pAnimation != null);

            if (base.initWithDuration(duration))
            {
                m_bRestoreOriginalFrame = bRestoreOriginalFrame;
                m_pAnimation = pAnimation;
                m_pOrigFrame = null;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCAnimate pCopy = null;
            if(pZone !=null && pZone.m_pCopyObject != null) 
            {
                //in case of being called at sub class
                pCopy = (CCAnimate)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCAnimate();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(Duration, m_pAnimation, m_bRestoreOriginalFrame);

            return pCopy;
        }

        ~CCAnimate()
        {
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            CCSprite pSprite = (CCSprite)(pTarget);


            if (m_bRestoreOriginalFrame)
            {
                // original code : m_pOrigFrame = pSprite->displayedFrame();
                m_pOrigFrame = pSprite.DisplayFrame;
            }
        }

        public override void Stop()
        {
            if (m_bRestoreOriginalFrame && Target != null)
            {
                // original code: ((CCSprite*)(m_pTarget))->setDisplayFrame(m_pOrigFrame);
                ((CCSprite)(Target)).DisplayFrame=m_pOrigFrame;
            }

            base.Stop();
        }

        public override void Update(float time)
        {
            List<CCSpriteFrame> pFrames = m_pAnimation.getFrames();
            int numberOfFrames = pFrames.Count;

            int idx = (int)(time * numberOfFrames);

            if (idx >= numberOfFrames)
            {
                idx = numberOfFrames - 1;
            }

            CCSprite pSprite = (CCSprite)(Target);
            if (! pSprite.isFrameDisplayed(pFrames[idx]))
            {
                pSprite.DisplayFrame=pFrames[idx];
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            List<CCSpriteFrame> pOldArray = m_pAnimation.getFrames();
            List<CCSpriteFrame> pNewArray = new List<CCSpriteFrame>(pOldArray.Count);
   
            if (pOldArray.Count > 0)
            {
                CCSpriteFrame pElement;
                for (int nIndex = pOldArray.Count - 1; nIndex >= 0; nIndex--)
                {
                    pElement = pOldArray[nIndex];
                    if (null == pElement)
                    {
                        break;
                    }

                    pNewArray.Insert(pOldArray.Count - 1 - nIndex, (CCSpriteFrame)(pElement.copy()));
                }

            }

            CCAnimation pNewAnim = CCAnimation.animationWithFrames(pNewArray, m_pAnimation.getDelay());

            return CCAnimate.actionWithDuration(Duration, pNewAnim, m_bRestoreOriginalFrame);
        }


    }
}
