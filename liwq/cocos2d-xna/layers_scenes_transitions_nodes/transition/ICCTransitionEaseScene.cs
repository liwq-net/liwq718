using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public interface ICCTransitionEaseScene
    {
        /// <summary>
        ///  returns the Ease action that will be performed on a linear action.
        ///@since v0.8.2
        /// </summary>
        CCFiniteTimeAction easeActionWithAction(CCActionInterval action);
    }
}
