using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// A CCMenuItemToggle
    /// A simple container class that "toggles" it's inner items
    /// The inner itmes can be any MenuItem
    /// </summary>
    public class CCMenuItemToggle : CCMenuItem, ICCRGBAProtocol
    {
        private byte m_cOpacity;

        /// <summary>
        /// conforms with CCRGBAProtocol protocol
        /// </summary>
        public byte Opacity
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ccColor3B Color
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private ccColor3B m_tColor;
        /// <summary>
        /// conforms with CCRGBAProtocol protocol
        /// </summary>
        public bool IsOpacityModifyRGB
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private int m_uSelectedIndex;
        /// <summary>
        /// returns the selected item
        /// </summary>
        public int SelectedIndex
        {
            get { return m_uSelectedIndex; }
            set
            {
                if (value != m_uSelectedIndex)
                {
                    m_uSelectedIndex = value;
                }

                this.removeChildByTag(CCMenuItem.kCurrentItem, false);
                CCMenuItem item = m_pSubItems[m_uSelectedIndex];
                this.addChild(item, 0, CCMenuItem.kCurrentItem);
                CCSize s = new CCSize(item.contentSize.width, item.contentSize.height);
                this.contentSize = s;
                item.position = new CCPoint(s.width / 2, s.height / 2);
            }
        }

        public List<CCMenuItem> m_pSubItems;
        /// <summary>
        /// CCMutableArray that contains the subitems. You can add/remove items in runtime, and you can replace the array with a new one.
        /// @since v0.7.2
        /// </summary>
        public List<CCMenuItem> SubItems
        {
            get { return m_pSubItems; }
            set { m_pSubItems = value; }
        }

        public CCMenuItemToggle()
        { }

        /// <summary>
        /// creates a menu item from a list of items with a target/selector
        /// </summary>
        public static CCMenuItemToggle itemWithTarget(SelectorProtocol target, SEL_MenuHandler selector, params CCMenuItem[] items)
        {
            CCMenuItemToggle pRet = new CCMenuItemToggle();
            pRet.initWithTarget(target, selector, items);

            return pRet;
        }

        /// <summary>
        /// initializes a menu item from a list of items with a target selector
        /// </summary>
        public bool initWithTarget(SelectorProtocol target, SEL_MenuHandler selector, CCMenuItem[] items)
        {
            base.initWithTarget(target, selector);
            this.m_pSubItems = new List<CCMenuItem>();
            foreach (var item in items)
            {
                m_pSubItems.Add(item);
            }

            this.SelectedIndex = 0;
            return true;
        }

        /// <summary>
        /// The follow methods offered to lua
        /// creates a menu item with a item
        /// </summary>
        public static CCMenuItemToggle itemWithItem(CCMenuItem item)
        {
            CCMenuItemToggle pRet = new CCMenuItemToggle();
            pRet.initWithItem(item);
            return pRet;
        }

        /// <summary>
        /// initializes a menu item with a item
        /// </summary>
        public bool initWithItem(CCMenuItem item)
        {
            base.initWithTarget(null, null);
            this.m_pSubItems = new List<CCMenuItem>();
            m_pSubItems.Add(item);
            this.SelectedIndex = 0;
            return true;
        }

        /// <summary>
        /// add more menu item
        /// </summary>
        public void addSubItem(CCMenuItem item)
        {
            this.m_pSubItems.Add(item);
        }

        /// <summary>
        /// return the selected item
        /// </summary>
        public CCMenuItem selectedItem()
        {
            return m_pSubItems[m_uSelectedIndex];
        }

        // super methods
        public override void activate()
        {
            // update index
            if (m_bIsEnabled)
            {
                int newIndex = (m_uSelectedIndex + 1) % m_pSubItems.Count;
                this.SelectedIndex = newIndex;
            }
            base.activate();
        }

        public override void selected()
        {
            base.selected();
            m_pSubItems[m_uSelectedIndex].selected();
        }

        public override void unselected()
        {
            base.unselected();
            m_pSubItems[m_uSelectedIndex].unselected();
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                foreach (var item in m_pSubItems)
                {
                    item.Enabled = value;
                }
            }
        }
    }
}
