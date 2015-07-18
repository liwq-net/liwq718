using System;
using System.Diagnostics;
namespace cocos2d
{
    /// <summary>
    /// @brief An interval action is an action that takes place within a certain period of time.
    ///  It has an start time, and a finish time. The finish time is the parameter
    /// duration plus the start time.
    /// </summary>
    /// <remarks>
    /// These CCActionInterval actions have some interesting properties, like:
    /// - They can run normally (default)
    /// - They can run reversed with the reverse method
    /// - They can run with the time altered with the Accelerate, AccelDeccel and Speed actions.
    ///    For example, you can simulate a Ping Pong effect running the action normally and
    ///  then running it again in Reverse mode.
    /// </remarks>
    ///<example>
    ///  CCAction *pingPongAction = CCSequence::actions(action, action->reverse(), NULL);
    /// </example>
    public class CCActionInterval : CCFiniteTimeAction
    {
        /// <summary>
        /// initializes the action
        /// </summary>
        public bool initWithDuration(float d)
        {
            Duration = d;

            // prevent division by 0
            // This comparison could be in step:, but it might decrease the performance
            // by 3% in heavy based action games.
            if (Duration == 0)
            {
                Duration = (float)ccMacros.FLT_EPSILON;
            }

            m_elapsed = 0;
            m_bFirstTick = true;

            return true;
        }

        /// <summary>
        /// returns true if the action has finished
        /// </summary>
        public override bool IsDone()
        {
            return m_elapsed >= Duration;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCActionInterval ret = null;
            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = (CCActionInterval)(tmpZone.m_pCopyObject);
            }
            else
            {
                // action's base class , must be called using __super::copyWithZone(), after overriding from derived class
                Debug.Assert(false);

                ret = new CCActionInterval();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(Duration);

            return ret;
        }

        public override void Step(float dt)
        {
            if (m_bFirstTick)
            {
                m_bFirstTick = false;
                m_elapsed = 0;
            }
            else
            {
                m_elapsed += dt;
            }

            Update(Math.Min(1, m_elapsed / Duration));
        }

        protected void startWithTargetUsedByCCOrbitCamera(Node target)
        {
            base.StartWithTarget(target);
            m_elapsed = 0.0f;
            m_bFirstTick = true;
        }

        public override void StartWithTarget(Node target)
        {
            base.StartWithTarget(target);
            m_elapsed = 0.0f;
            m_bFirstTick = true;
        }

        /// <summary>
        /// C# cannot return type of sub class if it override father function.
        /// In c++ this function return CCActionInterval, I don't know if it
        /// will be a problem. 
        /// Fix me if needed.
        /// </summary>
        public override CCFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }

        public void setAmplitudeRate(float amp)
        {
            throw new NotImplementedException();
        }

        public float getAmplitudeRate()
        {
            throw new NotImplementedException();
        }


        #region properties

        /** how many seconds had elapsed since the actions started to run. */
        protected float m_elapsed;
        public float elapsed
        {
            // read only
            get
            {
                return m_elapsed;
            }
        }

        #endregion

        protected bool m_bFirstTick;
    }
}
