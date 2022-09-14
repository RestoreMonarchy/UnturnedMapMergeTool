using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedMapMergeTool.Unturned.Unity
{
    public class Color
    {
        public Color()
        {

        }

        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public float r { get; set; }
        public float g { get; set; }
        public float b { get; set; }


    }
}
