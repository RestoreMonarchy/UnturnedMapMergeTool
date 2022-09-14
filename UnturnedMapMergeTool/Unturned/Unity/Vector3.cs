using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedMapMergeTool.Unturned.Unity
{
    public class Vector3
    {
        public Vector3()
        {

        }
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public static Vector3 one => new Vector3(1, 1, 1);
    }
}
