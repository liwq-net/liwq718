using System;
namespace cocos2d
{
    /** 
    @brief Calls a 'callback' with the node as the first argument
    N means Node
    */
    public class CCCallFuncN : CCCallFunc
    {

        public CCCallFuncN()
        {
            m_pCallFuncN = null;
        }

        ~CCCallFuncN()
        {
        }

        public static CCCallFuncN actionWithTarget(SelectorProtocol pSelectorTarget, SEL_CallFuncN selector)
        {
            CCCallFuncN pRet = new CCCallFuncN();

            if (pRet != null && pRet.initWithTarget(pSelectorTarget, selector))
            {
                return pRet;
            }

            return null;
        }

        // todo
        //public static CCCallFuncN actionWithScriptFuncName(string pszFuncName) 
        //{
        //    CCCallFuncN pRet = new CCCallFuncN();

        //    if (pRet && pRet->initWithScriptFuncName(pszFuncName)) {
        //        pRet->autorelease();
        //        return pRet;
        //    }

        //    CC_SAFE_DELETE(pRet);
        //    return NULL;
        //}

        public bool initWithTarget(SelectorProtocol pSelectorTarget, SEL_CallFuncN selector)
        {
            if (base.initWithTarget(pSelectorTarget))
            {
                m_pCallFuncN = selector;
                return true;
            }

            return false;
        }

        // super methods

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone pNewZone = null;
            CCCallFuncN pRet = null;

            if (zone != null && zone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pRet = (CCCallFuncN)(zone.m_pCopyObject);
            }
            else
            {
                pRet = new CCCallFuncN();
                zone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(zone);
            pRet.initWithTarget(m_pSelectorTarget, m_pCallFuncN);
            return pRet;
        }

        public override void execute()
        {
            if (null != m_pCallFuncN)
            {
                m_pCallFuncN(Target);
            }

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFuncN(
            //            m_scriptFuncName.c_str(), m_pTarget);
            //}
        }

        private SEL_CallFuncN m_pCallFuncN;
    }
}