using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameConstructLibrary
{
    public class GameTip
    {

        public bool Displayed = false;
        public string[] Tips { get; set; }
        public float Time { get; set; }

        public GameTip(string[] tips, float time)
        {
            Tips = tips;
            Time = time;
        }
    }
}
