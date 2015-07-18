using System;
namespace cocos2d
{
    public class CCCallFunc : CCActionInstant
    {
        public CCCallFunc()
        {
            m_pSelectorTarget = null;
            m_scriptFuncName = "";
            m_pCallFunc = null;
        }

        public static CCCallFunc actionWithTarget(SelectorProtocol pSelectorTarget, SEL_CallFunc selector)
        {
            CCCallFunc pRet = new CCCallFunc();

            if (pRet != null && pRet.initWithTarget(pSelectorTarget))
            {
                pRet.m_pCallFunc = selector;
                return pRet;
            }

            return null;
        }

        public virtual bool initWithTarget(SelectorProtocol pSelectorTarget)
        {
            m_pSelectorTarget = pSelectorTarget;
            return true;
        }


        /** executes the callback */
        public virtual void execute()
        {
            if (null != m_pCallFunc)
            {
                m_pCallFunc();
            }

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFunc(
            //            m_scriptFuncName.c_str());
            //}
        }

        //super methods
        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            execute();
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCCallFunc pRet = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pRet = (CCCallFunc)(pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCCallFunc();
                pZone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(pZone);
            pRet.initWithTarget(m_pSelectorTarget);
            pRet.m_pCallFunc = m_pCallFunc;
            pRet.m_scriptFuncName = m_scriptFuncName;
            return pRet;
        }

        // void registerScriptFunction(const char* pszFunctionName);

        public SelectorProtocol getTargetCallback()
        {
            return m_pSelectorTarget;
        }

        public void setTargetCallback(SelectorProtocol pSel)
        {
            if (pSel != m_pSelectorTarget)
            {
                m_pSelectorTarget = pSel;

            }
        }

        /** Target that will be called */
        protected SelectorProtocol m_pSelectorTarget;
        /** the script function name to call back */
        protected string m_scriptFuncName;

        private SEL_CallFunc m_pCallFunc;
    }
}