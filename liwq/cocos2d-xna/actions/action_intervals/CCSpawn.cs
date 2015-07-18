using System.Diagnostics;
using System;
namespace cocos2d
{
    /** @brief Spawn a new action immediately */
    public class CCSpawn : CCActionInterval
    {
        /** helper constructor to create an array of spawned actions */
        public static CCFiniteTimeAction actions(params CCFiniteTimeAction[] actions)
        {
            return actionsWithArray(actions);
        }

        /** helper contructor to create an array of spawned actions given an array */
        public static CCFiniteTimeAction actionsWithArray(CCFiniteTimeAction[] actions)
        {
            CCFiniteTimeAction prev = actions[0];

            for (int i = 1; i < actions.Length; i++)
            {
                prev = actionOneTwo(prev, actions[i]);
            }

            return prev;
        }

        /** creates the Spawn action */
        public static CCSpawn actionOneTwo(CCFiniteTimeAction action1, CCFiniteTimeAction action2)
        {
            CCSpawn spawn = new CCSpawn();
            spawn.initOneTwo(action1, action2);

            return spawn;
        }

        public bool initOneTwo(CCFiniteTimeAction action1, CCFiniteTimeAction action2)
        {
            Debug.Assert(action1 != null);
            Debug.Assert(action2 != null);

            bool bRet = false;

            float d1 = action1.Duration;
            float d2 = action2.Duration;

            if (base.initWithDuration(Math.Max(d1, d2)))
            {
                m_pOne = action1;
                m_pTwo = action2;

                if (d1 > d2)
                {
                    m_pTwo = CCSequence.actionOneTwo(action2, CCDelayTime.actionWithDuration(d1 - d2));
                }
                else if (d1 < d2)
                {
                    m_pOne = CCSequence.actionOneTwo(action1, CCDelayTime.actionWithDuration(d2 - d1));
                }

                bRet = true;
            }

            return bRet;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCSpawn ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCSpawn;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCSpawn();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            CCFiniteTimeAction param1 = m_pOne.copy() as CCFiniteTimeAction;
            CCFiniteTimeAction param2 = m_pTwo.copy() as CCFiniteTimeAction;
            if (param1 == null || param2 == null)
            {
                return null;
            }

            ret.initOneTwo(param1, param2);

            return ret;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            m_pOne.StartWithTarget(target);
            m_pTwo.StartWithTarget(target);
        }

        public override void Stop()
        {
            m_pOne.Stop();
            m_pTwo.Stop();
            base.Stop();
        }

        public override void Update(float dt)
        {
            if (m_pOne != null)
            {
                m_pOne.Update(dt);
            }

            if (m_pTwo != null)
            {
                m_pTwo.Update(dt);
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCSpawn.actionOneTwo(m_pOne.Reverse(), m_pTwo.Reverse());
        }

        protected CCFiniteTimeAction m_pOne = new CCFiniteTimeAction();
        protected CCFiniteTimeAction m_pTwo = new CCFiniteTimeAction();
    }
}
