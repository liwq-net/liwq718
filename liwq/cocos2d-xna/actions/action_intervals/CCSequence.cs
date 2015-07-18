using System.Diagnostics;

namespace cocos2d
{
    /// <summary>
    /// @brief Runs actions sequentially, one after another
    /// </summary>
    public class CCSequence : CCActionInterval
    {
        public CCSequence()
        {
            m_pActions = new CCFiniteTimeAction[2];
        }

        /// <summary>
        /// initializes the action
        /// </summary>
        public bool initOneTwo(CCFiniteTimeAction actionOne, CCFiniteTimeAction aciontTwo)
        {
            Debug.Assert(actionOne != null);
            Debug.Assert(aciontTwo != null);

            float d = actionOne.Duration + aciontTwo.Duration;
            base.initWithDuration(d);

            m_pActions[0] = actionOne;
            m_pActions[1] = aciontTwo;

            return true;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCSequence ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCSequence;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCSequence();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            CCFiniteTimeAction param1 = m_pActions[0].copy() as CCFiniteTimeAction;
            CCFiniteTimeAction param2 = m_pActions[1].copy() as CCFiniteTimeAction;

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
            m_split = m_pActions[0].Duration / Duration;
            m_last = -1;
        }

        public override void Stop()
        {
            m_pActions[0].Stop();
            m_pActions[1].Stop();
            base.Stop();
        }

        public override void Update(float dt)
        {
            int found = 0;
            float new_t = 0.0f;

            if (dt >= m_split)
            {
                found = 1;
                if (m_split == 1)
                {
                    new_t = 1;
                }
                else
                {
                    new_t = (dt - m_split) / (1 - m_split);
                }
            }
            else
            {
                found = 0;
                if (m_split != 0)
                {
                    new_t = dt / m_split;
                }
                else
                {
                    new_t = 1;
                }
            }

            if (m_last == -1 && found == 1)
            {
                m_pActions[0].StartWithTarget(Target);
                m_pActions[0].Update(1.0f);
                m_pActions[0].Stop();
            }

            if (m_last != found)
            {
                if (m_last != -1)
                {
                    m_pActions[m_last].Update(1.0f);
                    m_pActions[m_last].Stop();
                }

                m_pActions[found].StartWithTarget(Target);
            }

            m_pActions[found].Update(new_t);
            m_last = found;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCSequence.actionOneTwo(m_pActions[1].Reverse(), m_pActions[0].Reverse());
        }

        /// <summary>
        /// helper constructor to create an array of sequenceable actions
        /// </summary>
        public static CCFiniteTimeAction actions(params CCFiniteTimeAction[] actions)
        {
            return actionsWithArray(actions);
        }

        /// <summary>
        /// helper contructor to create an array of sequenceable actions given an array
        /// </summary>
        public static CCFiniteTimeAction actionsWithArray(CCFiniteTimeAction[] actions)
        {
            CCFiniteTimeAction prev = actions[0];

            for (int i = 1; i < actions.Length; i++)
            {
                if (actions[i] == null)
                {
                    continue;
                }
                prev = actionOneTwo(prev, actions[i]);
            }

            return prev;
        }

        public static CCSequence actionOneTwo(CCFiniteTimeAction actionOne, CCFiniteTimeAction actionTwo)
        {
            CCSequence sequence = new CCSequence();
            sequence.initOneTwo(actionOne, actionTwo);

            return sequence;
        }

        protected CCFiniteTimeAction[] m_pActions;
        protected float m_split;
        protected int m_last;
    }
}
