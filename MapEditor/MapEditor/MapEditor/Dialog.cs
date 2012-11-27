using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface;

namespace MapEditor
{

    public abstract class Dialog : WindowControl
    {

        protected UniRectangle mHidden;
        protected UniRectangle mBounds;

        protected Dialog()
        {
            mHidden = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
        }

        public void Hide()
        {
            Bounds = mHidden;
        }

        public void Show()
        {
            Bounds = mBounds;
        }
    }
}
