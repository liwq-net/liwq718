using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace cocos2d
{
    /// <summary>
    /// @brief CCLayer is a subclass of CCNode that implements the TouchEventsDelegate protocol.
    /// All features from CCNode are valid, plus the following new features:
    /// It can receive iPhone Touches
    /// It can receive Accelerometer input
    /// </summary>
    public class CCLayer : Node, ICCTargetedTouchDelegate, ICCStandardTouchDelegate, ICCTouchDelegate
    {
        public CCLayer()
        {
            AnchorPoint = CCPointExtension.ccp(0.5f, 0.5f);
            IsRelativeAnchorPoint = false;
        }

        public static new CCLayer node()
        {
            CCLayer ret = new CCLayer();
            if (ret.init())
            {
                return ret;
            }

            return null;
        }

        public virtual bool init()
        {
            bool bRet = false;
            do
            {
                Director director = Director.SharedDirector;
                if (director == null)
                {
                    break;
                }

                ContentSize = director.DesignSize;
                m_bIsTouchEnabled = false;
                m_bIsAccelerometerEnabled = false;

                bRet = true;
            } while (false);

            return bRet;
        }

        public override void onEnter()
        {
            // register 'parent' nodes first
            // since events are propagated in reverse order
            if (m_bIsTouchEnabled)
            {
                registerWithTouchDispatcher();
            }

            // then iterate over all the children
            base.onEnter();

            // add this layer to concern the Accelerometer Sensor

            if (m_bIsAccelerometerEnabled)
            {

                ///@todo
                throw new NotImplementedException();
            }

            // add this layer to concern the kaypad msg
            if (m_bIsKeypadEnabled)
            {

            }
        }
        public override void onExit()
        {
            if (m_bIsTouchEnabled)
            {
                CCTouchDispatcher.sharedDispatcher().removeDelegate(this);
            }

            // remove this layer from the delegates who concern Accelerometer Sensor
            if (m_bIsAccelerometerEnabled)
            {
                ///@todo
                throw new NotImplementedException();
            }

            // remove this layer from the delegates who concern the kaypad msg
            if (m_bIsKeypadEnabled)
            {

            }

            base.onExit();
        }
        public override void onEnterTransitionDidFinish()
        {
            if (m_bIsAccelerometerEnabled)
            {
                ///@todo
                throw new NotImplementedException();
            }

            base.onEnterTransitionDidFinish();
        }

        #region touches

        public virtual bool ccTouchBegan(CCTouch touch, CCEvent event_)
        {
            // Debug.Assert(false, "Layer#ccTouchBegan override me");
            return true;
        }
        public virtual void ccTouchMoved(CCTouch touch, CCEvent event_)
        {
        }
        public virtual void ccTouchEnded(CCTouch touch, CCEvent event_)
        {
        }
        public virtual void ccTouchCancelled(CCTouch touch, CCEvent event_)
        {
        }
        public virtual void ccTouchesBegan(List<CCTouch> touches, CCEvent event_)
        {
        }
        public virtual void ccTouchesMoved(List<CCTouch> touches, CCEvent event_)
        {
        }
        public virtual void ccTouchesEnded(List<CCTouch> touches, CCEvent event_)
        {
        }
        public virtual void ccTouchesCancelled(List<CCTouch> touches, CCEvent event_)
        {
        }

        #endregion

        /// <summary>
        /// @todo
        /// </summary>
        //virtual void didAccelerate(CCAcceleration* pAccelerationValue) {CC_UNUSED_PARAM(pAccelerationValue);}

        /** If isTouchEnabled, this method is called onEnter. Override it to change the
	    way CCLayer receives touch events.
	    ( Default: CCTouchDispatcher::sharedDispatcher()->addStandardDelegate(this,0); )
	    Example:
	    void CCLayer::registerWithTouchDispatcher()
	    {
	    CCTouchDispatcher::sharedDispatcher()->addTargetedDelegate(this,INT_MIN+1,true);
	    }
	    @since v0.8.0
	    */
        public virtual void registerWithTouchDispatcher()
        {
            ///@todo
            CCTouchDispatcher.sharedDispatcher().addStandardDelegate(this, 0);
        }

        #region Properties

        protected bool m_bIsTouchEnabled;
        /// <summary>
        /// whether or not it will receive Touch events.
        /// You can enable / disable touch events with this property.
        /// Only the touches of this node will be affected. This "method" is not propagated to it's children.
        /// @since v0.8.1
        /// </summary>
        public bool isTouchEnabled
        {
            get
            {
                return m_bIsTouchEnabled;
            }
            set
            {
                if (m_bIsTouchEnabled != value)
                {
                    m_bIsTouchEnabled = value;
                    if (IsRunning)
                    {
                        if (value)
                        {
                            registerWithTouchDispatcher();
                        }
                        else
                        {
                            CCTouchDispatcher.sharedDispatcher().removeDelegate(this);
                        }
                    }
                }
            }
        }

        private bool m_bIsAccelerometerEnabled;
        /// <summary>
        /// whether or not it will receive Accelerometer events
        /// You can enable / disable accelerometer events with this property.
        /// @since v0.8.1
        /// </summary>
        public bool isAccelerometerEnabled
        {
            get
            {
                return m_bIsAccelerometerEnabled;
            }
            set
            {
                if (value != m_bIsAccelerometerEnabled)
                {
                    m_bIsAccelerometerEnabled = value;
                    if (IsRunning)
                    {
                        if (value)
                        {
                            ///@todo
                            throw new NotImplementedException();
                        }
                        else
                        {
                            ///@todo
                            throw new NotImplementedException();
                        }
                    }
                }
            }
        }

        private bool m_bIsKeypadEnabled;
        /// <summary>
        /// whether or not it will receive keypad events
        /// You can enable / disable accelerometer events with this property.
        /// it's new in cocos2d-x
        /// </summary>
        public bool isKeypadEnabled
        {
            get
            {
                return m_bIsKeypadEnabled;
            }
            set
            {
                if (value != m_bIsKeypadEnabled)
                {
                    m_bIsKeypadEnabled = value;
                    if (IsRunning)
                    {
                        if (value)
                        {
                            ///@todo
                            throw new NotImplementedException();
                        }
                        else
                        {
                            ///@todo
                            throw new NotImplementedException();
                        }
                    }
                }
            }
        }

        #endregion
    }
}
