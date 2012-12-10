using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameConstructLibrary.Menu
{
    public abstract class SelectableItem : IMenuItem
    {
        private bool mSelected = false;
        public bool Selected
        {
            get
            {
                return mSelected;
            }
            set
            {
                if (mSelected && !value)
                {
                    OnDeselect();
                }
                else if (!mSelected && value)
                {
                    OnSelect();
                }

                mSelected = value;
            }
        }

        public abstract void OnSelect();
        public abstract void OnDeselect();

        public GameMenu Menu { get; set; }

        public abstract void Update(Microsoft.Xna.Framework.GameTime gameTime);

        public abstract void Render();
    }
}
