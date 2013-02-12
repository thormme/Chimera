using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface;
using Microsoft.Xna.Framework;

namespace WorldEditor.Dialogs
{
    public class HeightDialog : Dialog
    {

        private LabelControl mSizeLabel = new LabelControl();
        private InputControl mSizeInput = new InputControl();

        private LabelControl mIntensityLabel = new LabelControl();
        private InputControl mIntensityInput = new InputControl();

        private LabelControl mFeatherLabel = new LabelControl();
        private OptionControl mFeatherOption = new OptionControl();

        private LabelControl mSetHeightLabel = new LabelControl();
        private OptionControl mSetOption = new OptionControl();

        private ButtonControl mEditButton = new ButtonControl();
        private ButtonControl mBackButton = new ButtonControl();

        public HeightDialog()
            : base()
        {

            mSizeLabel.Text = "Size:";
            mSizeLabel.Bounds = new UniRectangle(40.0f, 40.0f, 120.0f, 40.0f);
            Children.Add(mSizeLabel);
            mSizeInput.Text = "0:";
            mSizeInput.Bounds = new UniRectangle(200.0f, 40.0f, 120.0f, 40.0f);
            Children.Add(mSizeInput);

            mBounds = new Rectangle(10, 10, 200, 200);

        }

    }
}
