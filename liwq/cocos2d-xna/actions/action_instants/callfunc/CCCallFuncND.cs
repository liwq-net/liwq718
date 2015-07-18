using System;
namespace cocos2d
{
    /** 
    @brief Calls a 'callback' with the node as the first argument and the 2nd argument is data
    * ND means: Node and Data. Data is void *, so it could be anything.
    */
    public class CCCallFuncND : CCCallFuncN
    {
        public static CCCallFuncND actionWithTarget(SelectorProtocol pSelectorTarget,
               SEL_CallFuncND selector, object d)
        {
            CCCallFuncND pRet = new CCCallFuncND();

            if (pRet != null && pRet.initWithTarget(pSelectorTarget, selector, d))
            {
                return pRet;
            }

            return null;
        }

        // todo
        //public static CCCallFuncND actionWithScriptFuncName(string pszFuncName, object d) 
        //{
        //    CCCallFuncND pRet = new CCCallFuncND();

        //    if (pRet != null && pRet.initWithScriptFuncName(pszFuncName)) 
        //    {
        //        pRet.m_pData = d;
        //        return pRet;
        //    }

        //    return null;
        //}


        public bool initWithTarget(SelectorProtocol pSelectorTarget,
                SEL_CallFuncND selector, object d)
        {
            if (base.initWithTarget(pSelectorTarget))
            {
                m_pData = d;
                m_pCallFuncND = selector;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone pNewZone = null;
            CCCallFuncND pRet = null;

            if (zone != null && zone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pRet = (CCCallFuncND)(zone.m_pCopyObject);
            }
            else
            {
                pRet = new CCCallFuncND();
                zone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(zone);
            pRet.initWithTarget(m_pSelectorTarget, m_pCallFuncND, m_pData);
            return pRet;
        }

        public override void execute()
        {
            if (null != m_pCallFuncND)
            {
                m_pCallFuncND(Target, m_pData);
            }

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFuncND(
            //            m_scriptFuncName.c_str(), m_pTarget, m_pData);
            //}
        }

        protected object m_pData;

        protected SEL_CallFuncND m_pCallFuncND;

    }
}