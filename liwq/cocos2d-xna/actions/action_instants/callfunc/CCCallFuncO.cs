using System;
namespace cocos2d
{
    /**
    @brief Calls a 'callback' with an object as the first argument.
    O means Object.
    @since v0.99.5
    */
    public class CCCallFuncO : CCCallFunc
    {

        public CCCallFuncO()
        {
            m_pObject = null;
            m_pCallFuncO = null;
        }

        ~CCCallFuncO()
        {
        }

        public static CCCallFuncO actionWithTarget(SelectorProtocol pSelectorTarget, SEL_CallFuncO selector, CCObject pObject)
        {
            CCCallFuncO pRet = new CCCallFuncO();

            if (pRet != null && pRet.initWithTarget(pSelectorTarget, selector, pObject))
            {
                return pRet;
            }

            return null;
        }

        // todo
        //public static CCCallFuncO actionWithScriptFuncName(string pszFuncName)
        //{
        //    CCCallFuncO pRet = new CCCallFuncO();

        //    if (pRet != null && pRet.initWithScriptFuncName(pszFuncName)) {
        //        return pRet;
        //    }

        //    return null;
        //}

        public bool initWithTarget(SelectorProtocol pSelectorTarget, SEL_CallFuncO selector, CCObject pObject)
        {
            if (base.initWithTarget(pSelectorTarget))
            {
                m_pObject = pObject;
                m_pCallFuncO = selector;
                return true;
            }

            return false;
        }

        // super methods
        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone pNewZone = null;
            CCCallFuncO pRet = null;

            if (zone != null && zone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pRet = (CCCallFuncO)(zone.m_pCopyObject);
            }
            else
            {
                pRet = new CCCallFuncO();
                zone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(zone);
            pRet.initWithTarget(m_pSelectorTarget, m_pCallFuncO, m_pObject);
            return pRet;
        }

        public override void execute()
        {
            if (null != m_pCallFuncO)
            {
                m_pCallFuncO(m_pObject);
            }

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFunc0(
            //            m_scriptFuncName.c_str(), m_pObject);
            //}
        }

        public CCObject getObject()
        {
            return m_pObject;
        }

        public void setObject(CCObject pObj)
        {
            if (pObj != m_pObject)
            {
                m_pObject = pObj;
            }
        }

        private CCObject m_pObject;
        private SEL_CallFuncO m_pCallFuncO;
    }
}